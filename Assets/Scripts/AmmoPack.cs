﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    private AudioSource sound;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Colliding");
        sound = GetComponent<AudioSource>();    
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<GunController>().addAmmo();
            sound.Play();
            Destroy(gameObject, .5f);
        }
    }
}
