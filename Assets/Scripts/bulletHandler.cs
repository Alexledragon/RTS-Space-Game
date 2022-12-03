using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    [Header("Death Timer")]
    [SerializeField] private float deathTime = 1f;
    private float deathTimer;

    [NonSerialized] public string targetTag;
    [SerializeField] private float dmg;

    // Start is called before the first frame update
    void Awake()
    {
        deathTimer = deathTime;
      


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BulletDeathTime();
        if (targetTag == "Player")
        {
            gameObject.layer = 8;
        }
        else
        {
            gameObject.layer = 9;
        }




    }


    void BulletDeathTime()
    {
        //slowly decrease the timer using deltatime
        deathTimer -= Time.deltaTime;

        //if the timer has reached 0 or below, the bullet is destroyed
        if (deathTimer <= 0)
        {
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(targetTag);
        //check if what the bullet collided is an Enemy
        if (collision.gameObject.tag == targetTag)
        {
            //run code here for dealing dmg
            collision.gameObject.GetComponent<ShipHealthManager>().takeDMG(dmg);

            //destroy gameobject after it's dealt dmg
            Destroy(gameObject);
        }
        //if it's not an enemy, simply destroy gameobject
        else
        {
            Destroy(gameObject);
        }
    }
}
