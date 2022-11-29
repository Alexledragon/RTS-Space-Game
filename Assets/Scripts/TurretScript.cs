using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [Header("1 for enemy, 2 for player")]
    [SerializeField] [Range(1, 2)] int targetType;

    //get script from owner ship
    [SerializeField] private TurretAI turretai;

    [Header("Turret Rotation")]
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private GameObject turretBarrel;
    private Transform targetShip;

    [Header("Shooting")]
    public Transform shootingSpot;
    public GameObject bullet;
    [SerializeField] private float bulletSpeed = 50f;

    [Header("Reloading")]
    [SerializeField] private float reloadTime = 1f;
    private float reloadTimer;

    private string bulletTargetString;

    // Start is called before the first frame update
    void Start()
    {
        reloadTimer = reloadTime;

        //apply the right behaviour depending on the one chosen
        switch (targetType)
        {
            case 1:
                bulletTargetString = "Enemy";
                break;

            case 2:
                bulletTargetString = "Player";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if there is a target, rotate towards it to shoot
        if (turretai.currentTarget != null)
        {
            targetShip = turretai.currentTarget;
            RotateTowardsTarget(targetShip);
        }
        //if there is no target, rotate to 0,0,0
        else if (turretai.currentTarget == null)
        {  
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, new Vector3(0, 0, 0), rotationSpeed, 0.0f);
            turretBarrel.transform.rotation = Quaternion.LookRotation(newDirection);
        }

        CanTurretShoot();

       

    }


    //method to rotate tip of turret towards target
    void RotateTowardsTarget(Transform target)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - transform.position;

        Vector3 interceptionDirection = GetInterceptionPoint(target, targetDirection);

        // Rotate the barrel towards target with rotatetowards, for 360 change the transform.forward of the newdirection rotatetowards to turretBarrel.transform.forward
        Vector3 newDirection = Vector3.RotateTowards(turretBarrel.transform.forward, interceptionDirection, rotationSpeed, 0.0f);
        turretBarrel.transform.rotation = Quaternion.LookRotation(newDirection);
    }


    //method to shoot forward from the canon using a bulletprefab
    void ShootCanon(GameObject BulletPrefab)
    {
        GameObject bullet = Instantiate(BulletPrefab, shootingSpot.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = turretBarrel.transform.TransformDirection(Vector3.forward * bulletSpeed);
        bullet.GetComponent<bulletHandler>().targetTag = bulletTargetString;

        //(FUTURE PROJECT, MAKE IT SO THAT IT SEES WHERE THE TARGET IS GOING AND SHOOTS ACORDINGLY USING THE TARGET'S RIGIDBODY VELOCITY, aka vector3.right * target.velocity.right * force)
    }

    //method to check if turret can shoot
    void CanTurretShoot()
    {
        //slowly decrease the timer using deltatime
        reloadTimer -= Time.deltaTime;

        //if the timer has reached 0 or below and we have a target, shoot then set timer back to reloadTime
        if  (turretai.currentTarget != null && reloadTimer <= 0)
        {
            ShootCanon(bullet);
            reloadTimer = reloadTime;
        }

    }

    //intake a transform and vector3 of a target, and determine the interception point to shoot it considering the target and bullet speed, give back the direction toward interception
    Vector3 GetInterceptionPoint(Transform targetTransform, Vector3 targetDirection)
    {
        //get distance to target, calculate the time the bullet would take to reach it
        float targetDistance = targetDirection.magnitude;
        float bulletTravelTime = targetDistance / bulletSpeed;

        //get the rigidbody associated with the target transform
        Rigidbody targetRigidbody = targetTransform.GetComponent<Rigidbody>();

        //create a vector 3 representing the movement and direction that the ship will take forward during the bullet travel time
        Vector3 targetMovePrediction = targetTransform.forward * (bulletTravelTime * targetRigidbody.velocity.magnitude);

        //add the target ship predicted movement to the current aim direction toward the target to shift it toward the predicted interception point
        Vector3 interceptionPoint = targetMovePrediction + targetDirection;
        return interceptionPoint;
    }
}
