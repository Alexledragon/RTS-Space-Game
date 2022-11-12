using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField] float maxSteeringSpeed;

    //reference the body meant to be moved by the script
    Rigidbody shipRigidBody;
    Transform shipTransform;
    //reference the AI script and the variables it will use
    ShipAI shipAI;
    Vector3 targetDirection;
    float targetSpeedPercent;

    //used for forward movement
    [DoNotSerialize] public float currentSpeedPercent;
    Vector3 currentDirection;


    void Start()
    {
        //assign the transform, rigidbody and AI script references to the gameobject holding the script
        shipRigidBody = GetComponent<Rigidbody>();
        shipTransform = GetComponent<Transform>();
        shipAI = GetComponent<ShipAI>();
    }

    private void FixedUpdate()
    {
        GetAIOrders();
        MoveShipForward();
        SteerShip();
    }



    void MoveShipForward()
    {
        //----- Make the ship move in the local forward direction at AI requested speed

        //register the normalised local forward direction
        Vector3 forward = shipTransform.forward;

        //gradually increment the current speed% to reach the target speed%, making movements smoother
        currentSpeedPercent = Mathf.MoveTowards(currentSpeedPercent, targetSpeedPercent, acceleration * Time.fixedDeltaTime);

        //create a vector scaling the forward direction to the speed currently used
        Vector3 forwardMovement = forward * (speed / 100f * currentSpeedPercent);

        //apply the wanted velocity to the ship
        shipRigidBody.velocity = forwardMovement;
    }

    void SteerShip()
    {
        //----- Steer the ship left and right to aim toward the direction given by AI

        //scale the speed at which the ship steer depending on the speed% it currently have, 5% of the steering speed ignore the speed%
        float currentSteerSpeed = (maxSteeringSpeed / 100 * 5) + (maxSteeringSpeed / 100 * 95) / 100 * currentSpeedPercent;

        //gradually increment the current Y rotation angle to reach the target direction given by the AI, making the steering smoother
        currentDirection = Vector3.RotateTowards(shipTransform.forward, targetDirection, currentSteerSpeed * Time.fixedDeltaTime, 0f);

        //apply the obtained rotation angle to the ship rigidbody
        shipRigidBody.rotation = Quaternion.LookRotation(currentDirection);
    }

    void GetAIOrders()
    {
        //----- Get and update the orders sent by the ai script

        //get the direction and speed wanted by the ai
        targetDirection = shipAI.aiDirection;
        targetSpeedPercent = shipAI.aiSpeedPercent;
    }
}
