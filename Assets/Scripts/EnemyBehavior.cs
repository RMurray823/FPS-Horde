using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : BaseEnemyCharacter
{
    private NavMeshAgent nav;
    private Animator anim;

    public int minSpeed = 3;
    public int maxSpeed = 5;
    public GameObject loot;

	// Use this for initialization
	void Start ()
    {
        base.Init();
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        anim = GetComponent<Animator>();

        isPanicked = false;
        nav.speed = Random.Range(minSpeed, maxSpeed);
        InvokeRepeating("TargetClosestEnemy", 0, .25f);
        target = player;
        targets = GameObject.FindGameObjectsWithTag("Ally");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (health.currentHealth <= 0)
            anim.SetTrigger("isDead");

        anim.SetFloat("Speed", nav.velocity.magnitude);

        if(target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > nav.stoppingDistance)
                nav.SetDestination(target.transform.position); //move to target's position.
            else
            {
                if (Time.time >= attackTime + attackSpeed)
                    anim.SetTrigger("attack");
            }
        }
    }

    override
    protected void Shot(int damage)
    {
        health.takeDamage(damage);
        //if currentHealth is below panic threshold.
        if (health.currentHealth <= health.maxHealth)
        {
            gameObject.tag = "Ally";
            isPanicked = true;
            target = null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "HealthPack") {
            HealthPack pack = other.GetComponent<HealthPack>();
            health.heal(pack.getHealAmount());
        }

        if(other.tag == "ArmorPack") {
            HealthPack pack = other.GetComponent<HealthPack>();
            health.healArmor(pack.getArmorAmount());
        }
    }

    private void Destroy()
    {
        if(Random.Range(0, 10) == 0)
        {
            Instantiate(loot, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

    private void TargetClosestEnemy()
    {
        GameObject closest;
        float distance;
        Vector3 position = transform.position; //get invoking obj position.
        //calculate difference between player and obj pos.
        Vector3 playerDiff = player.transform.position - position;
        if (!isPanicked)
        {
            closest = player;
            distance = playerDiff.sqrMagnitude;
            targets = GameObject.FindGameObjectsWithTag("Ally");
        }
        else
        {
            closest = null;
            distance = Mathf.Infinity;
            targets = GameObject.FindGameObjectsWithTag("Enemy");
        }

        foreach (GameObject go in targets)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        target = closest;
    }
}