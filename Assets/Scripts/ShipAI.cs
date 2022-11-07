using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipAI : MonoBehaviour
{
    [Header("Broaside Behaviour")]
    [SerializeField] float broadEngageDistance;
    [SerializeField] float broadDistance;
    [SerializeField] float broadRetreatDistance;
    [SerializeField] float broadEngageAngle;
    [SerializeField] float broadAngle;
    [SerializeField] float broadRetreatAngle;
    [SerializeField] float broadEngageSpeedPercent;
    [SerializeField] float broadSpeedPercent;
    [SerializeField] float broadRetreatSpeedPercent;


    //store a "movement order" as a normalised Vector3 direction for movement script
    [NonSerialized] public Vector3 aiDirection;
    [NonSerialized] public float aiSpeedPercent;

    //reference the target and ship transform
    [SerializeField] Transform targetTransform;
    Transform shipTransform;

    //collect info about target
    float targetDistance;
    Vector3 targetDirection;
    float targetSide;


    private void Start()
    {
        //assign the transform component to the gameobject holding the script
        shipTransform = GetComponent<Transform>();
    }

    void Update()
    {
        checkTargetState();
        BroadSideBehaviour();
    }

    void checkTargetState()
    {
        //----- Check the target state, output what the AI need to know for further decisions

        //get the direction vector between the ship and target, turn it's magnitude into the distance value and normalise the direction vector
        targetDirection = targetTransform.position - shipTransform.position;
        targetDistance = targetDirection.magnitude;
        targetDirection = targetDirection.normalized;

        //get the angle difference and direction between front of ship and target direction then only store the sign of the obtained angle
        //will output 1 if target on the right, -1 on the left, used to reverse manoeuvers sensitive to the target side relative to ship
        targetSide = Vector3.SignedAngle(shipTransform.forward, targetDirection, Vector3.up);
        targetSide = Mathf.Sign(targetSide);
    }

    void MoveTowardTarget()
    {
        //----- Make the ship move in a straight line toward target

        //set AI direction vector to target direction, set speed to 100%
        aiDirection = targetDirection;
        aiSpeedPercent = 100f;
    }

    void StandStill()
    {
        //----- Make the ship stop moving

        //set AI direction vector to ship direction to cancel rotation, set speed to 0%
        aiDirection = shipTransform.forward;
        aiSpeedPercent = 0f;
    }

    void BroadSideBehaviour()
    {
        //----- Get in range of the target and initiate broadside manoeuvers

        //check the distance with the target to act accordingly, if distance does not allow broadsiding, get closer
        //take the target direction vector and rotate it by the required angle to the left or right in relation to the target
        //assign obtained direction vector and speed to ship
        if(targetDistance < broadRetreatDistance) //if too close, bellow the wanted range
        {
            //make the ship try to put distance with the target slowly, at an angle
            aiDirection = Quaternion.AngleAxis(-broadRetreatAngle * targetSide, Vector3.up) * targetDirection;
            aiSpeedPercent = broadRetreatSpeedPercent;
        }
        else if(targetDistance < broadDistance) //if at ideal distance
        {
            //make the ship try to remain at his distance, to broadside the enemy
            aiDirection = Quaternion.AngleAxis(-broadAngle * targetSide, Vector3.up) * targetDirection;
            aiSpeedPercent = broadSpeedPercent;
        }
        else if(targetDistance < broadEngageDistance) //if close enough to start engaging, above ideal broadside range
        {
            //make the ship try to approach the enemy slowly at an angle
            aiDirection = Quaternion.AngleAxis(-broadEngageAngle * targetSide, Vector3.up) * targetDirection;
            aiSpeedPercent = broadEngageSpeedPercent;
        }
        else //if too far to engage, above wanted range
        {
            //call on the move toward function to get closer
            MoveTowardTarget();
        }
    }
}
