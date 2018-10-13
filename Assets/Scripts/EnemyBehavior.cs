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
        //check to update the animation method every half second.
        InvokeRepeating("Animation", 0.5f, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
        nav.SetDestination(player.position); //move to player's position.

	}

    void Animation()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) > nav.stoppingDistance)
            anim.SetBool("isMoving", true);
        else
            anim.SetTrigger("attack");
        player.BroadcastMessage("hit", damage);
            anim.SetBool("isMoving", false);
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
    void Shot()
    {
        if(health.takeDamage(50) <= 0)
            Destroy (gameObject);
    }
}
