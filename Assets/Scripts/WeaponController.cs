using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Animator anim;
    private GameObject sword;
    private AudioSource source;

    private bool firstAttack = true;
    private bool secondAttack = false;
    public bool isAttacking = false;

    public float firstAttackCooldown = 2.0f;
    public float secondAttackCooldown = 0.6f;
    // Start is called before the first frame update

    void Start()
    {
        sword = gameObject.transform.GetChild(0).gameObject;
        anim = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        source = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Do first attack if first cooldown is over.
            if(firstAttack)
            {
                StartCoroutine(ResetSecondAttack());
                StartCoroutine(ResetAttackingBool());
                SwordAttack();
                secondAttack = false;
                anim.SetBool("Second Attack", secondAttack);
            }
            
            // Do second attack if first cooldown is over
            else if(secondAttack)
            {
                StartCoroutine(ResetAttackingBool());
                SwordAttack2();
            }
        }
    }

    public void SwordAttack()
    {
        // Set animator values and start cooldown.
        firstAttack = false;
        anim.SetBool("First Attack", firstAttack);
        source.PlayOneShot(source.clip);
        isAttacking = true;
        anim.SetTrigger("Attack");
        StartCoroutine(ResetCoolDown());
    }

    public void SwordAttack2()
    {
        // Set animator values.
        secondAttack = false;
        anim.SetBool("Second Attack", secondAttack);
        source.PlayOneShot(source.clip);
        isAttacking = true;
        anim.SetTrigger("Attack2");
    }

    IEnumerator ResetCoolDown()
    {
        // After firstAttackCooldown seconds, can do first attack again.
        yield return new WaitForSeconds(firstAttackCooldown);
        firstAttack = true;
        anim.SetBool("First Attack", firstAttack);
        secondAttack = false;
        anim.SetBool("Second Attack", secondAttack);
    }

    IEnumerator ResetSecondAttack()
    {
        yield return new WaitForSeconds(secondAttackCooldown);

        // After secondAttackCooldown seconds, can do second attack if first attacked.
        if(!firstAttack)
        {
            secondAttack = true;
            anim.SetBool("Second Attack", secondAttack);
        }
    }

    // Set to the length of animations for use of isAttacking
    IEnumerator ResetAttackingBool ()
    {
        yield return new WaitForSeconds(1.0f);
        isAttacking = false;
    }
}
