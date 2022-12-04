using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHealth : MonoBehaviour
{
    [SerializeField]
    private float lowerHealthBound;

    [SerializeField]
    private float upperHealthBound;

    [SerializeField]
    private float maxHealth;
    private float currentHealth;

    private GameObject resourceParent;
    private TreeSound treeSound;
    private Animator anim;
    private MeshCollider mesh;
    private GameObject player;
    private Vector3 location;
    private List<string> miningType;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Random.Range(lowerHealthBound, upperHealthBound);
        currentHealth = maxHealth;
        treeSound = gameObject.GetComponentInChildren<TreeSound>();
        resourceParent = gameObject.transform.parent.gameObject;
        location = resourceParent.transform.position;
        
        mesh = GetComponent<MeshCollider>();
        anim = GetComponent<Animator>();

        player = GameObject.Find("Player");
       
        miningType = new List<string>();
        miningType.Add("Copper Ore");
        miningType.Add("Iron Ore");
        miningType.Add("Stone");
        
    }

    public void SubtractHealth(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            if(gameObject.tag == "Wood")
            {
                PlayTreeAnimation();
            }
            else if (miningType.Contains(gameObject.tag))
            {
                PlayStoneAnimation();
            }

            StartCoroutine(DesrtoyResource());
        }
    }

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        anim.Rebind();
        anim.Update(0f);
        currentHealth = maxHealth;
    }

    private void PlayStoneAnimation()
    {
        anim.SetTrigger("Destroy");
    }

    private void PlayTreeAnimation()
    {

        // Direction the player is facing.
        float playerX = Mathf.Floor(player.transform.position.x);
        float playerZ = Mathf.Floor(player.transform.position.z);

        // Player is to the right of the resource.
        if(playerX >= location.x)
        {
            if(playerZ >= location.z)
            {
                // Player is to the top right of the resource.
                anim.SetTrigger("SE");
            }
            else
            {
                // Player is to the top left of the resource.
                anim.SetTrigger("SW");
            }
        }

        // Player is to the left of the resource.
        else if (playerX <= location.x)
        {
            // Player is below and to the left of the resource. 
            if(playerZ <= location.z)
            {
                anim.SetTrigger("NW");
            }
            else
            {
                // Player is to the left and above the resource.
                anim.SetTrigger("NE");
            }
        }
        if(treeSound != null)
        {
            treeSound.PlayTreeDeathSound();
        }
        else
        {
            print("Error, no sound on this tree.");
            return;
        }
    }

    IEnumerator DesrtoyResource()
    {
        yield return new WaitForSeconds(5.0f);
        resourceParent.SetActive(false);
    }
}
