using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject gameSpawnUi;
public void exitGame()
    {
        Application.Quit();
    }
 public void changeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
public void startGame()
    {
        gameSpawnUi.SetActive(false);
    }

}




