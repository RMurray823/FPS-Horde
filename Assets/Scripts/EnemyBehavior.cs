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

	// Use this for initialization
	void Start ()
    {

        base.Init();
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        anim = GetComponent<Animator>();

        nav.speed = Random.Range(minSpeed, maxSpeed);
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (health.currentHealth <= 0)
            anim.SetTrigger("isDead");

        allies = GameObject.FindGameObjectsWithTag("Ally");

        target = GetClosestEnemy(allies);
        anim.SetFloat("Speed", nav.velocity.magnitude);
        //control movement amimations.
    
        if (Vector3.Distance(transform.position, target.transform.position) > nav.stoppingDistance)
            nav.SetDestination(target.transform.position); //move to target's position.

        else if (target != null)
        {
            if(Time.time >= attackTime + attackSpeed)
            anim.SetTrigger("attack");
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
        Destroy(gameObject);
    }

    private GameObject GetClosestEnemy(GameObject[] enemies)
    {
        Vector3 position = transform.position; //get invoking obj position.
        GameObject closest = player; //default to player.
        //calculate difference between player and obj pos.
        Vector3 playerDiff = player.transform.position - position;
        float distance = playerDiff.sqrMagnitude;
        foreach (GameObject go in allies)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
