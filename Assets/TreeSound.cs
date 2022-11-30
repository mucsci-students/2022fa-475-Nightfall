using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] treeDeath;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayTreeDeathSound()
    {
        int n = Random.Range(1, treeDeath.Length);
        audioSource.clip = treeDeath[n];

        audioSource.PlayOneShot(audioSource.clip);
        treeDeath[n] = treeDeath[0];
        treeDeath[0] = audioSource.clip;
    }

    
}
