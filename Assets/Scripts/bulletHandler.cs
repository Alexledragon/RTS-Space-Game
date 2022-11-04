using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletHandler : MonoBehaviour
{
    [Header("Death Timer")]
    [SerializeField] private float deathTime = 1f;
    private float deathTimer;

    // Start is called before the first frame update
    void Awake()
    {
        deathTimer = deathTime;
    }

    // Update is called once per frame
    void Update()
    {
        BulletDeathTime();
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
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.transform.localScale += new Vector3(+1, +1, +1);
        }
        Destroy(gameObject);
    }
}
