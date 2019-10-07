using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Instructions;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
    }

    public void StartGame()
    {
        Instructions.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void Continue()
    {
        SceneManager.LoadScene(1);
    }

    public void Back()
    {
        Instructions.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
