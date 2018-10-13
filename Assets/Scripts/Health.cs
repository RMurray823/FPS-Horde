using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour{

    public int currentHealth { get; set; }
    public int maxHealth { get; set; }

    public RectTransform healthBar;
    private void Start() {
        currentHealth = 100;
        maxHealth = 100;
    }

    //Apply the amount of damage defined and return the new health value
    public int takeDamage(int damage) {
        currentHealth -= damage;

        //Check for negative damage applied
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        updateBar();
        return currentHealth;
    }

    //Add the amount of health to restore and return the result
    public int heal(int heal) {
        currentHealth+= heal;
        
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        updateBar();
        return currentHealth;
    }

    private void updateBar() {
        if (healthBar != null) {
            healthBar.sizeDelta = new Vector2((float)currentHealth / (float)maxHealth, healthBar.sizeDelta.y);
        }
    }
}
