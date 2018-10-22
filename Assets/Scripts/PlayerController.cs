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
    //TODO: Player rotation seems sketchy still. Might want to look into cleaning it up.
    public float movementSpeed = 5.0f;
    private float rotationX = 0f;

    //Delay in seconds
    public float shootDelay = .2f;
    private float shotTime = 0f;

    private Health health;


    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();
        health = GetComponent<Health>();

        uiHealth.text = health.currentHealth.ToString();
    }
	
    // Update is called once per frame
    void Update () {
        HandleInput();
        uiHealth.text = health.currentHealth.ToString();
    }

    // Called in fixed timesteps
    void FixedUpdate() {
        body.MoveRotation(localRotation);
        body.MovePosition(body.position + keyboardInputs * movementSpeed * Time.fixedDeltaTime);
    }

    public void Hit(int damage)
    {
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
