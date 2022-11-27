using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    public Transform Target;
    private GameManager gameManager;
    [SerializeField] GameObject selectionSpherePrefab;
    [SerializeField] int shipSize = 1;
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
        if (gameObject.tag == "Player")
        {
            gameManager.selectedShip = this.gameObject;
            gameManager.addSelectedShipSphere(selectionSpherePrefab, this.gameObject.transform, shipSize, false);
           
        }
    }

    //if the player right clicks on the ship, then this ship will be marked as the target of the selected ship, but only if it's marked as an enemy
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && gameObject.tag == "Enemy")
        {
            if (gameManager.selectedShip != null)
            {
                gameManager.selectedShip.GetComponent<ShipManager>().Target = this.gameObject.transform;
                gameManager.addSelectedShipSphere(selectionSpherePrefab, this.gameObject.transform, shipSize, true);
                gameManager.selectedShip = null;
            }
        }
    }
}
