using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float mouseSensitivity = 500f;
    public new GameObject camera;

    float xRotation = 0f;
    float YRotation = 0f;

    void Start()
    {
        //Locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    void Update()
    {

        if(!InventorySystem.Instance.isOpen)
        { 
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            //control rotation around x axis (Look up and down)
            xRotation -= mouseY;

            //we clamp the rotation so we cant Over-rotate (like in real life)
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //control rotation around y axis (Look up and down)
            YRotation += mouseX;

            //applying both rotations
            camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.localRotation = Quaternion.Euler(0f, YRotation, 0f);

        }

    }
}