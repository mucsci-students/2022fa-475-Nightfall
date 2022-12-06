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
    private Dictionary<string, int> pickaxePower;
    private string currentTool;
    private int pickaxeStrength;

    private bool msgDisplaying = false;

    void Awake()
    {
        resourceType = new List<string>();
        resourceType.Add("Stone");
        resourceType.Add("Iron Ore");
        resourceType.Add("Copper Ore");

        pickaxePower = new Dictionary<string, int>();
        pickaxePower.Add("Wood", 1);
        pickaxePower.Add("Stone", 2);
        pickaxePower.Add("Copper", 3);
        pickaxePower.Add("Iron", 4);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        currentTool = Inventory.GetTool("pickaxe");
    }

    public void SwingPickaxe()
    {
        int stamCost = ToolData.GetValue("pickaxe", Inventory.GetTool("pickaxe"), "stamina");
        if (!isMining && PlayerHandler.GetValue("stamina") >= stamCost)
        {
            isMining = true;
            PlayerHandler.AddValue("stamina", -stamCost);
            axeAnim.SetTrigger("Mine Swing");
            StartCoroutine(ResetAttackingBool());
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(isMining && canGetResource && resourceType.Contains(other.tag))
        {
            string otherTag = other.tag;
            print(pickaxeStrength);

            PlayMiningSound();
            canGetResource = false;

            UpdateToolAbility();

            if (other.TryGetComponent(out ResourceHealth resourceHealth))
            {
                // Didn't know case statements had to be constants. So gross 'ifs' instead.
                // Wooden Pick
                if(currentTool == "Wood")
                {
                    if(otherTag == "Stone")
                    {
                        Inventory.AddItem(otherTag, pickaxeStrength);
                        resourceHealth.SubtractHealth(pickaxeStrength);
                    }
                    else
                    {
                        if (!msgDisplaying)
                        {
                            msgDisplaying = true;
                            UIManager.ToggleMsgText("Your pickaxe is too weak for this!");
                            StartCoroutine(FadeMsg());
                        }
                        print("Tool too weak!");
                    }
                }
                
                // Stone Pick
                else if(currentTool == "Stone")
                {
                    
                    if(otherTag == "Stone")
                    {
                        Inventory.AddItem(otherTag, pickaxeStrength);
                        resourceHealth.SubtractHealth(pickaxeStrength);
                    }
                    else if(otherTag == "Copper Ore")
                    {
                        Inventory.AddItem(otherTag, pickaxeStrength);
                        resourceHealth.SubtractHealth(pickaxeStrength);
                    }
                    else
                    {
                        print("Tool too weak!");
                    }
                }

                else if(currentTool == "Copper" || currentTool == "Iron")
                {
                    Inventory.AddItem(otherTag, pickaxeStrength);
                    resourceHealth.SubtractHealth(pickaxeStrength);
                }

                print(other.tag + Inventory.GetCount(other.tag));
                stoneChips.Play();
            }
        }
    }

    IEnumerator FadeMsg()
    {
        yield return new WaitForSeconds(2);

        UIManager.ToggleMsgText("");
        msgDisplaying = false;
    }

    public void UpdateToolAbility()
    {
        currentTool = Inventory.GetTool("pickaxe");
        pickaxeStrength = pickaxePower[currentTool];
    }

    void OnEnable()
    {
        isMining = false;
        currentTool = Inventory.GetTool("pickaxe");
        pickaxeStrength = pickaxePower[currentTool];
        axeAnim = GameObject.Find("Male").GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        stoneChips = GetComponentInChildren<ParticleSystem>(true);
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
