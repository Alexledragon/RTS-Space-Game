using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    [Header("Aggro System")]
    [SerializeField] float turretMaxRange;
    [SerializeField] float turretMinRange;

    //reference the target picked by the AI to send to the turret
    [NonSerialized] public Transform currentTarget;

    //used to adapt the script to enemy and ally ship uses
    string adversaryFaction;

    //used to detect and sort all the detected target
    GameObject[] targetsDetected;
    Transform closestTarget;
    float smallestDistance = 0;
    Transform shipTarget;
    float shipTargetDistance;


    private void Start()
    {
        //check for root parent tag, target enemies if player, target player if enemy
        if(transform.parent.root.CompareTag("Enemy"))
        {
            adversaryFaction = "Player";
        }
        else
        {
            adversaryFaction = "Enemy";
        }
    }

    void Update()
    {
        SearchForTargets();
        PickTarget();
    }

    void SearchForTargets()
    {
        //----- Detect all the opponent and declare the closest one and the one targeted by the ship

        //put all the objects holding the enemy tag in an array and check each of their relative distances
        //if closest found yet, declare it's distance and transform as closest, if also target by ship, declare as ship target
        targetsDetected = GameObject.FindGameObjectsWithTag(adversaryFaction);

        foreach (GameObject target in targetsDetected)
        {
            float targetDistance = (target.transform.position - transform.position).magnitude;

            if(targetDistance < smallestDistance | smallestDistance == 0)
            {
                smallestDistance = targetDistance;
                closestTarget = target.transform;
            }

            if (target.transform == GetComponentInParent<ShipManager>().Target)
            {
                shipTarget = target.transform;
                shipTargetDistance = targetDistance;
            }
        }
    }

    void PickTarget()
    {
        //----- Compare the different possible targets, define which one to give to the turret if any is within range

        //pick the ship target as turret target if within range, else pick the closest ship in range, just pick null if none is within range
        if (shipTargetDistance <= turretMaxRange & shipTargetDistance >= turretMinRange)
        {
            currentTarget = shipTarget;
        }
        else if (smallestDistance <= turretMaxRange & smallestDistance >= turretMinRange)
        {
            currentTarget = closestTarget;
        }
        else
        {
            currentTarget = null;
        }
    }
}
