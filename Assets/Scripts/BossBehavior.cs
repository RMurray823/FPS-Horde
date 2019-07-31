﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBehavior : BaseEnemyCharacter
{
    private NavMeshAgent nav;
    private Animator anim;
    private bool panicked;
    private string bossState;


    public GameObject[] loot;

    public int minSpeed = 3;
    public int maxSpeed = 5;
    public float threatRadius = 10f;

    public GameObject position1;
    public GameObject position2;
    public GameObject position3;
    public GameObject BossHealthPack1;
    private bool flag1;
    private bool flag2;
    private bool flag3;

    // Use this for initialization
    void Start()
    {
        base.Init();
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        anim = GetComponent<Animator>();

        panicked = false;
        //nav.speed = Random.Range(minSpeed, maxSpeed);
        InvokeRepeating("TargetClosestEnemy", 0, 0.25f);
        target = player;
        bossState = "Patrolling";
        flag1 = true;
        flag2 = false;
        flag3 = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (health.currentHealth <= 0)
        {
            anim.SetTrigger("isDead");
        }
        else if (health.currentHealth <= 25)
        {
            //Rage Mode
        }
        else if (health.currentHealth <= 50)
        {
            //Seek health, then chase player/attack player
            bossState = "SeekHealth";
        }
        else if (health.currentHealth <= 75)
        {
            //Spawn enemies //Only once? Once per minute?
        }
        //anim.SetFloat("Speed", nav.velocity.magnitude);

        //boss state: 1 = Patrolling, 2 = Chasing, 3 = Attacking
        switch (bossState)
        {
            case "Patrolling":
                //
                // Debug.Log("Changing to Patrol mode.");

                Patrolling();
                break;
            case "Chasing":
                //
                //Debug.Log("Changing to Chase mode.");
                Chasing();
                break;
            case "Attacking":
                //
                //Debug.Log("Changing to Attack mode.");
                Attacking();
                break;
            case "SeekHealth":
                //Boss will seek out a health pack
                SeekHealth();
                break;
            default:
                Debug.Log("No boss state selected! Debug as to why...");
                break;
        }
    }

    private void Patrolling()
    {
        anim.SetTrigger("Attack_01");
        /*if (target == null) //Boss patrols if the player isn't in range
        {
            //anim.SetFloat("Speed", nav.velocity.magnitude);

            if (flag1 == true) //Moving towards position1.
            {
                if (Vector3.Distance(transform.position, position1.transform.position) > 4f) //0.001 //if not close, move towards position1
                {
                    nav.SetDestination(position1.transform.position);
                }
                else
                {
                    flag1 = false;
                    flag2 = true;
                }
            }
            else if (flag2 == true)
            {
                if (Vector3.Distance(transform.position, position2.transform.position) > 3.5f) //0.001 //if not close, move towards position1
                {
                    nav.SetDestination(position2.transform.position);
                }
                else
                {
                    flag2 = false;
                    flag3 = true;
                }
            }
            else if (flag3 == true)
            {
                if (Vector3.Distance(transform.position, position3.transform.position) > 3.5f) //0.001 //if not close, move towards position1
                {
                    nav.SetDestination(position3.transform.position);
                }
                else
                {
                    flag3 = false;
                    flag1 = true;
                }
            }
        }
        else //Detects player, sets bossState to chase
        {
            bossState = "Chasing";
            Chasing();
        }*/
    }

    private void Chasing()
    {
        //ADD a collision check here, if it happens have boss dodge somewhere...
        threatRadius = 100f; //So the boss can always tell where the player is located.



        if (Vector3.Distance(nav.transform.position, target.transform.position) > nav.stoppingDistance)
            nav.SetDestination(target.transform.position); //move to target's position.
        else
        {
            bossState = "Attacking"; //Switches boss state
            Attacking(); 
        }
        //set rotation to face target.
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
        targetRotation.y = 180;
    }

    private void Attacking()
    {
        if (Time.time >= attackTime + attackSpeed)
        {
            Debug.Log("Attacking!!!");
            anim.SetTrigger("attack_01"); //This causes damage to the player
        }
        else //Player moves out of attack range, so sent boss state back to chase.
        {
            Debug.Log("Chasing!!!");
            bossState = "Chasing";
        }

        //set rotation to face target.
        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
        targetRotation.y = 180;
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
    }

    private void SeekHealth()
    {
        //Stay in this state until a health pack is picked up, or health < 50
        //nav.SetDestination(BossHealthPack1.transform.position); //Points the boss as the object it is targeting.
        //transform.position = Vector3.MoveTowards(transform.position, BossHealthPack1.transform.position, (.1f)); //Just moves the object, does not point at it.
        nav.SetDestination(BossHealthPack1.transform.position);
        //add some logic to switch back to chase/attack mode after picking up health.
    }

    private void Dodge()
    {
        //Debug.Log("Dodging!!!");
        transform.position += Vector3.right * .10f;
    }

    override
    protected void Shot(ShotInformation info)
    {

        if (info.tag == "WeakPoint")
        {
            health.takeDamage(info.damage * 2);
        }
        else
        {
            health.takeDamage(info.damage);
        }

        //if currentHealth is below panic threshold.
        if (health.currentHealth <= health.maxHealth / 5)
        {
            if (Random.Range(1, 10) == 1)
            {
                panicked = true;
                target = null;
            }
            InvokeRepeating("Decay", 0f, 0.5f);
        }
    }

    //Used to kill a panicking enemy.
    private void Decay()
    {
        health.takeDamage(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HealthPack")
        {
            HealthPack pack = other.GetComponent<HealthPack>();
            health.heal(pack.getHealAmount());
        }

        if (other.tag == "ArmorPack")
        {
            HealthPack pack = other.GetComponent<HealthPack>();
            health.healArmor(pack.getArmorAmount());
        }
    }

    private void Destroy()
    {
        dropLoot();
        Destroy(gameObject);
    }

    private void dropLoot()
    {
        int i = Random.Range(1, 50);
        Vector3 dropPosition = transform.position;
        dropPosition.y = transform.position.y + 0.5f;
        Quaternion dropRotation = transform.rotation;

        switch (i)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                Instantiate(loot[0], dropPosition, dropRotation);
                break;
            case 5:
                Instantiate(loot[0], dropPosition, dropRotation);
                dropLoot();
                break;
            case 6:
            case 7:
                Instantiate(loot[1], dropPosition, dropRotation);
                break;
            case 8:
                Instantiate(loot[1], dropPosition, dropRotation);
                dropLoot();
                break;
            case 9:
                Instantiate(loot[2], dropPosition, dropRotation);
                break;
            case 10:
                Instantiate(loot[2], dropPosition, dropRotation);
                dropLoot();
                break;
            default:
                break;
        }
    }

    private void TargetClosestEnemy()
    {
        target = null;
        //get a list of all colliders in radius
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, threatRadius);
        Vector3 position = transform.position; //position of invoking obj.
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (Collider col in objectsInRange)
        {
            if (!panicked) //if not panicked, attack player or allies.
            {
                if (col.tag == "Player" || col.tag == "Ally")
                {
                    Vector3 diff = col.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = col.gameObject;
                        distance = curDistance;
                    }
                }
            }
            else //else if panicked, target anything.
            {
                if (col.tag == "Enemy" || col.tag == "Player" || col.tag == "Ally")
                {
                    Vector3 diff = col.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance && curDistance != 0)
                    {
                        closest = col.gameObject;
                        distance = curDistance;
                    }
                }
            }
        }
        target = closest;
    }
}
