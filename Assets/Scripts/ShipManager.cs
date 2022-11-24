using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public Transform Target;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
 
    }


    //if the player clicks on the ship, the ship will send a signal to the gamemanager to send it as the selected ship 
    private void OnMouseDown()
    {
        gameManager.selectedShip = this.gameObject;
    }

    //if the player right clicks on the ship, then this ship will be marked as the target of the selected ship, but only if it's marked as an enemy
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (gameManager.selectedShip != null)
            {
                gameManager.selectedShip.GetComponent<ShipManager>().Target = this.gameObject.transform;
            }
        }
    }
}
