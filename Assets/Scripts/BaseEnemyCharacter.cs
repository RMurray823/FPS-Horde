using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyCharacter : BaseCharacter {

    public int damage;
    public int attackSpeed;
    protected float attackTime;

    protected GameObject player;
    protected GameObject target;
    protected GameObject[] targets;
    protected bool isPanicked;
     
    override
    public void Init() {
        base.Init();
        player = GameObject.FindGameObjectWithTag("Player"); //find a player
        targets = GameObject.FindGameObjectsWithTag("Ally"); //make an array of all ally NPC's
    }
    protected void Attack() {
        attackTime = Time.time;
        target.SendMessage("Hit", damage);
    }
    //TODO:Stop enemy attacking itself
    protected void Shot(int damage) {
        health.takeDamage(damage);
        if (health.currentHealth <= health.maxHealth)
        {
            gameObject.tag = "Ally";
            isPanicked = true;
        }
    }
    override
    public void Hit(int damage)
    {
        health.takeDamage(damage);
    }

    protected void CriticalHit(int damage) {
        health.takeDamage(damage * 2);
    }
}
