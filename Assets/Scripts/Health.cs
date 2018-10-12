using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour{

    public int health { get; set; }
    public int maxHealth { get; set; }

    private void Start() {
        health = 100;
        maxHealth = 100;
    }

    //Apply the amount of damage defined and return the new health value
    public int takeDamage(int damage) {
        health -= damage;

        //Check for negative damage applied
        if (health > maxHealth)
            health = maxHealth;
        return health;
    }

    //Add the amount of health to restore and return the result
    public int heal(int heal) {
        health++;
        
        if (health > maxHealth)
            health = maxHealth;

        return health;
    }
}
