using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyScript : MonoBehaviour
{
    private NavMeshAgent nav;
    private GameObject player;
    private GameObject target;
    private GameObject[] enemies;
    private Health health;
    private Animator anim;
    private float range = 50f;

	// Use this for initialization
	void Start ()
    {
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        player = GameObject.FindGameObjectWithTag("Player"); //find a player.
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //movement management
        if(Vector3.Distance(transform.position, player.transform.position) <= nav.stoppingDistance * 2)
            nav.SetDestination(player.transform.position);
        anim.SetFloat("Speed", nav.velocity.magnitude);
        //combat management
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        target = GetClosestEnemy(enemies);
        if (Vector3.Distance(transform.position, target.transform.position) < range)
        {
            anim.SetBool("Aiming", true);
        }

    }

    void Hit(int damage)
    {
        if (health.takeDamage(damage) <= 0)
            Debug.Log("Dead.");
    }

    void Shoot(GameObject target)
    {
        anim.SetTrigger("Attacl");
    }

    private GameObject GetClosestEnemy(GameObject[] enemies)
    {
        Vector3 position = transform.position;
        GameObject closest = null;
        float distance = Mathf.Infinity;
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
