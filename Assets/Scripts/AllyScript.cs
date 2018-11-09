using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyScript : BaseAllyCharacter
{
    private NavMeshAgent nav;

    //objects on the map for the NPC to interact with.
    private GameObject player;
    private GameObject target;
    private GameObject[] enemies;

    private Animator anim;

    public int damage = 10;
    public float fireRate = 1f;
    public float range = 50f;
    public float accuracy = .8f;

    // Use this for initialization
    void Start ()
    {
        base.Init();
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player"); //find a player.

    }
	
	// Update is called once per frame
	void Update ()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        target = GetClosestEnemy(enemies);
        //movement management
        if (Vector3.Distance(transform.position, player.transform.position) >= nav.stoppingDistance)
        {
            anim.SetBool("Aiming", false);
            nav.SetDestination(player.transform.position);
        }
        //check for target
        else if (target != null)
        {
            //if a target exists and is within range, shoot at it
            if (Vector3.Distance(transform.position, target.transform.position) < range)
            {
                anim.SetBool("Aiming", true);
                var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f);
                gunController.fireBullet();
            }
        }
        anim.SetFloat("Speed", nav.velocity.magnitude);
    }

    override
    public void Hit (int damage)
    {
        if (health.takeDamage(damage) <= 0)
        {
            anim.SetTrigger("Dead");
        }
    }

    private void Shoot()
    {
        Vector3 dir = transform.forward;
        Vector3 pos = transform.position;
        RaycastHit results;

        if (Physics.Raycast(pos, dir, out results))
        {
            if (results.collider.tag == "WeakPoint")
                results.rigidbody.SendMessage("CriticalHit", damage);

            else if (results.collider.tag == "Enemy")
                results.collider.SendMessage("Shot", damage);


        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private GameObject GetClosestEnemy (GameObject[] enemies)
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
