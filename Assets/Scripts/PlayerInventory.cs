using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    private const int gunListSize = 2;
    public GameObject[] guns;
    private int currentWeapon = 0;

	void Start () {
        
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
