using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
   public GameObject pauseMenu;

    public static bool GamePaused = false;
   

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                ResumeGame();
                Debug.Log("ESCAPE 1");
            }
            else
            {
                PauseGame();
                Debug.Log("ESCAPE 2");
            }
         }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        Debug.Log("GAME PAUSE");
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        Debug.Log("GAME RESUME");
    }
}
