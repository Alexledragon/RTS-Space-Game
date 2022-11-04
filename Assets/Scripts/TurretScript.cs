using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    //get script from owner ship
    [SerializeField] private ShipManager shipScr;

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

    // Start is called before the first frame update
    void Start()
    {
        reloadTimer = reloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        //if there is a target, rotate towards it to shoot
        if (shipScr.Target != null)
        {
            targetShip = shipScr.Target;
            RotateTowardsTarget(targetShip);
        }
        //if there is no target, rotate to 0,0,0
        else if (shipScr.Target == null)
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

        // Rotate the barrel towards target with rotatetowards, for 360 change the transform.forward of the newdirection rotatetowards to barrel.transform.forward
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed, 0.0f);
        turretBarrel.transform.rotation = Quaternion.LookRotation(newDirection);
    }


    //method to shoot forward from the canon using a bulletprefab
    void ShootCanon(GameObject BulletPrefab)
    {
        GameObject bullet = Instantiate(BulletPrefab, shootingSpot.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = turretBarrel.transform.TransformDirection(Vector3.forward * bulletSpeed);

        //(FUTURE PROJECT, MAKE IT SO THAT IT SEES WHERE THE TARGET IS GOING AND SHOOTS ACORDINGLY USING THE TARGET'S RIGIDBODY VELOCITY, aka vector3.right * target.velocity.right * force)
    }

    //method to check if turret can shoot
    void CanTurretShoot()
    {
        //slowly decrease the timer using deltatime
        reloadTimer -= Time.deltaTime;

        //if the timer has reached 0 or below and we have a target, shoot then set timer back to reloadTime
        if  (shipScr.Target != null && reloadTimer <= 0)
        {
            ShootCanon(bullet);
            reloadTimer = reloadTime;
        }

    }
}
