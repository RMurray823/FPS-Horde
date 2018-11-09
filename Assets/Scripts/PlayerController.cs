using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : BaseAllyCharacter {
    private Vector3 keyboardInputs = Vector3.zero;
    private Vector3 mouseInput = Vector3.zero;

    private Quaternion localRotation = Quaternion.identity;

    //TODO: Player rotation seems sketchy still. Might want to look into cleaning it up.
    private float rotationX = 0f;

    // Use this for initialization
    void Start () {
        base.Init();
    }
	
    // Update is called once per frame
    void Update () {
        HandleInput();
    }

    void FixedUpdate() {
        body.MoveRotation(localRotation);
        body.MovePosition(body.position + keyboardInputs * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag=="HealthPack") {
            HealthPack pack = other.GetComponent<HealthPack>();
            health.heal(pack.getHealAmount());
        }

        if (other.tag == "ArmorPack") {
            HealthPack pack = other.GetComponent<HealthPack>();
            health.healArmor(pack.getArmorAmount());
        }
    }

    private void HandleInput() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            heldGun = playerInventory.swapGun();
            gunController = heldGun.GetComponent<GunController>();
        }
        if(Input.GetKey(KeyCode.Mouse0)) {
            gunController.fireBullet();
        }

        keyboardInputs = Vector3.zero;
        keyboardInputs.x = Input.GetAxis("Horizontal");
        keyboardInputs.z = Input.GetAxis("Vertical");

        keyboardInputs = transform.TransformDirection(keyboardInputs);

        mouseInput = Vector3.zero;
        mouseInput.x = Input.GetAxis("Mouse X");
        rotationX += mouseInput.x * 100f * Time.fixedDeltaTime;

        localRotation = Quaternion.Euler(0, rotationX, 0);
    }
}
