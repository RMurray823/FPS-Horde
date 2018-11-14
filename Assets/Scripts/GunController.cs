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

    private bool triggerHeld = false;

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

    public int loadedAmmo = 30;
    public int unloadedAmmo = 90;

    public bool infiniteAmmo = false;

    private float reloadStart = 0.0f;
    //1 second reload time by default
    public float reloadTime = 1.0f;
    private bool reloading = false;


    public int numOfBurstShots;

    //Tracks when to stop invokerepeat
    private int burstCount;

    // Use this for initialization
    void Start() {
        mainCamera = Camera.main;
        var audio = GetComponents<AudioSource>();
        gunNoise = audio[0];
        reloadNoise = audio[1];
        burstCount = 0;
    }

    // Update is called once per frame
    void Update() {
        if (reloading) {
            if (Time.time - reloadStart <= reloadTime) {
                reloading = false;
            }
        }
    }

    //TODO:Reloading isn't delaying like I want it too can fix this later
    public void reload() {
        int neededShots = maxLoadedAmmo - loadedAmmo;

        if (unloadedAmmo >= neededShots) {
            loadedAmmo += neededShots;
            unloadedAmmo -= neededShots;
        } else {
            if (unloadedAmmo > 0) {
                loadedAmmo += unloadedAmmo;
                unloadedAmmo = 0;
            } else {

                return;
            }
        }
        reloadNoise.Play();

        reloading = true;
        reloadStart = Time.time;
    }

    public void addAmmo() {
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

    private void FireBullet() {

        //Cancel the repeating invoke
        if (++burstCount == numOfBurstShots) CancelInvoke("FireBullet");

        Vector3 cameraDir;
        Vector3 cameraPos;

        //TODO: I don't think we should have to do these checks. Maybe we should pass to the guncontroller where the bullet should exit
        if (transform.root.tag == "Player") {
            cameraDir = mainCamera.transform.forward;
            cameraPos = mainCamera.transform.position;
        } else {
            cameraDir = transform.forward;
            cameraPos = transform.position;
            if (transform.root.tag == "Ally")
                transform.root.SendMessage("ShootAnimation");
        }

        RaycastHit results;
        //TODO: this shouldn't be handled here. It should be handled in the enemy class
        if (Physics.Raycast(cameraPos, cameraDir, out results)) {
            ShotInformation info = new ShotInformation();
            info.damage = damage;
            info.tag = results.collider.tag;

            //We send the shot to the root of the collider we shot. This might not be ideal if we want gun shots to appear where the "bullet" hits
            results.collider.transform.root.SendMessage("Shot", info, SendMessageOptions.DontRequireReceiver);
        }

        gunNoise.Play();

        if (!infiniteAmmo) {
            if (--loadedAmmo == 0) {
                CancelInvoke("FireBullet");
            }
        }
    }

    //Returns true if we actually shot or not.
    public bool Shoot() {
        if (loadedAmmo > 0 && !reloading) {
            if (Time.time - shotTime >= shootDelay) {
                shotTime = Time.time;

                switch (gunFireType) {
                    case FireType.Automatic:
                        FireBullet();
                        break;
                    case FireType.Burst:
                        burstCount = 0;
                        InvokeRepeating("FireBullet", 0, burstDelay);
                        break;
                    case FireType.Semi:
                        if (triggerHeld)
                            return false;
                        else
                            FireBullet();
                        break;
                }
            }
            triggerHeld = true;
            return true;

        } else {
            reload();
            return false;
        }
    }

    public void SetShooting(bool flag) {
        triggerHeld = flag;
    }
}

