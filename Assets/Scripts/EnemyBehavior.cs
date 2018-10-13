using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private NavMeshAgent _nav;
    private Transform _player;
    private Health health;
    private Animator _anim;
	// Use this for initialization
	void Start ()
    {
        _nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        _player = GameObject.FindGameObjectWithTag("Player").transform; //find a player.
        health = GetComponent<Health>();
        _anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        _nav.SetDestination(_player.position); //move to player's position.
        if (_anim.velocity.magnitude != 0)
            _anim.SetBool("isMoving", true);
        else
            _anim.SetBool("isMoving", false);
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
