using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public GameObject player;

    public Text uiHealth;
    public Text uiArmor;

    //Remaining ammo not loaded
    public Text uiRemainingAmmo;
    //Currently loaded
    public Text uiClipAmmo;

    private Health health;

    private GameObject heldGun;
    private GunController gunInfo;

    void Start () {
        health = player.GetComponent<Health>();
        heldGun = player.GetComponent<PlayerController>().getHeldGun();

        uiHealth.text = health.currentHealth.ToString();
        uiArmor.text = health.currentArmor.ToString();
    }
	
	
	void Update () {
        heldGun = player.GetComponent<PlayerController>().getHeldGun();
        gunInfo = heldGun.GetComponent<GunController>();

        UpdateUI();
    }

    private void UpdateUI() {
        uiHealth.text = health.currentHealth.ToString();
        uiArmor.text = health.currentArmor.ToString();

        uiClipAmmo.text = gunInfo.getAmmoInClip().ToString();
        uiRemainingAmmo.text = gunInfo.getAmmoNotInClip().ToString();
    }
}
