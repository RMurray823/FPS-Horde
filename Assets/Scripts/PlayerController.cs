using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float movementSpeed = 5.0f;

    private float rotationX = 0f;

    private Rigidbody body;

    private Vector3 keyboardInputs = Vector3.zero;
    private Vector3 mouseInput = Vector3.zero;

    private Quaternion localRotation = Quaternion.identity;

    public GameObject bullet;
    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();
    }
	
    // Update is called once per frame
    void Update () {
        HandleInput();
    }

    private void HandleInput() {
        if (Input.GetKey(KeyCode.Escape)) {
            if (Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetKeyUp(KeyCode.Space)) {
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
        Vector3 localPos = transform.forward;
        Instantiate(bullet, Camera.main.transform.position + localPos, Camera.main.transform.rotation);
    } 

    void FixedUpdate() {
        body.MoveRotation(localRotation);
        body.MovePosition(body.position + keyboardInputs * movementSpeed * Time.fixedDeltaTime);
    }
}
