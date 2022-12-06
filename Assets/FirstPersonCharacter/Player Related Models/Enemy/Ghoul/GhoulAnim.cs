using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulAnim : MonoBehaviour
{
    [SerializeField]public AudioClip[] sounds;

    private Animation animation;
    private AudioSource audio;
    // https://docs.unity3d.com/2017.4/Documentation/Manual/AnimationScripting.html

    private bool isAlive = true;
    private bool canPlay = true;
    
    // Start is called before the first frame update
    void Start()
    {
        animation = gameObject.GetComponent<Animation>();
        audio = gameObject.GetComponent<AudioSource>();
       
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
    /*
    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }

        if (isAlive)
        {
            // Only plays walk animation, in this code we're not moving the object.
            animation.CrossFade("Walk");
        }
        else
        {
            animation.CrossFade("Idle");
        }
    }*/

    public void Kill()
    {
        isAlive = false;
        animation.Stop();
        animation.CrossFade("Death");
        audio.clip = sounds[Random.Range(2, 3)];
        audio.Play();
    }

    public void Walk()
    {
        animation.CrossFade("Walk");
    }

    public void Idle()
    {
        animation.CrossFade("Idle");
    }

    public void Attack()
    {
        if (Random.Range(0f,1f) < .5)
        {            
            animation.CrossFade("Attack1");
        }
        else
        {
            animation.CrossFade("Attack2");
        }

        if (canPlay)
        {
            canPlay = false;
            audio.clip = sounds[Random.Range(0, 1)];
            audio.Play();
            StartCoroutine(AudioCooldown());
        }
    }

    public void Hit()
    {
        audio.clip = sounds[4];
        audio.Play();
    }

    IEnumerator AudioCooldown()
    {
        yield return new WaitForSeconds(4);
        canPlay = true;
    }
}
