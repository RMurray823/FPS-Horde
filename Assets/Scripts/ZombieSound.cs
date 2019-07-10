using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSound : MonoBehaviour
{

    public AudioSource deathAudio;
    private AudioClip deathClip;
    private bool is_dead;
    private int health;

    // Use this for initialization
    void Start()
    {

        is_dead = false;

        var audio = GetComponents<AudioSource>();
        deathAudio = audio[2];
        deathClip = deathAudio.clip;
    }

    // Update is called once per frame
    void Update()
    {
        health = GetComponent<Health>().currentHealth;

        if (health <= 0)
        {
            //play death audio
            if (!is_dead)
            {
                PlayDeath();
                is_dead = true;
            }
        }
    }


    private void PlayDeath()
    {
        deathAudio.PlayOneShot(deathClip);
    }

    private void PlayAttack()
    { }
}
