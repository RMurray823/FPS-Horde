using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private NavMeshAgent _nav;
    private Transform _player;
	// Use this for initialization
	void Start ()
    {
        _nav = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {
        _nav.SetDestination(_player.position);
	}

    void Shot()
    {
        Destroy (gameObject);
    }
}
