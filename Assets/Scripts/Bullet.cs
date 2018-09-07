using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float maxVelocity = 5f;

    // Use this for initialization
    void Start() {
        Rigidbody body = GetComponent<Rigidbody>();
        body.velocity = transform.forward * maxVelocity;
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Player" && other.tag != "Bullet")
            Destroy(gameObject);
    }
}
