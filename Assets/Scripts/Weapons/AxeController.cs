using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{
    [SerializeField] private AudioClip[] axeSounds;
    private Animator hatchetAnim;
    private AudioSource source;
    private bool canGetResource;        // Control for getting one resource + playing sound once. 
    private bool isChopping = false;    // Control for animation and allowing gathering
    private ParticleSystem woodChips;

    // Start is called before the first frame update
    void Start()
    {
        hatchetAnim = GameObject.Find("Male").GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        canGetResource = true;
        woodChips = GetComponentInChildren<ParticleSystem>(true);
    }

    public void SwingAxe()
    {
        if(!isChopping)
        {
            isChopping = true;
            // GameManager.SwingTool("axes");
            hatchetAnim.SetTrigger("Chop Swing");
            StartCoroutine(ResetAttackingBool());
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(isChopping && canGetResource && other.tag == "Wood")
        {
            canGetResource = false;
            PlayAxeSound();
            Inventory.AddItem("Wood" , 1);
            print("Wood: " + Inventory.GetCount("Wood"));
            woodChips.Play();

            if (other.TryGetComponent(out ResourceHealth resourceHealth))
            {
                // Will change the passed float to tool type dmg
                resourceHealth.SubtractHealth(1.0f);
                print("player position: " + gameObject.transform.position);
                print("Resource position: " + other.gameObject.transform.position);
                print(other.transform.position - gameObject.transform.position);
            }

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

    // Set to the length of hatchetAnim for use of isChopping
    IEnumerator ResetAttackingBool()
    {
        yield return new WaitForSeconds(1.0f);
        isChopping = false;
        canGetResource = true;
    }
}
