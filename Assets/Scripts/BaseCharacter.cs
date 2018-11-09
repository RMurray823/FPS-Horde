using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour {

    protected Rigidbody body;
    protected Health health;
    public float movementSpeed = 5.0f;

    virtual
    public void Init() {
        body = GetComponent<Rigidbody>();
        health = GetComponent<Health>(); 
    }
}
