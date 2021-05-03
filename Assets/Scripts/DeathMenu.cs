using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenu;

    public static bool GamePaused = false;

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
        Debug.Log("Restart");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("Menu");
    }
    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
