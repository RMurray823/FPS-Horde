using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private Transform player;
    public GameObject enemy;
    public float spawnTime = 3f;
    public float spawnRadius;
    public Transform[] spawnPoints; //used to hold multiple locations for enemies to spawn from.

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("Spawn", spawnTime, spawnTime); //causes this script to run once for each interval spawnTime.
	}

    void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        if (Vector3.Distance(this.transform.position, player.transform.position) < spawnRadius)
            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
	
	/* Update is called once per frame (unneeded in this implementation)
	void Update ()
    {
		
	}*/
}
