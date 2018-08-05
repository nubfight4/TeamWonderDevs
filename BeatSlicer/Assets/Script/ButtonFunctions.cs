using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {

	public void StartGame()
    {
        SceneManager.LoadScene("BeatSlicerTestScene");
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
