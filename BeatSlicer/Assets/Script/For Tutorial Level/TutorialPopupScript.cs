using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopupScript : MonoBehaviour
{
    private GameObject player;
    public GameObject tutorialCanvas;
    public GameObject Image1;
    public GameObject Image2;
    public GameObject Image3; 

    private bool tutorialPause;
    private bool hasBeenTriggered = false;

    public bool sectionOne;
    public bool sectionTwo;
    public bool sectionThree;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Start()
    {
        tutorialCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        tutorialPause = false;
    }
	

	void Update()
    {
        if(tutorialPause == true && hasBeenTriggered == false)
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                ResumeGame();
                tutorialPause = false;
                hasBeenTriggered = true;
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        tutorialCanvas.SetActive(false);
        tutorialPause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TutorialPause()
    {
        tutorialCanvas.SetActive(true);

        if(sectionOne == true)
        {
            Image1.SetActive(true);
        }
        else if(sectionTwo == true)
        {
            Image2.SetActive(true);
        }
        else if(sectionThree == true)
        {
            Image3.SetActive(true);
        }


        Time.timeScale = 0.0f;
        tutorialPause = true;
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }


    void OnTriggerEnter(Collider other)
    {
        if(hasBeenTriggered != true)
        {
            if(other.tag == "Player")
            {
                TutorialPause();
                tutorialPause = true;
            }  
        }
    }
}
