using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private NavMeshAgent nav;
    private GameObject player;
    private GameObject target;
    private GameObject[] allies;
    private Health health;
    private Animator anim;

    private float attackTime;

    public int damage = 5;
    public int attackSpeed = 1;
    public int minSpeed = 3;
    public int maxSpeed = 5;

	// Use this for initialization
	void Start ()
    {
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player"); //find a player
        allies = GameObject.FindGameObjectsWithTag("Ally"); //make an array of all ally NPC's
        nav.speed = Random.Range(minSpeed, maxSpeed);
    }
	
	// Update is called once per frame
	void Update ()
    {

        target = GetClosestEnemy(allies);
        anim.SetFloat("Speed", nav.velocity.magnitude);
        //control movement amimations.
        if (Vector3.Distance(transform.position, target.transform.position) > nav.stoppingDistance)
            nav.SetDestination(target.transform.position); //move to target's position.

        else if (Vector3.Distance(transform.position, target.transform.position) <= nav.stoppingDistance)

        {
            if(Time.time >= attackTime + attackSpeed)
            anim.SetTrigger("attack");
        }

    }

    private void Attack()
    {
        attackTime = Time.time;
        target.SendMessage("Hit", damage);
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

    void Shot(int damage)
    {
        if (health.takeDamage(damage) <= 0)
            anim.SetTrigger("isDead");
    }

    void CriticalHit(int damage)
    {
        if (health.takeDamage(damage * 2) <= 0)
            anim.SetTrigger("isDead");
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private GameObject GetClosestEnemy(GameObject[] enemies)
    {
        Vector3 position = transform.position; //get invoking obj position.
        //calculate difference between player and obj pos.
        Vector3 playerDiff = player.transform.position - position;
        GameObject closest = player; //default to player.
        float distance = playerDiff.sqrMagnitude;
        foreach (GameObject go in enemies)
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
