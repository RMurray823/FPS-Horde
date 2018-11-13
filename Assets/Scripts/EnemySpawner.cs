using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    private Transform player;
    private Health health;
    public GameObject[] enemyTypes;
    public float spawnTime = 3f;
    public float spawnRadius;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<Health>();
        InvokeRepeating("Spawn", spawnTime, spawnTime); //causes this script to run once for each interval spawnTime.
    }

    void Spawn()
    {
        int temp = Random.Range(0, enemyTypes.Length);

        if (Vector3.Distance(this.transform.position, player.transform.position) < spawnRadius)
        {
            Instantiate(enemyTypes[temp], transform.position, transform.rotation);
        }
    }

    void Shot(int damage)
    {
        if (health.takeDamage(damage) <= 0)
            Destroy(gameObject);
    }
}