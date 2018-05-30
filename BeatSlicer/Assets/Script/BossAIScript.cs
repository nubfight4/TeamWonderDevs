using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIScript : MonoBehaviour 
{
	enum attackPattern
	{
		attackPattern1,
		attackPattern2,
		attackPattern3,
		standby
	};
		
	public GameObject player;

	private Rigidbody bossRigidbody;
	private Vector3 bossMovementVelocity;
	private float bossMovementDirection = 3.0f;
	private float defaultDirection = 1.0f;

	public GameObject bulletRed;
	public GameObject bulletGreen;
	public GameObject bulletBlue;

	#pragma warning disable 0414 // Disable Warning
	private GameObject newBullet;
	#pragma warning restore 0414 // Reenable Warning

	public Transform bossBulletHardPoint;

	//For Testing
	public Transform bossBulletHardPointM;
	public Transform bossBulletHardPointL;
	public Transform bossBulletHardPointR;

	public float nextFire = 2.0f;
	private float nextFireTemp;
	attackPattern currentAttackPattern = attackPattern.standby;
	//attackPattern previousAttackPattern = attackPattern.standby; // To Be Used Eventually


	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		bossRigidbody = GetComponent<Rigidbody>();
	}


	void Start()
	{
		nextFireTemp = nextFire;
		currentAttackPattern = attackPattern.attackPattern1;
	}


	void Update()
	{
		bossAIShootingFunction();
		bossMovementFunction();
	}


	void bossAIShootingFunction()
	{
		nextFireTemp -= Time.deltaTime;
		transform.LookAt(player.transform);
		Vector3 eulerAngles = transform.rotation.eulerAngles;
		eulerAngles = new Vector3(0, eulerAngles.y, 0);
		transform.rotation = Quaternion.Euler(eulerAngles);

		if(nextFireTemp <= 0)
		{
			nextFireTemp = nextFire;
			setAttackPattern();
			newBullet = null;
		}
	}


	void setAttackPattern()
	{
		if(currentAttackPattern == attackPattern.attackPattern1)
		{
			newBullet = Instantiate(bulletRed, bossBulletHardPoint.position, bossBulletHardPoint.rotation);
			newBullet = Instantiate(bulletRed, bossBulletHardPointM.position, bossBulletHardPointM.rotation);
			newBullet = Instantiate(bulletRed, bossBulletHardPointL.position, bossBulletHardPointL.rotation);
			newBullet = Instantiate(bulletRed, bossBulletHardPointR.position, bossBulletHardPointR.rotation);

			currentAttackPattern = attackPattern.attackPattern2;
			//previousAttackPattern = attackPattern.attackPattern1;
		}
		else if(currentAttackPattern == attackPattern.attackPattern2)
		{
			newBullet = Instantiate(bulletGreen, bossBulletHardPoint.position, bossBulletHardPoint.rotation);
			newBullet = Instantiate(bulletGreen, bossBulletHardPointM.position, bossBulletHardPointM.rotation);
			newBullet = Instantiate(bulletGreen, bossBulletHardPointL.position, bossBulletHardPointL.rotation);
			newBullet = Instantiate(bulletGreen, bossBulletHardPointR.position, bossBulletHardPointR.rotation);

			currentAttackPattern = attackPattern.attackPattern3;
			//previousAttackPattern = attackPattern.attackPattern2;
		}
		else if(currentAttackPattern == attackPattern.attackPattern3)
		{
			newBullet = Instantiate(bulletBlue, bossBulletHardPoint.position, bossBulletHardPoint.rotation);
			newBullet = Instantiate(bulletBlue, bossBulletHardPointM.position, bossBulletHardPointM.rotation);
			newBullet = Instantiate(bulletBlue, bossBulletHardPointL.position, bossBulletHardPointL.rotation);
			newBullet = Instantiate(bulletBlue, bossBulletHardPointR.position, bossBulletHardPointR.rotation);

			currentAttackPattern = attackPattern.attackPattern1;
			//previousAttackPattern = attackPattern.attackPattern3;
		}
	}


	void bossMovementFunction()
	{
		if(defaultDirection < bossMovementDirection)
		{
			defaultDirection += 1.0f * Time.deltaTime;
		}
		else
		{
			//For Velocity Changes
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

			//For Direction Changes
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


	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Boundary")
		{
			bossMovementVelocity = -bossMovementVelocity.normalized;
			bossMovementVelocity = bossMovementVelocity * 2.0f;
		}
	}
}
