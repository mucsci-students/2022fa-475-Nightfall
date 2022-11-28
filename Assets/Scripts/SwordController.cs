using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private PlayerHandler plr;
    private Animator swordAnim;
    private AudioSource source;

    private bool firstAttack = true;
    private bool secondAttack = false;
    private bool isAttacking = false;
    private bool canDealDamage;

    public float firstAttackCooldown = 2.0f;
    public float secondAttackCooldown = 0.6f;
    
    // Start is called before the first frame update
    void Start()
    {
        //plr = GameManager.GetPlr();
        swordAnim = GameObject.Find("Male").GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
        canDealDamage = true;
    }

    public void SwingSword()
    {
        // Do first attack if first cooldown is over.
        if(firstAttack)
        {
            StartCoroutine(ResetSecondAttack());
            StartCoroutine(ResetAttackingBool());
            SwordAttack();
            // secondAttack = false;
            // swordAnim.SetBool("Second Attack", secondAttack);
        }
                
        // Do second attack if first cooldown is over
        else if(secondAttack)
        {
            StartCoroutine(ResetAttackingBool());
            SwordAttack2();
        }
    }

    void OnEnable()
    {
        // Scuffed fix for now. This means, swapping back here immediately restarts CD.
        firstAttack = true;
    }

    private void OnTriggerEnter(Collider other) 
    {
        // If swinging at enemy with a sword
        if(other.tag == "Enemy" && isAttacking && canDealDamage)
        {
            canDealDamage = false;
            print(other.name);
        }
    }

    private void SwordAttack()
    {
        // Set swordAnimator values and start cooldown.
        firstAttack = false; 
        // plr.SwingTool("swords");
        swordAnim.SetBool("First Attack", firstAttack);
        
        source.PlayOneShot(source.clip);
        
        canDealDamage = true;
        isAttacking = true;
        swordAnim.SetTrigger("Attack");
        StartCoroutine(ResetCoolDown());
    }

    private void SwordAttack2()
    {
        // Set swordAnimator values.
        secondAttack = false;
        // GameManager.SwingTool("swords");
        swordAnim.SetBool("Second Attack", secondAttack);
        source.PlayOneShot(source.clip);
        canDealDamage = true;
        isAttacking = true;
        swordAnim.SetTrigger("2ndAttack");
        StartCoroutine(ResetAnim());
    }

    IEnumerator ResetCoolDown()
    {
        // After firstAttackCooldown seconds, can do first attack again.
        yield return new WaitForSeconds(firstAttackCooldown);
        firstAttack = true;
        swordAnim.SetBool("First Attack", firstAttack);
        swordAnim.Rebind();
        swordAnim.Update(0f);
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
    }

    IEnumerator ResetAnim()
    {
        yield return new WaitForSeconds(1.0f);
        swordAnim.Rebind();
        swordAnim.Update(0f);
    }
}
