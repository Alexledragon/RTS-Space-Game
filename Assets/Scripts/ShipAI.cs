using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipAI : MonoBehaviour
{
    [Header("AI System")]
    [SerializeField][Range(1, 2)] int behaviourType;

    [Header("Broaside Behaviour (1)")]
    [SerializeField] float broadEngageDistance;
    [SerializeField] float broadDistance;
    [SerializeField] float broadRetreatDistance;
    [SerializeField] float broadEngageAngle;
    [SerializeField] float broadAngle;
    [SerializeField] float broadRetreatAngle;
    [SerializeField] float broadEngageSpeedPercent;
    [SerializeField] float broadSpeedPercent;
    [SerializeField] float broadRetreatSpeedPercent;

    [Header("Torpedo Skirmish Behaviour (2)")]
    [SerializeField] GameObject torpedo;
    [SerializeField] Transform torpedoTube;
    [SerializeField] float torpedoRechargeTime;
    [SerializeField] float torpedoShootRange;
    [SerializeField] float torpedoRetreatDistance;
    [SerializeField] float torpedoApproachDistance;
    [SerializeField] float torpedoEngagmentSpeedPercent;
    [SerializeField] float torpedoRetreatSpeedPercent;
    [SerializeField] float torpedoApproachSpeedPercent;
    [SerializeField] float torpedoSafetySpeedPercent;

    bool torpedoIsLoaded;
    float torpedoTimer;


    //store a "movement order" as a normalised Vector3 direction for movement script
    [NonSerialized] public Vector3 aiDirection;
    [NonSerialized] public float aiSpeedPercent;

    //reference the target and ship transform
    [NonSerialized] public Transform targetTransform;
    Transform shipTransform;

    //collect info about target
    float targetDistance;
    Vector3 targetDirection;
    float targetSide;


    private void Start()
    {
        targetTransform = GetComponent<ShipManager>().Target;

        //assign the transform component to the gameobject holding the script
        shipTransform = GetComponent<Transform>();

        //set the torpedo reload timer to the set time and enable the torpedo to be shot at spawn
        torpedoTimer = torpedoRechargeTime;
        torpedoIsLoaded = true;
    }

    private void Update()
    {
        //check for the selected behaviour for the AI, if torpedo skirmisher was picked, enable the torpedo reload cooldown
        if(behaviourType == 2)
        {
            CheckTorpedoCooldown();
        }
    }

    void FixedUpdate()
    {
        checkTargetState();

        //apply the right behaviour depending on the one chosen
        switch (behaviourType)
        {
            case 1: BroadSideBehaviour();
                break;

            case 2: TorpedoSkirmishBehaviour();
                break;
        }
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

    void TorpedoSkirmishBehaviour()
    {
        //----- Make the ship approach the target to shoot a torpedo then retreat while it reload

        //check if a torpedo is loaded and ready to fire, get in range to shoot if yes, stay at a set distance until ready to shoot if not
        if(torpedoIsLoaded == true)
        {
            //make the ship move toward the target
            aiDirection = targetDirection;
            aiSpeedPercent = torpedoEngagmentSpeedPercent;

            //if the ship is at shooting range, instanciate a torpedo with an initial speed% equal to the ship speed% and reset the torpedo reload timer and state
            if (targetDistance <= torpedoShootRange)
            {
                GameObject lastFiredTorpedo = Instantiate(torpedo, torpedoTube.position, torpedoTube.rotation, shipTransform);
                lastFiredTorpedo.GetComponent<TorpedoManager>().currentSpeedPercent = GetComponent<ShipMovement>().currentSpeedPercent;

                torpedoTimer = 0;
                torpedoIsLoaded = false;
            }
        }
        else
        {
            //if the target is too close, move away from it
            if(targetDistance <= torpedoRetreatDistance)
            {
                aiDirection = Quaternion.AngleAxis(180, Vector3.up) * targetDirection;
                aiSpeedPercent = torpedoRetreatSpeedPercent;
            }
            //if the target is at a safe distance, turn around clockwise
            else if(targetDistance >= torpedoApproachDistance)
            {
                aiDirection = shipTransform.right;
                aiSpeedPercent = torpedoSafetySpeedPercent;
            }
            //if the target is too far, try to get closer back to safe distance
            else
            {
                aiDirection = targetDirection;
                aiSpeedPercent = torpedoApproachSpeedPercent;
            }
        }
    }

    void CheckTorpedoCooldown()
    {
        //----- Handle the torpedo reload system

        //if the timer did not reach the wanted time, increment it over time, when the time is reached, set the torpedo as ready to shoot
        if(torpedoTimer < torpedoRechargeTime)
        {
            torpedoTimer += Time.deltaTime;
        }
        else
        {
            torpedoIsLoaded = true;
        }
    }
}
