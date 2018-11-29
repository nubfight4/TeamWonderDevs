using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialScript : MonoBehaviour {
    public GameObject TutorialScreen01;
    public GameObject TutorialScreen02;
    public GameObject TutorialScreen03;
    bool Tutorial01isDone = false;
    bool Tutorial02isDone = false;

	void Update () {
        if (!Tutorial01isDone && Input.anyKeyDown)
        {
            TutorialScreen01.SetActive(false);
            TutorialScreen02.SetActive(true);
            Tutorial01isDone = true;
        }

        else if (!Tutorial02isDone && Input.anyKeyDown)
        {
            TutorialScreen02.SetActive(false);
            TutorialScreen03.SetActive(true);
            Tutorial02isDone = true;
        }

        else if (Tutorial01isDone && Tutorial02isDone && Input.anyKeyDown)
        {
            SceneManager.LoadScene("BeatSlicerTestScene");
        }
    }
}
