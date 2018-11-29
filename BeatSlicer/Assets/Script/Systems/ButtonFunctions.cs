using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {
    public GameManagerScript gameManagerScript;

    public void TutorialStart()
    {
        SceneManager.LoadScene("Tutorial Level Scene");
    }

	public void StartGame()
    {
        SceneManager.LoadScene("BeatSlicerTestScene");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits Scene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }

    public void ExitToMainMenu()
    {
        gameManagerScript.isPaused = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
