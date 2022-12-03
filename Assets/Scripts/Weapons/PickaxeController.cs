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
    private ParticleSystem stoneChips;
    private List<string> resourceType;

    void Awake()
    {
        resourceType = new List<string>();
        resourceType.Add("Stone");
        resourceType.Add("Iron Ore");
        resourceType.Add("Copper Ore");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        axeAnim = GameObject.Find("Male").GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        stoneChips = GetComponentInChildren<ParticleSystem>(true);
    }

    public void SwingPickaxe()
    {
        if(!isMining)
        {
            isMining = true;
            axeAnim.SetTrigger("Mine Swing");
            StartCoroutine(ResetAttackingBool());
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(isMining && canGetResource && resourceType.Contains(other.tag))
        {
            PlayMiningSound();
            canGetResource = false;
            Inventory.AddItem(other.tag , 1);
            print(other.tag + Inventory.GetCount(other.tag));
            stoneChips.Play();
            if (other.TryGetComponent(out ResourceHealth resourceHealth))
            {
                // Will change the passed float to tool type dmg
                resourceHealth.SubtractHealth(1.0f);
            }
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
