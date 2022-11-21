using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{
    [SerializeField] private AudioClip[] axeSounds;
    private Animator axeAnim;
    private AudioSource source;
    private bool canGetResource;        // Control for getting one resource + playing sound once. 
    private bool isChopping = false;    // Control for animation and allowing gathering
    
    // Start is called before the first frame update
    void Start()
    {
        axeAnim = gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        canGetResource = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && !isChopping)
        {
            isChopping = true;
            // GameManager.SwingTool("axes");
            axeAnim.SetTrigger("Swing");
            StartCoroutine(ResetAttackingBool());
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(isChopping && canGetResource && other.tag == "Wood")
        {
            canGetResource = false;
            PlayAxeSound();
            Debug.Log(other.name);
            // Adding to inventory goes here.
        }
    }
    
    void OnEnable()
    {
        isChopping = false;
    }

    private void PlayAxeSound()
    {
        int n = Random.Range(1, axeSounds.Length);
        source.clip = axeSounds[n];
        source.PlayOneShot(source.clip);
        // move picked sound to index 0 so it's not picked next time
        axeSounds[n] = axeSounds[0];
        axeSounds[0] = source.clip;
    }

    // Set to the length of axeAnim for use of isChopping
    IEnumerator ResetAttackingBool()
    {
        yield return new WaitForSeconds(0.75f);
        isChopping = false;
        canGetResource = true;
    }
}
