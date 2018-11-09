using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalEvent : MonoBehaviour
{
    public GameObject[] enemyTypes;
    public Transform spawnCenter;
    public float spawnRate;

    private Transform player;
    private float spawnRadius;

    // Use this for initialization
    void Start()
    {
        spawnRadius = GetComponent<SphereCollider>().radius;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("FPlayer within area.");
            InvokeRepeating("Spawn", 0f, spawnRate);
        }
    }
    void Spawn()
    {
        int temp = Random.Range(0, enemyTypes.Length);

        Instantiate(enemyTypes[temp], spawnCenter.position + (Random.insideUnitSphere * spawnRadius), spawnCenter.rotation);
    }
}
