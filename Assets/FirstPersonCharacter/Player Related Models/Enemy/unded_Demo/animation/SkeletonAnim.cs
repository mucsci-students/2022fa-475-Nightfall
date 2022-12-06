using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnim : MonoBehaviour
{
    public AudioClip[] sounds;

    private AudioSource audio;
    private Animator animation;
    private bool isAlive = true;
    private bool canPlay = true;

    // Start is called before the first frame update
    void Start()
    {
        animation = gameObject.GetComponent<Animator>();
        audio = gameObject.GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update()
    {
        /*
            Ghoul and skeleton use different methods since the Ghoul uses a
            Legacy animator. "animation.Play()" will play both Idle and Walk until
            they are interrupted by another animation. Attack will only play once.
        */
    
        if (!isAlive)
        {
            gameObject.transform.Translate(0, -.005f, 0);
            gameObject.transform.Rotate(.05f, 0, 0);
        }
    }

    public void Walk()
    {
        animation.Play("Walk");
        //audio.clip = sounds[0];
        //audio.loop = true;
        //audio.Play();
    }

    public void Idle()
    {
        animation.Play("Idle");
    }

    public void Kill()
    {
        isAlive = false;
        animation.enabled = false;
        audio.loop = false;
        audio.clip = sounds[1];
        audio.Play();
    }

    public void Attack()
    {
        animation.Play("Attack");
        if (!audio.clip != sounds[3])
        {
            audio.clip = sounds[3];
        }
        if (canPlay)
        {
            canPlay = false;
            audio.Play();
            StartCoroutine(AudioCooldown());
        }
    }

    public void Hit()
    {
        audio.loop = false;
        audio.clip = sounds[2];
        audio.Play();
    }

    IEnumerator AudioCooldown()
    {
        yield return new WaitForSeconds(4);
        canPlay = true;
    }
}
