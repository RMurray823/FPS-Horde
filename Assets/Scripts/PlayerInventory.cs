using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    private const int gunListSize = 2;
    public GameObject[] guns;
    private int currentWeapon = 0;

    private void Start() {
        guns[0].SetActive(true);
        for(int i = 1; i < guns.Length; i++) {
            guns[i].SetActive(false);
        }
    }

    public GameObject getHeldGun() {
        return guns[currentWeapon];
    }

    public GameObject swapGun() {
        currentWeapon = (currentWeapon + 1) % gunListSize;
        if (guns[currentWeapon] == null) {
            currentWeapon = 0;
        }

        return guns[currentWeapon];
    }
}
