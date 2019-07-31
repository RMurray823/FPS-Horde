using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController:MonoBehaviour {

	private float rotationY = 0f;
	private float rotationX = 0f;
	private Vector3 mouseInput = Vector3.zero;

	public Vector3 relativePosition;

	private void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update() {

		HandleInput();

	}

	private void HandleInput() {
		mouseInput = Vector3.zero;
		mouseInput.y = -Input.GetAxis("Mouse Y");

		rotationY += mouseInput.y * 100f * Time.deltaTime;

		rotationY = Mathf.Clamp(rotationY, -90, 90);
		Quaternion localRotation = Quaternion.Euler(rotationY, rotationX, 0);

		transform.localRotation = localRotation;
	}


}
