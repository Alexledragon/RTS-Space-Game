using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private float scaleTime;
    // Start is called before the first frame update
    void Start()
    {
        scaleTime = 1;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuOn();
        }

    }

    public void setGameTimeSpeed(float scale)
    {
        scaleTime = scale;
        Time.timeScale = scale;
    }

    public void pauseMenuOn()
    {
        if (pauseMenu.activeInHierarchy == true)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = scaleTime;

        }

        else if (pauseMenu.activeInHierarchy == false)
        {

            Debug.Log("owo");
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
