using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;
    [SerializeField] float maxVerticalCameraAngle;
    [SerializeField] float minVerticalCameraAngle;

    float inputMouseX;
    float inputMouseY;
    float cameraRotationX;
    float cameraRotationY;

    private void Start()
    {
        
    }

    void Update()
    {
        ManageCursor();
        RegisterInputs();
        RotateCamera();
    }


    //---------- CUSTOM FUNCTIONS

    void RegisterInputs()
    {
        //----- Register the inputs used by the user

        //register the mouse movement inputs
        inputMouseX = Input.GetAxis("Mouse X");
        inputMouseY = Input.GetAxis("Mouse Y");
    }

    void RotateCamera()
    {
        //----- Rotate the camera in relation to the user mouse movements

        //if right click is held down, increment the X and Y camera rotation values, keep X rotation within wanted boundaries and hide the cursor
        if (Input.GetMouseButton(1))
        {
            cameraRotationX -= inputMouseY * mouseSensitivity;
            cameraRotationX = Mathf.Clamp(cameraRotationX, maxVerticalCameraAngle, minVerticalCameraAngle);

            cameraRotationY += inputMouseX * mouseSensitivity;
        }

        //apply the obtained rotation values to the camera transform
        transform.localRotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0f);
    }

    void ManageCursor()
    {
        //----- Manage the cursor lock and visibility depending on the inputs currently pressed

        //lock and hide the cursor when right click is held
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        //confine and show the cursor if no input is pressed
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
