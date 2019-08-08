using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public GameObject player;

    public Text uiHealth;
    public Text uiArmor;
    public Text bossStateUI;

    //Remaining ammo not loaded
    public Text uiRemainingAmmo;
    //Currently loaded
    public Text uiClipAmmo;

    private Health health;
    public string bossState;

    private GameObject heldGun;
    private GunController gunInfo;

    GameObject boss;

    void Start () {
        health = player.GetComponent<Health>();
         boss = GameObject.Find("Boss");
        
        heldGun = player.GetComponent<PlayerController>().GetHeldGun();

        uiHealth.text = health.currentHealth.ToString();
        uiArmor.text = health.currentArmor.ToString();
    }
	
	
	void Update () {
        bossState = boss.GetComponent<BossBehavior>().bossState;
        
        heldGun = player.GetComponent<PlayerController>().GetHeldGun();

        if(heldGun)
            gunInfo = heldGun.GetComponent<GunController>();

        UpdateUI();
    }

    private void UpdateUI() {

        bossStateUI.text = bossState;
        uiHealth.text = health.currentHealth.ToString();
        //uiHealth.text = health.currentHealth.ToString();
        uiArmor.text = health.currentArmor.ToString();

        if(heldGun) {
            uiClipAmmo.text = gunInfo.GetAmmoInClip().ToString();
            uiRemainingAmmo.text = gunInfo.GetAmmoNotInClip().ToString();
        } else {
            uiClipAmmo.text = "0";
            uiRemainingAmmo.text = "0";
        }

    }
}
