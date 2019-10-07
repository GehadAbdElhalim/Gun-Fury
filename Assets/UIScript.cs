using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject CrossHair;
    public GameObject GameOverScreen;
    public static bool GameIsPaused = false;
    public static bool PlayerDead = false;

    private void Start()
    {
        GameIsPaused = false;
        PlayerDead = false;
        GameOverScreen.SetActive(false);
        PauseMenu.SetActive(false);
        CrossHair.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (PlayerDead)
        {
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        CrossHair.SetActive(false);
        Cursor.visible = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Resume()
    {
        CrossHair.SetActive(true);
        GameOverScreen.SetActive(false);
        Cursor.visible = false;
        GameIsPaused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        GameOverScreen.SetActive(true);
        CrossHair.SetActive(false);
        Cursor.visible = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
