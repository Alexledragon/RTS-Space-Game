using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHealthManager : MonoBehaviour
{

    [SerializeField] private float shipHP = 100;

    [Header("Shield")]
    [SerializeField] private float shipShieldMax = 100f;
    private float currentshipShields;
    [SerializeField] private int shipShieldRechargeSpeed = 10;

    [Header("recharge Timer")]
    [SerializeField] private float rechargeTime = 5f;
    private float rechargeTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentshipShields = shipShieldMax;
    }

    // Update is called once per frame
    void Update()
    {
        ShieldRecharge();
    }

    //method to recharge the shields upon damage that has been taken
    void ShieldRecharge()
    {
        //slowly decrease the timer using deltatime
        rechargeTimer -= Time.deltaTime;

        //if the timer has reached 0 or below, the shield starts recharging
        if (rechargeTimer <= 0)
        {
            currentshipShields = Mathf.MoveTowards(currentshipShields, shipShieldMax, shipShieldRechargeSpeed * Time.deltaTime);
        }
    }

    //method to take damage
     public void takeDMG(float dmg)
    {
        //make an int to store the dmg 
        float tempDmgDataStorage = dmg;

        //first we see how much dmg the shield can take before breaking and cancel the recharge 
        tempDmgDataStorage -= currentshipShields;
        currentshipShields -= dmg;
        rechargeTimer = rechargeTime;

        //we then check if the shield has reached 0 or below zero, if it did we set it to 0, that way we won't go into negative levels
        if (currentshipShields <= 0)
        {
            currentshipShields = 0;
        }

        //we set the dmg to tempdmgdatastorage incase the shield couldn't take all the dmg
        dmg = tempDmgDataStorage;

        //then if there is some dmg that still remains, we put it on the ship's hp
        if (dmg > 0)
        {
            shipHP -= dmg;
        }
        if (shipHP <= 0)
        {
            Destroy(gameObject);
        }

    }

}
