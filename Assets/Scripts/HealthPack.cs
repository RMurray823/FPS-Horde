using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    public int healAmount;
    public int armorAmount;
    public int ammoAmount;
    public GameObject soundPlayer;

    public int getHealAmount() {
        return healAmount;
    }

    public int getArmorAmount() {
        return armorAmount;
    }

    public int getAmmoAmount() {
        return ammoAmount;
    }
    private void OnTriggerEnter(Collider collision) {
  
        if (collision.tag == "Player" || collision.tag == "Enemy" )
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            Instantiate(soundPlayer, null);
            Destroy(gameObject);
        }
        
    }
}
