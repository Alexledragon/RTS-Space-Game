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
        if (Target != null)
        {
            Debug.Log(Target.position.x);
            Debug.Log(Target.position.y);
        }
 
    }

    private void OnMouseDown()
    {
        gameManager.selectedShip = this.gameObject;
    }
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
