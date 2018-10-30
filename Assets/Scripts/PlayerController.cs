using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {
    private Rigidbody body;

    private Vector3 keyboardInputs = Vector3.zero;
    private Vector3 mouseInput = Vector3.zero;

    private Quaternion localRotation = Quaternion.identity;

    public Text uiHealth;
    public Text uiArmor;

    //Remaining ammo not loaded
    public Text uiRemainingAmmo;
    //Currently loaded
    public Text uiClipAmmo;

    //TODO: Player rotation seems sketchy still. Might want to look into cleaning it up.
    public float movementSpeed = 5.0f;
    private float rotationX = 0f;

    private Health health;

    //Currently held gun info
    public GameObject heldGun;
    private GunController gunController;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();
        health = GetComponent<Health>();

        gunController = heldGun.GetComponent<GunController>();

        //Initialize text UI components
        uiHealth.text = health.currentHealth.ToString();
        uiArmor.text = health.currentArmor.ToString();
        uiClipAmmo.text = gunController.getAmmoInClip().ToString();
        uiRemainingAmmo.text = gunController.getAmmoNotInClip().ToString();

    }
	
    // Update is called once per frame
    void Update () {
        HandleInput();
        UpdateUI();
    }

    private void UpdateUI() {
        uiHealth.text = health.currentHealth.ToString();
        uiArmor.text = health.currentArmor.ToString();

        uiClipAmmo.text = gunController.getAmmoInClip().ToString();
        uiRemainingAmmo.text = gunController.getAmmoNotInClip().ToString();
    }

    void FixedUpdate() {
        body.MoveRotation(localRotation);
        body.MovePosition(body.position + keyboardInputs * movementSpeed * Time.fixedDeltaTime);
    }

    public void Hit(int damage) {
        if(health.takeDamage(damage) <= 0) {
            Debug.Log("dead");
        }
        uiHealth.text = health.currentHealth.ToString();
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
