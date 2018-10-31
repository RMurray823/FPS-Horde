using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    public int healAmount;
    public int armorAmount;
    private AudioSource sound;

    public int getHealAmount() {
        return healAmount;
    }

    public int getArmorAmount() {
        return armorAmount;
    }
    private void OnTriggerEnter(Collider collision) {
        Debug.Log("Colliding");
        sound = GetComponent<AudioSource>();    
        if (collision.tag == "Player" || collision.tag == "Enemy")
        {
            sound.Play();
            Destroy(gameObject, 2.0f);
        }
    }
}
