using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {
    public GameManagerScript gameManagerScript;
    public SceneTransitionScript sceneTransitionScript;

    public void TutorialStart()
    {
        sceneTransitionScript.gameObject.SetActive(true);
        sceneTransitionScript.sceneName= "Tutorial Level Scene";
        sceneTransitionScript.startTransit = true;
    }

	public void StartGame()
    {
        sceneTransitionScript.gameObject.SetActive(true);
        sceneTransitionScript.sceneName = "BeatSlicerTestScene";
        sceneTransitionScript.startTransit = true;
    }

    public void Credits()
    {
        sceneTransitionScript.gameObject.SetActive(true);
        sceneTransitionScript.sceneName = "Credits Scene";
        sceneTransitionScript.startTransit = true;
    }

    public void MainMenu()
    {
        sceneTransitionScript.gameObject.SetActive(true);
        sceneTransitionScript.sceneName = "MainMenu";
        sceneTransitionScript.startTransit = true;
    }

    public void ExitToDesktop()
    {
        sceneTransitionScript.gameObject.SetActive(true);
        sceneTransitionScript.sceneName = " ";
        sceneTransitionScript.startTransit = true;
        Application.Quit();
    }

    public void ExitToMainMenu()
    {
        gameManagerScript.isPaused = false;
        Time.timeScale = 1.0f;
        sceneTransitionScript.gameObject.SetActive(true);
        sceneTransitionScript.sceneName = "MainMenu";
        sceneTransitionScript.startTransit = true;
    }
}
