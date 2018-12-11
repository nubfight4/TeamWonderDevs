using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour 
{
	#region Singleton
	private static GameManagerScript mInstance;

	public static GameManagerScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				GameManagerScript temp = ManagerControllerScript.Instance.gameManager;

				if(temp == null)
				{
					temp = Instantiate(ManagerControllerScript.Instance.gameManagerPrefab, Vector3.zero, Quaternion.identity).GetComponent<GameManagerScript>();
				}
				mInstance = temp;
				ManagerControllerScript.Instance.gameManager = mInstance;
				DontDestroyOnLoad(mInstance.gameObject);
			}
			return mInstance;
		}
	}

	public static bool CheckInstanceExist()
	{
		return mInstance;
	}
    #endregion Singleton

    public GameObject PausePanel;
    public GameObject TutorialCanvas;
    public bool isPaused;
    private Scene currentSceneName;

    void Awake()
	{
		if(GameManagerScript.CheckInstanceExist())
		{
			Destroy(this.gameObject);
		}

        //ResumeGame();

    }

    private void Start()
    {
        PausePanel = GameObject.FindGameObjectWithTag("PausePanel");

        if(PausePanel.activeSelf == true)
        {
            PausePanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    void Update()
	{
        currentSceneName = SceneManager.GetActiveScene();

        if(Input.GetButtonDown("Pause"))
        {
            if(currentSceneName.name == "Tutorial Level Scene")
            {
                if(TutorialCanvas.activeSelf != true)
                {
                    if(!isPaused)
                    {
                        PausePanel.SetActive(true);
                        Time.timeScale = 0.0f;
                        isPaused = true;
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }
                    else
                    {
                        ResumeGame();
                    }
                }
            }
            else
            {
                if(!isPaused)
                {
                    PausePanel.SetActive(true);
                    Time.timeScale = 0.0f;
                    isPaused = true;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    ResumeGame();
                }
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        PausePanel.SetActive(false);
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
