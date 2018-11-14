using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory: MonoBehaviour {
    public GameObject[] guns;
    private int currentWeapon = 0;

    private void Start() {
        guns[0].SetActive(true);
        guns[0].GetComponent<GunController>().held = true;
        for (int i = 1; i < guns.Length; i++) {
            if (guns[i] != null) {
                guns[i].SetActive(false);
                guns[i].GetComponent<GunController>().held = true;
            }
        }
    }

    public void PickUpGun(GameObject gun) {

        //Already held by another NPC/player so we early quit
        if (gun.GetComponent<GunController>().held)
            return;

        gun.transform.parent = Camera.main.transform;
        gun.transform.localRotation = new Quaternion(0, 0, 0, 0);
        gun.transform.localPosition = new Vector3(.27f, -.3f, .4f);
        gun.transform.localScale = new Vector3(1, 1, 1);
        gun.GetComponent<GunController>().held = true;

        bool emptySpot = false;


            for (int i = 0; i < guns.Length; i++) {
                if (guns[i] == null) {
                    gun.SetActive(false);
                    guns[i] = gun;
                    emptySpot = true;
                    break;
                }
            }
            if(!emptySpot) {
                guns[currentWeapon] = gun;
                gun.SetActive(true);
            }
        
    }

    public GameObject getHeldGun() {
        return guns[currentWeapon];
    }

    public GameObject swapGun() {
        currentWeapon = (currentWeapon + 1) % guns.Length;
        if (guns[currentWeapon] == null) {
            currentWeapon = 0;
        }

        return guns[currentWeapon];
    }
}
