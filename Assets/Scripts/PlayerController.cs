using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float movementSpeed = 5.0f;
    private float rotationX = 0f;
    private float rotationY = 0f;

    private Rigidbody body;

    private Vector3 keyboardInputs = Vector3.zero;
    private Vector3 mouseInput = Vector3.zero;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();
    }
	
    // Update is called once per frame
    void Update () {
        HandleInput();
    }

    //TODO: Character slows down when looking up and down. Need to stop this from happening
    private void HandleInput() {
        if (Input.GetKey(KeyCode.Escape)) {
            if (Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }

        keyboardInputs = Vector3.zero;
        keyboardInputs.x = Input.GetAxis("Horizontal");
        keyboardInputs.z = Input.GetAxis("Vertical");

        //This needs to be changed up to fix movement inconsistencies
        keyboardInputs = Camera.main.transform.TransformDirection(keyboardInputs);

        mouseInput = Vector3.zero;
        mouseInput.x = Input.GetAxis("Mouse X");

        rotationX += mouseInput.x * 100f * Time.deltaTime;

        Quaternion localRotation = Quaternion.Euler(0, rotationX, 0);
        transform.rotation = localRotation;
    }

    private void FixedUpdate() {
        //Temporary fix to stop the character from trying to move up and down the Y-Axis when camera is facing up/down
        keyboardInputs.y = 0;
        body.MovePosition(body.position + keyboardInputs * movementSpeed * Time.fixedDeltaTime);
    }
}
