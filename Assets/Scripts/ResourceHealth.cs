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

    private GameObject resourceParent;
    private TreeSound treeSound;
    private Animator anim;
    private MeshCollider mesh;
    private GameObject player;
    private Vector3 location;

    // Start is called before the first frame update
    void Start()
    {
        resourceParent = gameObject.transform.parent.gameObject;
        maxHealth = Random.Range(lowerHealthBound, upperHealthBound);
        anim = GetComponent<Animator>();
        mesh = GetComponent<MeshCollider>();
        treeSound = gameObject.GetComponentInChildren<TreeSound>();
        player = GameObject.Find("Player");
        location = resourceParent.transform.position;
        print(maxHealth);
    }

    public void SubtractHealth(float dmg)
    {
        maxHealth -= dmg;
        if (maxHealth <= 0.0f)
        {
            if(gameObject.tag == "Wood")
            {
                PlayTreeAnimation();
            }
            else if (gameObject.tag == "Stone")
            {
                PlayStoneAnimation();
            }

            StartCoroutine(DesrtoyResource());
        }
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
        }
    }

    IEnumerator DesrtoyResource()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}