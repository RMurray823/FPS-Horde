﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private float rotationY = 0f;
    private float rotationX = 0f;
    private Vector3 mouseInput = Vector3.zero;

    public bool gunFiring = false;

    // Update is called once per frame
    void Update() {

            HandleInput();
      
    }

    private void HandleInput() {

        Cursor.visible = false;

        mouseInput = Vector3.zero;
        mouseInput.y = -Input.GetAxis("Mouse Y");
        mouseInput.x = Input.GetAxis("Mouse X");

        rotationY += mouseInput.y * 100f * Time.deltaTime;
        rotationX += mouseInput.x * 100f * Time.deltaTime;

        rotationY = Mathf.Clamp (rotationY, -90, 90);

        if (gunFiring == true) {
            rotationY -= 2 ;
            gunFiring = false;
        }

        Quaternion localRotation = Quaternion.Euler(rotationY, rotationX, 0);

        transform.localRotation = localRotation;
    }

    public void Firing(bool currentlyFiring) {
        if (currentlyFiring == true)
        {
            gunFiring = true;
        }
    }
       




}
