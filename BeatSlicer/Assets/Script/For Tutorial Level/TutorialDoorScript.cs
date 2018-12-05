using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialDoorScript : MonoBehaviour
{
    public SceneTransitionScript sceneTransitionScript;
    void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            sceneTransitionScript.gameObject.SetActive(true);
            sceneTransitionScript.sceneName = "BeatSlicerTestScene";
            sceneTransitionScript.startTransit = true;
        }
    }
}
