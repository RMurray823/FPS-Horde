using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    public int healAmount;
    public int armorAmount;

    public int getHealAmount() {
        return healAmount;
    }

    public int getArmorAmount() {
        return armorAmount;
    }
    private void OnTriggerEnter(Collider collision) {
        Debug.Log("Colliding");
        if (collision.tag == "Player")
            Destroy(gameObject);
    }
}
