using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    //get script from owner ship
    [SerializeField] private ShipScript shipScr;
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameObject Barrel;
    private Transform target;
    public Transform shootingSpot;
    public GameObject Bullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if there is a target, rotate towards it to shoot
        if (shipScr.Target != null)
        {
            target = shipScr.Target;
            RotateTowardsTarget();
        }
        //if there is no target, rotate to 0,0,0
        else if (shipScr.Target == null)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, new Vector3(0, 0, 0), speed, 0.0f);
            Barrel.transform.rotation = Quaternion.LookRotation(newDirection);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            ShootCanon(Bullet);
        }

    }


    //method to rotate tip of turret towards target
    void RotateTowardsTarget()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target.position - transform.position;

        // Rotate the barrel towards target
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed, 0.0f);
        Barrel.transform.rotation = Quaternion.LookRotation(newDirection);
    }


    //method to shoot forward from the canon using a bulletprefab
    void ShootCanon(GameObject BulletPrefab)
    {
        GameObject bullet = Instantiate(BulletPrefab, shootingSpot.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * 10f);
    }
}
