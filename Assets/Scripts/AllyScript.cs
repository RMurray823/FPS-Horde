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
    private AudioSource gunShot;

    private float shotTime;

    public int damage = 10;
    public float fireRate = 1f;
    public float range = 50f;

    // Use this for initialization
    void Start ()
    {
        gunShot = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        health = GetComponent<Health>();
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
                if (Time.time >= shotTime + fireRate)
                {
                    Shoot(target);
                    gunShot.Play();
                }
            }
        }
        anim.SetFloat("Speed", nav.velocity.magnitude);
    }

    void Hit (int damage)
    {
        if (health.takeDamage(damage) <= 0)
        {
            Debug.Log("dead");
        }
    }

    void Shoot (GameObject target)
    {
        anim.SetTrigger("Attack");
        shotTime = Time.time;
        Vector3 dir = transform.forward;
        Vector3 pos = transform.position;
        RaycastHit result;

        if (Physics.Raycast(pos, dir, out result))
        {
            if (result.collider.tag == "WeakPoint")
                result.rigidbody.SendMessage("CriticalHit", damage);

            else if (result.collider.tag == "Enemy")
                result.collider.SendMessage("Shot", damage);
        }
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
