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

        health.maxHealth = 100;
        health.currentHealth = 100;
        uiHealth.text = health.currentHealth.ToString();
    }
	
    // Update is called once per frame
    void Update () {
        HandleInput();
    }

    // Called in fixed timesteps
    void FixedUpdate() {
        body.MoveRotation(localRotation);
        body.MovePosition(body.position + keyboardInputs * movementSpeed * Time.fixedDeltaTime);
    }

    public void hit(int damage) {

        if(health.takeDamage(damage) <= 0) {
            Debug.Log("dead");
        }

        uiHealth.text = health.currentHealth.ToString();

    }

    private void HandleInput() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetKey(KeyCode.Mouse0)) {
            fireBullet();
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

    public void fireBullet() {
        if(Time.time - shotTime >= shootDelay) {
            shotTime = Time.time;

            Vector3 cameraDir = Camera.main.transform.forward;
            Vector3 cameraPos = Camera.main.transform.position;
            RaycastHit results;

            if (Physics.Raycast(cameraPos, cameraDir, out results)) {
                if(results.collider.tag == "Enemy") {
                    results.collider.BroadcastMessage("Shot");
                }
            }
        }	
    } 
}
