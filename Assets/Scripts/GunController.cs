using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    private Camera mainCamera;
    private AudioSource gunNoise;

    private float shotTime;
    public float shootDelay;
    public int damage = 50;
    // Use this for initialization
	void Start () {
        mainCamera = Camera.main;
        gunNoise = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Mouse0)) {
            fireBullet();
        }
    }

    public void fireBullet() {
        if (Time.time - shotTime >= shootDelay) {
            shotTime = Time.time;

            Vector3 cameraDir = mainCamera.transform.forward;
            Vector3 cameraPos = mainCamera.transform.position;
            RaycastHit results;

            gunNoise.Play();
            if (Physics.Raycast(cameraPos, cameraDir, out results)) {
                if (results.collider.tag == "WeakPoint")
                    results.rigidbody.SendMessage("CriticalHit", damage);

                else if (results.collider.tag == "Enemy")
                    results.collider.SendMessage("Shot", damage);
            }
        }
    }
}
