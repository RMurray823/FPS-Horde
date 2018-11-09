using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAllyCharacter : BaseCharacter {

    //Currently held gun info
    protected GameObject heldGun;
    protected GunController gunController;
    protected PlayerInventory playerInventory;

    override
    public void Init() {
        base.Init();
        playerInventory = GetComponent<PlayerInventory>();
        heldGun = playerInventory.getHeldGun();
        gunController = heldGun.GetComponent<GunController>();
    }

    public Health getHealth() {
        return health;
    }

    public GameObject getHeldGun() {
        return heldGun;
    }

    virtual
    public void Hit(int damage) {
        if (health.takeDamage(damage) <= 0) {
            Debug.Log("dead");
        }
    }
}
