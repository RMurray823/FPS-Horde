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
        if(Input.GetKey(KeyCode.Escape)) {
            if(Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }

        keyboardInputs = Vector3.zero;
        keyboardInputs.x = Input.GetAxis("Horizontal");
        keyboardInputs.z = Input.GetAxis("Vertical");

        mouseInput = Vector3.zero;
        mouseInput.x = Input.GetAxis("Mouse X");
        //mouseInput.y = -Input.GetAxis("Mouse Y");

        rotationX += mouseInput.x * 100f * Time.deltaTime;
        //rotationY += mouseInput.y * 100f * Time.deltaTime;

        Quaternion localRotation = Quaternion.Euler(0, rotationX, 0);
        transform.rotation = localRotation;

        keyboardInputs = Camera.main.transform.TransformDirection(keyboardInputs);
    }

    private void FixedUpdate() {
        body.MovePosition(body.position + keyboardInputs * movementSpeed * Time.fixedDeltaTime);
    }
}
