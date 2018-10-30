using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    private Camera mainCamera;
    private AudioSource gunNoise;

    private float shotTime;
    public float shootDelay;

    public int maxAmmo = 90;
    public int maxLoadedAmmo = 30;

    protected int loadedAmmo = 30;
    protected int unloadedAmmo = 90;

    public int damage = 50;

    private float reloadStart = 0.0f;
    //1 second reload time by default
    public float reloadTime = 1.0f;
    private bool reloading = false;


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

        if (reloading) {
            if (Time.time - reloadStart <= reloadTime) {
                reloading = false;
            }
        }
    }

    //Reloading isn't delaying like I want it too can fix this later
    public void reload() {

        int neededShots = maxLoadedAmmo - loadedAmmo;

        if(unloadedAmmo >= neededShots) {
            loadedAmmo += neededShots;
            unloadedAmmo -= neededShots;
        } else{
            if (unloadedAmmo > 0) {
                loadedAmmo += unloadedAmmo;
                unloadedAmmo = 0;
            } else {
                return;
            }
        }

        reloading = true;
        reloadStart = Time.time;
    }

    public int getAmmoInClip() {
        return loadedAmmo;
    }

    public int getAmmoNotInClip() {
        return unloadedAmmo;
    }
    public void fireBullet() {
        if(loadedAmmo > 0 && !reloading) {
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

                loadedAmmo--;
            }
        } else {
            reload();
        }
    }
}
