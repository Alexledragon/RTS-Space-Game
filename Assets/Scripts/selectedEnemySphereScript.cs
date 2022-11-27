using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedEnemySphereScript : MonoBehaviour
{
    private Material renderedMaterial;
    private bool switchRender = true;
    private float materialNumber = 0.5f;
    [SerializeField] private float deathTime = 1f;
    private float deathTimer;
    // Start is called before the first frame update
    private void Awake()
    {
        renderedMaterial = GetComponent<Renderer>().material;
        deathTimer = deathTime;
        
    }

    private void Update()
    {
        flashMaterial();
        selfDeathTime();
    }

    void flashMaterial()
    {
        if (switchRender == true)
        {
            materialNumber = Mathf.MoveTowards(materialNumber, 0f, Time.deltaTime * 2);
            if (materialNumber == 0)
            {
                switchRender = false;
            }
        }
        else if (switchRender == false)
        {
            materialNumber = Mathf.MoveTowards(materialNumber, 0.7f, Time.deltaTime * 2);
            if (materialNumber == 0.7f)
            {
                switchRender = true;
            }

        }
        renderedMaterial.SetFloat("_Cutoff", materialNumber);
    }

    void selfDeathTime()
    {
        //slowly decrease the timer using deltatime
        deathTimer -= Time.deltaTime;

        //if the timer has reached 0 or below, the bullet is destroyed
        if (deathTimer <= 0)
        {
            Debug.Log("uuw");
            Destroy(gameObject);
        }

    }
}
