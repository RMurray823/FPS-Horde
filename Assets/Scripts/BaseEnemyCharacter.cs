﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyCharacter : BaseCharacter {

    public int damage;
    public int attackSpeed;
    protected float attackTime;

    protected GameObject player;
    protected GameObject target;
    protected GameObject[] allies;
     
    override
    public void Init() {
        base.Init();
        player = GameObject.FindGameObjectWithTag("Player"); //find a player
        allies = GameObject.FindGameObjectsWithTag("Ally"); //make an array of all ally NPC's
    }
    protected void Attack() {
        attackTime = Time.time;
        target.SendMessage("Hit", damage);
    }

    protected void Shot(int damage) {
        health.takeDamage(damage);
    }

    protected void CriticalHit(int damage) {
        health.takeDamage(damage * 2);
    }
}