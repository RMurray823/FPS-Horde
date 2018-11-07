using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyScript : MonoBehaviour
{
    private NavMeshAgent nav;

    //objects on the map for the NPC to interact with.
    private GameObject player;
    private GameObject target;
    private GameObject[] enemies;

    private Health health;
    private Animator anim;

    //information on NPC inventory
    private GameObject heldGun;
    private PlayerInventory playerInventory;
    private GunController gunController;

    //TODO: Get GunController working then remove this data.
    private float shotTime;

    public int damage = 10;
    public float fireRate = 1f;
    public float range = 50f;
    public float accuracy = .8f;

    // Use this for initialization
    void Start ()
    {
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player"); //find a player.
        heldGun = playerInventory.getHeldGun();
        gunController = heldGun.GetComponent<GunController>();
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
                    gunController.fireBullet();
                }
            }
        }
        anim.SetFloat("Speed", nav.velocity.magnitude);
    }

    void Hit (int damage)
    {
        if (health.takeDamage(damage) <= 0)
        {
            anim.SetTrigger("Dead");
        }
    }

    void Shoot (GameObject target)
    {
        anim.SetTrigger("Attack");
        shotTime = Time.time;

        float randomOffset = UnityEngine.Random.Range(-(1 - accuracy), 1 - accuracy);

        Vector3 dir = transform.forward;

        dir.x += randomOffset * 0.3f;
        dir.y += randomOffset * 1.6f;
        dir.z += randomOffset * 0.3f;

        Vector3 pos = transform.position;
        RaycastHit result;

        if (Physics.Raycast(pos, dir, out result))
        {
            if (result.collider.tag == "WeakPoint")
                result.rigidbody.SendMessage("CriticalHit", damage);

            else if (result.collider.tag == "Enemy")
                result.collider.SendMessage("Shot", damage);
            Debug.Log(result);
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
