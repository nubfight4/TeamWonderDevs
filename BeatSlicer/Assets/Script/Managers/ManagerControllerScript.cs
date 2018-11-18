using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerControllerScript : MonoBehaviour 
{
	#region Singleton
	private static ManagerControllerScript mInstance = null;

	public static ManagerControllerScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag("ManagerController");

				if(tempObject == null)
				{
					Debug.Break();
				}
				else
				{
					mInstance = tempObject.GetComponent<ManagerControllerScript>();
				}
			}
			return mInstance;
		}
	}
	public static bool CheckInstanceExist()
	{
		return mInstance;
	}
	#endregion Singleton

	[Header("Settings")]
	public GameObject gameManagerPrefab;
	public GameObject soundManagerPrefab;
    public GameObject objectPoolerPrefab;
    public GameObject rhythmManagerPrefab;

	[Header("In-Scene Managers")]
	public SoundManagerScript soundManager;
	public GameManagerScript gameManager;
    public ObjectPooler objectPooler;
    public RhythmManager rhythmManager;
}
