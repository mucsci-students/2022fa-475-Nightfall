using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulAnim : MonoBehaviour
{
    private Animation animation;
    // https://docs.unity3d.com/2017.4/Documentation/Manual/AnimationScripting.html
    
    // Start is called before the first frame update
    void Start()
    {
        animation = gameObject.GetComponent<Animation>();

       
        animation.wrapMode = WrapMode.Loop;
        animation["Walk"].layer = 0;
        animation["Run"].layer = 0;
        animation["Idle"].layer = 0;

        animation["Attack1"].wrapMode = WrapMode.Once;
        animation["Attack1"].layer = 1;
        animation["Attack2"].wrapMode = WrapMode.Once;
        animation["Attack2"].layer = 1;
        animation["Death"].wrapMode = WrapMode.Once;
        animation["Death"].layer = 1;
        animation.Stop();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1)
        {
            // Only plays walk animation, in this code we're not moving the object.
            animation.CrossFade("Walk");
        }
        else
        {
            animation.CrossFade("Idle");
        }
        // Shoot
        if (Input.GetButtonDown ("Fire1"))
        {
            animation.CrossFade("Attack1");
        }
    }
}
