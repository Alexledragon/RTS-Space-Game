using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

//make sure the gameobject have a rigidbody and ship AI script
[RequireComponent(typeof(Rigidbody))]

public class TorpedoManager : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField] float maxSteeringSpeed;

    //reference the ShipAI that instantiated the torpedo
    ShipAI parentShipAi;

    //reference the target transform and store informations obtained from it
    Transform targetTransform;
    Vector3 targetDirection;

    //reference the body meant to be moved by the script
    Rigidbody torpedoRigidBody;
    Transform torpedoTransform;

    //used for forward movement
    [DoNotSerialize] public float currentSpeedPercent;
    Vector3 currentDirection;

    
    void Start()
    {
        //assign the transform, rigidbody and AI script references to the gameobject holding the script
        torpedoRigidBody = GetComponent<Rigidbody>();
        torpedoTransform = GetComponent<Transform>();

        //define the ShipAI that instantiated the torpedo, get it's target transform and unchild from it
        parentShipAi = GetComponentInParent<ShipAI>();
        targetTransform = parentShipAi.targetTransform;
        transform.parent = null;
    }

    private void FixedUpdate()
    {
        GetTargetInfo();
        MoveTorpedoForward();
        SteerTorpedo();
    }



    void MoveTorpedoForward()
    {
        //----- Make the torpedo move in the local forward direction at AI requested speed

        //register the normalised local forward direction
        Vector3 forward = torpedoTransform.forward;

        //gradually increment the current speed% to reach the maximum speed, making movements smoother
        currentSpeedPercent = Mathf.MoveTowards(currentSpeedPercent, 100, acceleration * Time.deltaTime);

        //create a vector scaling the forward direction to the speed currently used
        Vector3 forwardMovement = forward * (speed / 100f * currentSpeedPercent);

        //apply the wanted velocity to the torpedo
        torpedoRigidBody.velocity = forwardMovement;
    }

    void SteerTorpedo()
    {
        //----- Steer the torpedo left and right to aim toward the direction given by AI

        //scale the speed at which the torpedo steer depending on the speed% it currently have, 5% of the steering speed ignore the speed%
        float currentSteerSpeed = (maxSteeringSpeed / 100 * 5) + (maxSteeringSpeed / 100 * 95) / 100 * currentSpeedPercent;

        //gradually increment the current Y rotation angle to reach the target direction, making the steering smoother
        currentDirection = Vector3.RotateTowards(torpedoTransform.forward, targetDirection, currentSteerSpeed * Time.deltaTime, 0f);

        //apply the obtained rotation angle to the torpedo rigidbody
        torpedoRigidBody.rotation = Quaternion.LookRotation(currentDirection);
    }

    void GetTargetInfo()
    {
        //----- Get the informations about the target that the torpedo require to function

        //get the vector between the torpedo and target
        targetDirection = targetTransform.position - torpedoTransform.position;
    }
}
