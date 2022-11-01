using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Rotations")]
    [SerializeField] float mouseSensitivity;
    [SerializeField] float maxVerticalAngle;
    [SerializeField] float minVerticalAngle;
    [Header("Movements")]
    [SerializeField] float movementSpeed;
    [SerializeField] float elevationSpeed;
    [SerializeField] float maxElevation;
    [SerializeField] float minElevation;

    //used to register user input values
    float inputMouseX;
    float inputMouseY;
    float inputMouseScroll;
    float inputKeyHorizontal;
    float inputKeyVertical;

    //used to treat and calculate camera movements
    float cameraRotationX;
    float cameraRotationY;
    float wantedCameraElevation;
    float cameraElevation;

    private void Start()
    {
        //make starting camera vertical angle half between min and max values
        cameraRotationX = (maxVerticalAngle + minVerticalAngle) / 2f;
        //make camera look toward Z at start
        cameraRotationY = 0;
        //make starting camera elevation half between min and max values
        wantedCameraElevation = (maxElevation + minElevation) / 2f;
        cameraElevation = wantedCameraElevation;
    }

    void Update()
    {
        ManageCursor();
        RegisterInputs();

        RotateCamera();
        MoveCamera();
        ElevateCamera();
    }


    //---------- CUSTOM FUNCTIONS


    void RegisterInputs()
    {
        //----- Register the inputs used by the user

        //register the mouse movement inputs
        inputMouseX = Input.GetAxis("Mouse X");
        inputMouseY = Input.GetAxis("Mouse Y");

        //register the mouse scroll input
        inputMouseScroll = Input.GetAxis("Mouse ScrollWheel");

        //register the keyboard movement inputs
        inputKeyHorizontal = Input.GetAxisRaw("Horizontal");
        inputKeyVertical = Input.GetAxisRaw("Vertical");
    }


    void RotateCamera()
    {
        //----- Rotate the camera in relation to the user mouse movements

        //if right click is held down, increment the X and Y camera rotation values with mouse inputs, keep X rotation within wanted boundaries and hide the cursor
        if (Input.GetMouseButton(1))
        {
            cameraRotationX -= inputMouseY * mouseSensitivity;
            cameraRotationX = Mathf.Clamp(cameraRotationX, maxVerticalAngle, minVerticalAngle);

            cameraRotationY += inputMouseX * mouseSensitivity;
        }

        //apply the obtained rotation values to the camera transform
        transform.localRotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0f);
    }


    void MoveCamera()
    {
        //----- Move the camera on the X and Z axis in relation to the user keyboard inputs

        //register the camera local forward direction and remove the Y direction to force the vector to stay horizontal
        Vector3 forward = transform.forward;
        forward = new Vector3(forward.x, 0f, forward.z);
        //register the camera local right direction
        Vector3 right = transform.right;

        //create a vector with the inputed movements oriented toward the local directions, normalise that vector and scale it to wanted speed
        Vector3 cameraMovement = inputKeyVertical * forward + inputKeyHorizontal * right;
        cameraMovement = cameraMovement.normalized * movementSpeed;

        //use the movement vector to increment the camera transform position over time and move it
        transform.position += cameraMovement * Time.deltaTime;
    }


    void ElevateCamera()
    {
        //----- Move the camera on the Y axis in relation to the user mouse wheel inputs

        //increment the wanted camera elevation value to reach with the mouse wheel input value, keep it within wanted boundaries
        wantedCameraElevation -= inputMouseScroll * elevationSpeed;
        wantedCameraElevation = Mathf.Clamp(wantedCameraElevation, minElevation, maxElevation);

        //to smoothen the camera movement, make the camera elevation value gradually move toward the wanted elevation
        cameraElevation = Mathf.MoveTowards(cameraElevation, wantedCameraElevation, elevationSpeed / 150);

        //apply the obtained camera elevation value to the camera transform without affecting the X and Z positions
        transform.position = new Vector3(transform.position.x, cameraElevation, transform.position.z);
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
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
