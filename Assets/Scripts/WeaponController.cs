using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Animator swordAnim;
    private Animator axeAnim;
    private GameObject axe;
    private GameObject sword;
    private AudioSource source;

    private bool firstAttack = true;
    private bool secondAttack = false;
    private bool swordIsActive;
    private bool axeIsActive;
    public bool isAttacking = false;
    public bool isChopping = false;

    public float firstAttackCooldown = 2.0f;
    public float secondAttackCooldown = 0.6f;
    
    // Start is called before the first frame update
    void Start()
    {
        sword = gameObject.transform.GetChild(0).gameObject;
        axe = gameObject.transform.GetChild(1).gameObject;
        swordAnim = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        axeAnim = gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        swordIsActive = sword.activeInHierarchy;
        axeIsActive = axe.activeInHierarchy;
    }

    // Update is called once per frame
    void Update()
    {
        // if(!sword.activeInHierarchy)
        // {
        //     swordIsActive = false;
        // }
    
        if(Input.GetMouseButtonDown(0))
        {
            // Do first attack if first cooldown is over.
            if(firstAttack && swordIsActive)
            {
                StartCoroutine(ResetSecondAttack());
                StartCoroutine(ResetAttackingBool());
                SwordAttack();
                secondAttack = false;
                swordAnim.SetBool("Second Attack", secondAttack);
            }
            
            // Do second attack if first cooldown is over
            else if(secondAttack && swordIsActive)
            {
                StartCoroutine(ResetAttackingBool());
                SwordAttack2();
            }

            if(axeIsActive)
            {
                isChopping = true;
                axeAnim.SetTrigger("Swing");
                StartCoroutine(ResetAttackingBool());
            }
        }
    }

    public void SwordAttack()
    {
        // Set swordAnimator values and start cooldown.
        firstAttack = false;
        swordAnim.SetBool("First Attack", firstAttack);
        source.PlayOneShot(source.clip);
        isAttacking = true;
        swordAnim.SetTrigger("Attack");
        StartCoroutine(ResetCoolDown());
    }

    public void SwordAttack2()
    {
        // Set swordAnimator values.
        secondAttack = false;
        swordAnim.SetBool("Second Attack", secondAttack);
        source.PlayOneShot(source.clip);
        isAttacking = true;
        swordAnim.SetTrigger("Attack2");
    }

    IEnumerator ResetCoolDown()
    {
        // After firstAttackCooldown seconds, can do first attack again.
        yield return new WaitForSeconds(firstAttackCooldown);
        firstAttack = true;
        swordAnim.SetBool("First Attack", firstAttack);
        secondAttack = false;
        swordAnim.SetBool("Second Attack", secondAttack);
    }

    IEnumerator ResetSecondAttack()
    {
        yield return new WaitForSeconds(secondAttackCooldown);

        // After secondAttackCooldown seconds, can do second attack if first attacked.
        if(!firstAttack)
        {
            secondAttack = true;
            swordAnim.SetBool("Second Attack", secondAttack);
        }
    }

    // Set to the length of swordAnimations for use of isAttacking
    IEnumerator ResetAttackingBool ()
    {
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
        isChopping = false;
    }
}
