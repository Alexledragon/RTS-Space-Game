using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject selectedShip;
    public GameObject selectedTarget;
    private GameObject selectionSphere;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void addSelectedShipSphere(GameObject sphere, Transform shipTransform, int shipSize, bool isEnemy)
    {
        Destroy(selectionSphere);
        selectionSphere = Instantiate(sphere);
        selectionSphere.transform.parent = shipTransform;
        if (isEnemy == true)
        {
            selectionSphere.transform.localScale = new Vector3(5 * shipSize, 5 * shipSize, 5 * shipSize);
        }
        else
        {
            selectionSphere.transform.localScale = new Vector3(1 * shipSize, 1 * shipSize, 1 * shipSize);
        }

        selectionSphere.transform.localPosition = new Vector3(0, 5 * shipSize, 0);

    }
}
