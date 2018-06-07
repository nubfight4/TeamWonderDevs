using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


	void Awake()
	{
		if(GameManagerScript.CheckInstanceExist())
		{
			Destroy(this.gameObject);
		}
	}

	void Start()
	{

	}
}
