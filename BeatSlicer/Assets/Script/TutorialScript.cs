using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialScript : MonoBehaviour {
    public GameObject TutorialScreen01;
    public GameObject TutorialScreen02;
    bool Tutorial01isDone = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Tutorial01isDone && Input.anyKey)
        {
            SceneManager.LoadScene("BeatSlicerTestScene");
        }
        else if (!Tutorial01isDone && Input.anyKey)
        {
            TutorialScreen01.SetActive(false);
            TutorialScreen02.SetActive(true);
            Tutorial01isDone = true;
        }
	}
}
