using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIScript : MonoBehaviour 
{
	public GameObject player;

	private Rigidbody bossRigidbody;
	private Vector3 bossMovementVelocity;
	private float bossMovementDirection = 3.0f;
	private float defaultDirection = 1.0f;

	public GameObject bullet;
	#pragma warning disable 0414
	private GameObject newBullet;
	#pragma warning restore 0414
	public Transform bossBulletHardPoint;

	public float nextFire = 2.0f;
	private float nextFireTemp;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		bossRigidbody = GetComponent<Rigidbody>();
	}


	void Start()
	{
		nextFireTemp = nextFire;
	}


	void Update()
	{
		bossAIShootingFunction();

		if(defaultDirection < bossMovementDirection)
		{
			defaultDirection += 1.0f * Time.deltaTime;
		}
		else
		{
			bossMovementFunction();

			if(Random.value > 0.5f)
			{
				bossMovementDirection += Random.value;
			}
			else
			{
				bossMovementDirection -= Random.value;
			}

			if(Random.value < 1.0f)
			{
				bossMovementDirection = 1 + Random.value;
			}

			defaultDirection = 0.0f;
		}

		bossRigidbody.velocity = bossMovementVelocity;
	}


	void bossAIShootingFunction()
	{
		nextFireTemp -= Time.deltaTime;
		transform.LookAt(player.transform);

		if(nextFireTemp <= 0)
		{
			nextFireTemp = nextFire;
			newBullet = Instantiate(bullet, bossBulletHardPoint.position, bossBulletHardPoint.rotation);
			newBullet = null;
		}
	}


	void bossMovementFunction()
	{
		if(Random.value > 0.5f)
		{
			bossMovementVelocity.x = 2.0f * Random.value;
		}
		else
		{
			bossMovementVelocity.x = -2.0f * Random.value;
		}

		if(Random.value > 0.5f)
		{
			bossMovementVelocity.z = 2.0f * Random.value;
		}
		else
		{
			bossMovementVelocity.z = -2.0f * Random.value;
		}
	}


	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Boundary")
		{
			bossMovementVelocity = -bossMovementVelocity.normalized;
			bossMovementVelocity = bossMovementVelocity * 2.0f;
		}
	}
}
