using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    private AudioSource sound;

    private void OnTriggerEnter(Collider collision)
    {
        sound = GetComponent<AudioSource>();    
        if (collision.tag == "Player")
        {
            sound.Play();
            Destroy(gameObject, .5f);
        }
    }
}
