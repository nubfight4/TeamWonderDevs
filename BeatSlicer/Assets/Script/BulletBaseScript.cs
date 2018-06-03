using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBaseScript : MonoBehaviour 
{
	public enum BulletType
	{
		RED_BULLET = 0,
		BLUE_BULLET,
		GREEN_BULLET,
		SET_TYPE
	};

	public GameObject player;

	public float bulletSpeed = 10.0f;
	public float selfDestructTimer = 5.0f;
	private float tempSelfDestructTimer;

	private Rigidbody bulletRigidbody;

	public BulletType bulletType = BulletType.SET_TYPE;
	public bool isToBeDestroyed = false;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		bulletRigidbody = GetComponent<Rigidbody>();
		tempSelfDestructTimer = selfDestructTimer;
	}


	void OnEnable()
	{
		selfDestructTimer = tempSelfDestructTimer;
		bulletRigidbody.velocity = transform.forward * bulletSpeed;
	}


	void Update()
	{
		bulletBaseScriptUpdate();
	}


	public void bulletBaseScriptUpdate()
	{
		selfDestructFunction();

		if(isToBeDestroyed == true)
		{
			gameObject.SetActive(false);
			isToBeDestroyed = false;
		}
	}


	void selfDestructFunction()
	{
		if(gameObject.activeSelf == true)
		{
			selfDestructTimer -= Time.deltaTime;

			if(selfDestructTimer <= 0.0f)
			{
				isToBeDestroyed = true;
			}
		}
	}


	void OnTriggerEnter(Collider other)
	{
		if(gameObject.activeSelf == true)
		{
			if(bulletType != BulletType.BLUE_BULLET) // Test Undestroyable Blue Bullet
			{
				if(other.tag == "Player")
				{
					isToBeDestroyed = true;
				}
			}
		}
	}
}
