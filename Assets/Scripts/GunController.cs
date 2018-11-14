using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour {

    public enum FireType {
        Automatic,
        Semi,
        Burst
    }

    public bool shooting = false;

    public FireType gunFireType;
    private Camera mainCamera;
    private AudioSource gunNoise;
    private AudioSource reloadNoise;

    private float shotTime;
    public float shootDelay;
    public float burstDelay;
    public int damage = 50;

    public int maxAmmo = 90;
    public int maxLoadedAmmo = 30;

    protected int loadedAmmo = 30;
    protected int unloadedAmmo = 90;

    private float reloadStart = 0.0f;
    //1 second reload time by default
    public float reloadTime = 1.0f;
    private bool reloading = false;

    // Use this for initialization
	void Start ()
    {
        mainCamera = Camera.main;
        var audio = GetComponents<AudioSource>();
        gunNoise    = audio[0];
        reloadNoise = audio[1];
	}
	
	// Update is called once per frame
	void Update () {
        if (reloading) {
            if (Time.time - reloadStart <= reloadTime) {
                reloading = false;
            }
        }
    }

    //TODO:Reloading isn't delaying like I want it too can fix this later
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

    public void addAmmo()
    {
        if (unloadedAmmo + maxLoadedAmmo >= maxAmmo)
            unloadedAmmo = maxAmmo;
        else
            unloadedAmmo += maxLoadedAmmo;
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

                switch (gunFireType) {
                    case FireType.Automatic:
                        break;
                    case FireType.Burst:
                        break;
                    case FireType.Semi:
                        if (shooting)
                            return;
                        break;
                }

                shotTime = Time.time;

                Vector3 cameraDir;
                Vector3 cameraPos;


                if(transform.root.tag == "Player")
                {
                    cameraDir = mainCamera.transform.forward;
                    cameraPos = mainCamera.transform.position;
                }
                else
                {
                    cameraDir = transform.forward;
                    cameraPos = transform.position;
                    if(transform.root.tag == "Ally")
                        transform.root.SendMessage("ShootAnimation");
                }
                RaycastHit results;

                gunNoise.Play();
                if (Physics.Raycast(cameraPos, cameraDir, out results)) {
                    if (results.collider.tag == "WeakPoint")
                        results.rigidbody.SendMessage("Shot", damage * 2);

                    else if (results.collider.tag == "Enemy")
                        results.collider.SendMessage("Shot", damage);


                }
                loadedAmmo--;
            }
        } else {
            reloadNoise.Play();
            reload();
        }

        shooting = true;
    }
}
