using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenu : MonoBehaviour
{
      public void Level1()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Loading 1");
    }
    /*
    public void Level2()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Loading 2");
    }

    public void Level3()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Loading 3");
    }
    */
    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}