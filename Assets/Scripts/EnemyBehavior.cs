using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : BaseEnemyCharacter
{
    private NavMeshAgent nav;
    private Animator anim;
    private bool panicked;

    public int minSpeed = 3;
    public int maxSpeed = 5;
    public float threatRadius = 10f;
    public GameObject loot;

	// Use this for initialization
	void Start ()
    {
        base.Init();
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        anim = GetComponent<Animator>();

        panicked = false;
        nav.speed = Random.Range(minSpeed, maxSpeed);
        InvokeRepeating("TargetClosestEnemy", 0, .25f);
        target = player;
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
            //set rotation to face target.
            var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f);
        }

    }

    override
    protected void Shot(int damage)
    {
        health.takeDamage(damage);
        //if currentHealth is below panic threshold.
        if (health.currentHealth <= health.maxHealth / 5)
        {
            if(Random.Range(0, 5) == 0)
            {
                gameObject.tag = "Ally";
                panicked = true;
                target = null;
            }
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
            else //else if panicked, target enemies.
            {
                if (col.tag == "Enemy")
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
        }
        target = closest;
    }
}