using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private NavMeshAgent nav;
    private Transform player;
    private Health health;
    private Animator anim;
    private int damage = 5;

	// Use this for initialization
	void Start ()
    {
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        player = GameObject.FindGameObjectWithTag("Player").transform; //find a player.
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        nav.SetDestination(player.position); //move to player's position.
        Debug.Log(this.anim.GetCurrentAnimatorClipInfo(0));
        //control movement
        if (Vector3.Distance(transform.position, player.transform.position) > nav.stoppingDistance)
            anim.SetBool("isMoving", true);
        else
        {
            anim.SetTrigger("attack");
            //only hit once per second
            anim.SetBool("isMoving", false);
        }
    }

    private void Attack()
    {
        player.BroadcastMessage("hit", damage);
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
}
