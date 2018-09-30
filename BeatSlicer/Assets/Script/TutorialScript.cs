using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialScript : MonoBehaviour {
    public GameObject TutorialScreen01;
    public GameObject TutorialScreen02;
    bool Tutorial01isDone = false;

	void Update () {
        if (!Tutorial01isDone && Input.anyKeyDown)
        {
            TutorialScreen01.SetActive(false);
            TutorialScreen02.SetActive(true);
            Tutorial01isDone = true;
        }

        else if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("BeatSlicerTestScene");
        }
    }
}
