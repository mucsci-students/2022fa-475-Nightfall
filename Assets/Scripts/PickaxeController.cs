using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MonoBehaviour
{
    [SerializeField] private AudioClip[] miningSounds;
    private Animator axeAnim;
    private AudioSource source;
    private bool canGetResource;        // Control for getting one resource + playing sound once. 
    private bool isMining = false;      // Control for animation and allowing gathering
    
    // Start is called before the first frame update
    void Start()
    {
        axeAnim = gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && !isMining)
        {
            isMining = true;
            axeAnim.SetTrigger("Swing");
            StartCoroutine(ResetAttackingBool());
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(isMining && canGetResource && other.tag == "Stone")
        {
            PlayMiningSound();
            canGetResource = false;
            Debug.Log(other.name);
            // Adding to inventory here
        }
    }

    void OnEnable()
    {
        isMining = false;
    }

    private void PlayMiningSound()
    {
        int n = Random.Range(1, miningSounds.Length);
        source.clip = miningSounds[n];
        source.PlayOneShot(source.clip);
        // move picked sound to index 0 so it's not picked next time
        miningSounds[n] = miningSounds[0];
        miningSounds[0] = source.clip;
    }


    // Set to the length of mining animation for use of isMining
    IEnumerator ResetAttackingBool ()
    {
        yield return new WaitForSeconds(1.0f);
        canGetResource = true;
        isMining = false;
    }
}
