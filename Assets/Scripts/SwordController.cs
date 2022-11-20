using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private Animator swordAnim;
    private Animator cameraAnim;
    private AudioSource source;

    private bool firstAttack = true;
    private bool secondAttack = false;
    private bool isAttacking = false;

    public float firstAttackCooldown = 2.0f;
    public float secondAttackCooldown = 0.6f;
    
    // Start is called before the first frame update
    void Start()
    {
        swordAnim = GetComponent<Animator>();
        cameraAnim = gameObject.transform.parent.parent.GetChild(0).GetComponent<Animator>();
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
                swordAnim.SetBool("Second Attack", secondAttack);
            }
                
            // Do second attack if first cooldown is over
            else if(secondAttack)
            {
                StartCoroutine(ResetAttackingBool());
                SwordAttack2();
            }
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
        if(other.tag == "Enemy" && isAttacking)
        {
            Debug.Log(other.name);
        }
    }

    private void SwordAttack()
    {
        // Set swordAnimator values and start cooldown.
        firstAttack = false;
        swordAnim.SetBool("First Attack", firstAttack);
        cameraAnim.SetTrigger("Swing");
        source.PlayOneShot(source.clip);
        isAttacking = true;
        swordAnim.SetTrigger("Attack");
        StartCoroutine(ResetCoolDown());
    }

    private void SwordAttack2()
    {
        // Set swordAnimator values.
        secondAttack = false;
        swordAnim.SetBool("Second Attack", secondAttack);
        cameraAnim.SetTrigger("Swing2");
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
    }
}
