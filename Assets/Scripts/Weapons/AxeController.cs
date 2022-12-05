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
    private List<string> resourceType;
    private Dictionary<string, int> hatchetPower;
    private string currentTool;
    private int hatchetStrength;

    void Awake()
    {
        hatchetPower = new Dictionary<string, int>();
        hatchetPower.Add("Wood", 1);
        hatchetPower.Add("Stone", 2);
        hatchetPower.Add("Copper", 3);
        hatchetPower.Add("Iron", 4);
    }

    void Start()
    {
        currentTool = Inventory.GetTool("axe");
    }

    public void SwingAxe()
    {
        int stamCost = ToolData.GetValue("axe", Inventory.GetTool("axe"), "stamina");
        if (!isChopping && PlayerHandler.GetValue("stamina") >= stamCost)
        {
            isChopping = true;
            PlayerHandler.AddValue("stamina", -stamCost);
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
            Inventory.AddItem("Wood" , hatchetStrength);
            print("Wood: " + Inventory.GetCount("Wood"));
            woodChips.Play();
            UpdateToolAbility();

            if (other.TryGetComponent(out ResourceHealth resourceHealth))
            {
                resourceHealth.SubtractHealth(hatchetStrength);
            }
        }
    }

    public void UpdateToolAbility()
    {
        currentTool = Inventory.GetTool("axe");
        hatchetStrength = hatchetPower[currentTool];
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

    void OnEnable()
    {
        isChopping = false;
        currentTool = Inventory.GetTool("axe");
        hatchetStrength = hatchetPower[currentTool];
        hatchetAnim = GameObject.Find("Male").GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        canGetResource = true;
        woodChips = GetComponentInChildren<ParticleSystem>(true);
    }

    // Set to the length of hatchetAnim for use of isChopping
    IEnumerator ResetAttackingBool()
    {
        yield return new WaitForSeconds(1.0f);
        isChopping = false;
        canGetResource = true;
    }
}
