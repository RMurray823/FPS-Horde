﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalEvent : MonoBehaviour
{
    public GameObject[] enemyTypes;
    public Transform[] spawnPoints;

    private SphereCollider spawnArea;
    private Transform player;


    // Use this for initialization
    void Start()
    {
        spawnArea = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("Spawn", 0f, 3f);
    }

    void OnTriggerEvent(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("FPlayer within area.");
            InvokeRepeating("Spawn", 0f, 3f);
        }
    }
    void Spawn()
    {

        int temp = Random.Range(0, enemyTypes.Length);
        int spot = Random.Range(0, spawnPoints.Length);

        Instantiate(enemyTypes[temp], spawnPoints[spot].position, spawnPoints[spot].rotation);
    }
}
