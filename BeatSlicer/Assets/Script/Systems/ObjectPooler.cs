using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ObjectPoolItem
{
	public GameObject objectToPool;
	public string poolName;
	public int amountToPool;
	public bool shouldExpand = true;
}


public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler Instance;

	public const string defaultRootObjectPoolName = "Pooled Objects";
	public string rootPoolName = defaultRootObjectPoolName;

	public List<GameObject> pooledObjects;
	public List<ObjectPoolItem> itemsToPool;

	void Awake()
	{
		Instance = this;
	}


	private void Start()
	{
		if(string.IsNullOrEmpty(rootPoolName))
		{
			rootPoolName = defaultRootObjectPoolName;
		}
			
		getParentPoolObject(rootPoolName);

		pooledObjects = new List<GameObject>();

		foreach(ObjectPoolItem item in itemsToPool)
		{
			for(int i = 0; i < item.amountToPool; i++)
			{
				createPooledObject(item);
			}
		}
	}


	private GameObject getParentPoolObject(string objectPoolName)
	{
		if(string.IsNullOrEmpty(objectPoolName))
		{
			objectPoolName = rootPoolName;
		}
			
		GameObject parentObject = GameObject.Find(objectPoolName);

		if(parentObject == null)
		{
			parentObject = new GameObject();
			parentObject.name = objectPoolName;

			if(objectPoolName != rootPoolName)
			{
				parentObject.transform.parent = GameObject.Find(rootPoolName).transform;
			}
				
		}

		return parentObject;
	}


	public GameObject getPooledObject(string tag)
	{
		for(int i = 0; i < pooledObjects.Count; i++)
		{
			if(pooledObjects[i].activeInHierarchy == false && pooledObjects[i].CompareTag(tag))
			{
				return pooledObjects[i];
			}
				
		}

		foreach(ObjectPoolItem item in itemsToPool)
		{
    
			{
				if(item.shouldExpand)
				{
					return createPooledObject(item);
				}
			}
		}

		return null;
	}


	private GameObject createPooledObject(ObjectPoolItem item)
	{
		GameObject gameObj = Instantiate<GameObject>(item.objectToPool);

		GameObject parentPoolObject = getParentPoolObject(item.poolName);
		gameObj.transform.parent = parentPoolObject.transform;

		gameObj.SetActive(false);
		pooledObjects.Add(gameObj);

		return gameObj;
	}
}
