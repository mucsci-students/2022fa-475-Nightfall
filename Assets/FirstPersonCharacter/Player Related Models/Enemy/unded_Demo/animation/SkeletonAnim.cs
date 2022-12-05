using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnim : MonoBehaviour
{
    private Animator animation;
    // Start is called before the first frame update
    void Start()
    {
        animation = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
            Ghoul and skeleton use different methods since the Ghoul uses a
            Legacy animator. "animation.Play()" will play both Idle and Walk until
            they are interrupted by another animation. Attack will only play once.
        */
       if (Input.GetKey(KeyCode.E))
        {
           animation.Play("Walk");
        }
        if(Input.GetKey(KeyCode.R))
        {
            animation.Play("Idle");
        }
        if (Input.GetButtonDown ("Fire1"))
        {
            animation.Play("Attack");
        }
    }
}
