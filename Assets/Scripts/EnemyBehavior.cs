using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private NavMeshAgent _nav;
    private Transform _player;
    private Health health;
	// Use this for initialization
	void Start ()
    {
        _nav = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<Health>();
    }
	
	// Update is called once per frame
	void Update () {
        _nav.SetDestination(_player.position);
	}

    void Shot()
    {
        if(health.takeDamage(50) <= 0)
            Destroy (gameObject);
    }
}
