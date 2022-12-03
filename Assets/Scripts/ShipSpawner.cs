using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] bool spawnEnemies;
    [SerializeField] int rowSize;
    [SerializeField] float zIncrementPerStep;
    [SerializeField] float xIncrementPerStep;

    Vector3 referencePoint;
    int currentRowSize;
    float currentZIncrement;
    float currentXIncrement;

    string spawnedShipsTag;


    public void SpawnShip(GameObject ship)
    {
        Quaternion rotationIncrement;
        if (spawnEnemies)
        {
            rotationIncrement = Quaternion.Euler(new Vector3(0f, 270f, 0f));
        }
        else
        {
            rotationIncrement = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        }

        Instantiate(ship, transform.position, transform.rotation * rotationIncrement);
        MoveToNext();
    }

    public void ClearAll()
    {
        GameObject[] shipsDetected = GameObject.FindGameObjectsWithTag(spawnedShipsTag);
        foreach(GameObject ship in shipsDetected)
        {
            GameObject.Destroy(ship);
        }
        currentRowSize = 0;
        currentZIncrement = 0f;
        currentXIncrement = 0f;
    }

    private void Start()
    {
        referencePoint = transform.position;
        currentRowSize = 0;
        currentZIncrement = 0f;
        currentXIncrement = 0f;

        if (!spawnEnemies)
        {
            xIncrementPerStep = -xIncrementPerStep;
            spawnedShipsTag = "Player";
        }
        else
        {
            spawnedShipsTag = "Enemy";
        }
    }

    void MoveToNext()
    {
        if (currentRowSize > rowSize)
        {
            currentZIncrement = 0f;
            currentXIncrement += xIncrementPerStep;
            currentRowSize = 0;
        }
        else
        {
            currentZIncrement += zIncrementPerStep;
            currentRowSize++;
        }

        transform.position = referencePoint + (Vector3.right * currentXIncrement) + (Vector3.forward * currentZIncrement);
    }
}
