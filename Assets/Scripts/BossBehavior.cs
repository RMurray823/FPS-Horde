﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Boss Scale is 4.921592, 4.921593, 4.921593
//*****************************************************
//Things to add:
// Change his movement speed.
// Better dodge - no
// Not hit player away 
// Boss to stop moving when hitting
// Boss health to work/health bar
// Introduce elements that happen when health is triggered

//
//*****************************************************
public class BossBehavior : BaseEnemyCharacter
{
    private NavMeshAgent nav;
    private Animator anim;
    private bool panicked;
    public string bossState;

    private int counter = 0;
    public int i = 0;
    private float currentTime;

    public GameObject[] loot;

    //public int minSpeed = 3;
    public int maxSpeed = 5;
    public float threatRadius = 10f;

    public GameObject position1; //object one to patrol around
    public GameObject position2; //object two to patrol around
    public GameObject position3; //object three to patrol around
    public GameObject BossHealthPack1;
    private bool canChangeStates = true;
    private bool flag1 = true;
    private bool flag2 = false;
    private bool flag3 = false;
    public bool pickedUpHealth = false;
    //Animation flags
    public bool animWalk = true;
    public bool animRun = true;
    public bool animAttack = true;
    public bool animDie = true;
    public bool animJump = true;
    public string currentHealthState;

    // Use this for initialization
    void Start()
    {
        base.Init();
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component. //Changing Stopping Distance from 0 to 10;
        anim = GetComponent<Animator>();

        panicked = false;
        //nav.speed = Random.Range(minSpeed, maxSpeed);
        nav.speed = maxSpeed;
        InvokeRepeating("TargetClosestEnemy", 0, 0.25f);
        target = player;
        bossState = "Patrolling";
        Pressed_walk();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.currentHealth <= 0)
        {
            Debug.Log("Dying");
            currentHealthState = "Dying";
            bossState = "Death";

            
        }
        else if (health.currentHealth > 0 && health.currentHealth <= 25)
        {
            Debug.Log("Rage");
            currentHealthState = "Rage";
            //Rage Mode
        }
        else if (health.currentHealth > 25 && health.currentHealth <= 50)
        {
            Debug.Log("Seeking health");
            currentHealthState = "Seeking health";
            //Seek health, then chase player/attack player
            bossState = "SeekHealth";
        }
        else if (health.currentHealth > 50 && health.currentHealth <= 75)
        {
            Debug.Log("Health is fine");
            currentHealthState = "fine";
            //Spawn enemies //Only once? Once per minute?
        }
        else if (health.currentArmor > 75)
        {
            Debug.Log("Full Boss Health");
            currentHealthState = "Full";
        }

        //boss state: 1 = Patrolling, 2 = Chasing, 3 = Attacking
        if (canChangeStates)
        {
            switch (bossState)
            {
                case "Patrolling":
                    // Debug.Log("Changing to Patrol mode.");
                    Patrolling();
                    break;
                case "Chasing":
                    //Debug.Log("Changing to Chase mode.");
                    if (animRun)
                    {
                        Pressed_run();
                        animRun = false;
                    }
                    Chasing();
                    break;
                case "Attacking":
                    //Debug.Log("Changing to Attack mode.");
                    if (animAttack)
                    {
                        Pressed_attack_01();
                        animAttack = false;
                    }
                    Attacking();
                    break;
                case "SeekHealth":
                    //Boss will seek out a health pack
                    if (animRun)
                    {
                        Pressed_run();
                        //anim.SetBool("run", true);
                        animRun = false;
                    }
                    //ClearAllBool();
                    SeekHealth();
                    break;
                case "Death":
                    Debug.Log("Dying State activated.");
                    if (animDie)
                    {
                        ClearAllBool();
                        anim.SetBool("die", true);
                        animDie = false;
                    }


                    break;
                default:
                    Debug.Log("No boss state selected! Debug as to why...");
                    break;
            }
        }
        else
        {
            SeekHealth();
        }

    }
    private void ClearAllBool()
    {
        anim.SetBool("defy", false);
        anim.SetBool("idle", false);
        anim.SetBool("dizzy", false);
        anim.SetBool("walk", false);
        anim.SetBool("run", false);
        anim.SetBool("jump", false);
        anim.SetBool("die", false);
        anim.SetBool("jump_left", false);
        anim.SetBool("jump_right", false);
        anim.SetBool("attack_01", false);
        anim.SetBool("attack_03", false);
        anim.SetBool("attack_02", false);
        anim.SetBool("damage", false);
    }
    private void Patrolling()
    {
        if (target == null) //Boss patrols if the player isn't in range
        {
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
                if (Vector3.Distance(transform.position, position2.transform.position) > 4f) //0.001 //if not close, move towards position1
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
                if (Vector3.Distance(transform.position, position3.transform.position) > 4f) //0.001 //if not close, move towards position1
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
            maxSpeed = 15; //This is NOT changing his speed...
            animRun = true;
        }
    }

    private void Chasing()
    {
        //ADD a collision check here, if it happens have boss dodge somewhere...
        threatRadius = 100f; //So the boss can always tell where the player is located.

        if (Vector3.Distance(nav.transform.position, target.transform.position) > (nav.stoppingDistance + 3))
        {
            nav.SetDestination(target.transform.position); //move to target's position.
        }
        else
        {
            bossState = "Attacking"; //Switches boss state
            animRun = true; //to switch the run animation back on next time it switches states.
        }
    }

    private void Attacking()
    {
        //Logic: If boss is still within range, attack again, else move closer.
        if (Vector3.Distance(nav.transform.position, target.transform.position) < (nav.stoppingDistance + 10)) //Will continue attacking until you move out of radius
        {
            //attack, else switch to chase state
            if (Time.time >= attackTime + attackSpeed)
            {
                Attack();
            }
            //Attack();
        }
        else //Move closer
        {
            bossState = "Chasing";
            animAttack = true;
        }
    }

    private void SeekHealth()
    {
        //Stay in this state until a health pack is picked up, or health < 50
        //nav.SetDestination(BossHealthPack1.transform.position); //Points the boss as the object it is targeting.
        //transform.position = Vector3.MoveTowards(transform.position, BossHealthPack1.transform.position, (.1f)); //Just moves the object, does not point at it.

        nav.SetDestination(BossHealthPack1.transform.position);

        //add some logic to switch back to chase mode after picking up health.
        //Distance_ = Vector3.Distance(Obj1.transform.position, Obj2.transform.position);
        

        if (pickedUpHealth)
        {
            

            if (animJump)
            {
                currentTime = Time.time;
                Pressed_jump();
                animJump = false;
                canChangeStates = false;
            }
            else
            {

                //for loop here
                if (Time.time <= currentTime + 1f)
                {
                    
                    canChangeStates = true;
                    bossState = "Chasing";
                    animRun = true;

                }

            }
            pickedUpHealth = false;
            canChangeStates = true;
            bossState = "Chasing";
            animRun = true;
        }
    }

    private void DodgeRight()
    {
        //Debug.Log("Dodging!!!");
        transform.position += Vector3.right * .10f;
    }
    private void DodgeLeft()
    {
        transform.position += Vector3.left * .10f;
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
            //if (Random.Range(1, 10) == 1)
            //{
            //    panicked = true;
            //    target = null;
            //}
            //InvokeRepeating("Decay", 0f, 0.5f);
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
            pickedUpHealth = true;
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
    public void Pressed_damage()
    {
        ClearAllBool();
        anim.SetBool("damage", true);
    }
    public void Pressed_idle()
    {
        ClearAllBool();
        anim.SetBool("idle", true);
    }
    public void Pressed_defy()
    {
        ClearAllBool();
        anim.SetBool("defy", true);
    }
    public void Pressed_dizzy()
    {
        ClearAllBool();
        anim.SetBool("dizzy", true);
    }
    public void Pressed_run()
    {
        ClearAllBool();
        anim.SetBool("run", true);
    }
    public void Pressed_walk()
    {
        ClearAllBool();
        anim.SetBool("walk", true);
    }
    public void Pressed_die()
    {
        ClearAllBool();
        anim.SetBool("die", true);
    }
    public void Pressed_attack_01()
    {
        ClearAllBool();
        anim.SetBool("attack_01", true);
    }
    public void Pressed_attack_02()
    {
        ClearAllBool();
        anim.SetBool("attack_02", true);
    }
    public void Pressed_attack_03()
    {
        ClearAllBool();
        anim.SetBool("attack_03", true);
    }
    public void Pressed_jump()
    {
        ClearAllBool();
        anim.SetBool("jump", true);
    }
}

//********************************************
//Code I don't want to lose yet:

//set rotation to face target.
//var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
//targetRotation.y = 180;

//I also liked this way below
//Vector3 relativePos = target.transform.position - transform.position;
//transform.rotation = Quaternion.LookRotation(relativePos);
//********************************************