using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void SceneChange(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application Exited");
    }

    public void StartNewGame()
    {
        //Start New Game
        Debug.Log("New Game Started");
    }
}
