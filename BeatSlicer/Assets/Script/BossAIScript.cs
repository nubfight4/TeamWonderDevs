using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ArrayExtensions
{
	public static T RandomItem<T>(this T[] array)
	{
		return array[Random.Range(0, array.Length)];
	}
}
	
public class BossAIScript : MonoBehaviour 
{
	enum AttackPattern
	{
		ATTACK_PATTERN_1 = 0,
		ATTACK_PATTERN_2,
		ATTACK_PATTERN_3,
		SET_ATTACK_PATTERN
	};
		
	public GameObject player;

	private Rigidbody bossRigidbody;
	private Vector3 bossMovementVelocity;
	private float bossMovementDirection = 3.0f;
	private float defaultDirection = 1.0f;

	private float floatIncreaseDecrease = 0.0f;
	private bool isFloatIncrease = true;
	public float floatForce = 90.0f;
	public float floatHeight = 3.5f;

	private string[] allBullets = {"Bullet Red", "Bullet Blue", "Bullet Green"};
	//private string[] shootableBullets = {"Bullet Red", "Bullet Green"};
	//private string[] unshootableBullets = {"Bullet Blue"};

	public Transform bossBulletHardPoint;

	//For Testing
	public Transform bossBulletHardPointM;
	public Transform bossBulletHardPointL;
	public Transform bossBulletHardPointR;

	public float nextFire = 2.0f;
	private float nextFireTemp;

	AttackPattern currentAttackPattern = AttackPattern.SET_ATTACK_PATTERN;


	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		bossRigidbody = GetComponent<Rigidbody>();
		nextFireTemp = nextFire;
	}


	void Start()
	{
		nextFire = nextFireTemp;
		currentAttackPattern = AttackPattern.ATTACK_PATTERN_1;
	}


	void Update()
	{
		bossAIShootingFunction();
		bossMovementFunction();
	}


	void FixedUpdate()
	{
		bossfloatingFunction();
	}


	void bossAIShootingFunction()
	{
		nextFire -= Time.deltaTime;
		transform.LookAt(player.transform);
		Vector3 eulerAngles = transform.rotation.eulerAngles;
		eulerAngles = new Vector3(0, eulerAngles.y, 0);
		transform.rotation = Quaternion.Euler(eulerAngles);

		if(nextFire <= 0)
		{
			nextFire = nextFireTemp;
			setAttackPattern();
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


	void bossfloatingFunction()
	{
		Ray floatRay = new Ray (transform.position, -transform.up);
		RaycastHit floatHit;

		if(Physics.Raycast(floatRay, out floatHit, floatHeight))
		{
			if(isFloatIncrease == true)
			{
				floatIncreaseDecrease += Time.deltaTime;
			}
			else if(isFloatIncrease == false)
			{
				floatIncreaseDecrease -= Time.deltaTime;
			}

			if(floatIncreaseDecrease >= 0.5f)
			{
				isFloatIncrease = false;
			}
			else if(floatIncreaseDecrease <= -0.5f)
			{
				isFloatIncrease = true;
			}

			float propotionalHeight = ((floatHeight - floatHit.distance) / floatHeight) + floatIncreaseDecrease;
			Vector3 appliedHoverForce = Vector3.up * propotionalHeight * floatForce;
			bossRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
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


	void setAttackPattern()
	{
		if(currentAttackPattern == AttackPattern.ATTACK_PATTERN_1) // Set Bullets
		{
			GameObject redBullet = ObjectPooler.Instance.getPooledObject("Bullet Red");

			if(redBullet != null)
			{
				redBullet.transform.position = bossBulletHardPoint.position;
				redBullet.transform.rotation = bossBulletHardPoint.rotation;
				redBullet.SetActive(true);
			}

			GameObject redBullet1 = ObjectPooler.Instance.getPooledObject("Bullet Red");

			if(redBullet1 != null)
			{
				redBullet1.transform.position = bossBulletHardPointR.position;
				redBullet1.transform.rotation = bossBulletHardPointR.rotation;
				redBullet1.SetActive(true);
			}

			GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Bullet Blue");

			if(blueBullet != null)
			{
				blueBullet.transform.position = bossBulletHardPointM.position;
				blueBullet.transform.rotation = bossBulletHardPointM.rotation;
				blueBullet.SetActive(true);
			}

			GameObject greenBullet = ObjectPooler.Instance.getPooledObject("Bullet Green");

			if(blueBullet != null)
			{
				greenBullet.transform.position = bossBulletHardPointL.position;
				greenBullet.transform.rotation = bossBulletHardPointL.rotation;
				greenBullet.SetActive(true);
			}

			currentAttackPattern = AttackPattern.ATTACK_PATTERN_2;
		}
		else if(currentAttackPattern == AttackPattern.ATTACK_PATTERN_2) // Randomized Bullets
		{
			GameObject randBullet = ObjectPooler.Instance.getPooledObject(allBullets.RandomItem());

			if(randBullet != null)
			{
				randBullet.transform.position = bossBulletHardPoint.position;
				randBullet.transform.rotation = bossBulletHardPoint.rotation;
				randBullet.SetActive(true);

				randBullet = null;
			}

			randBullet = ObjectPooler.Instance.getPooledObject(allBullets.RandomItem());

			if(randBullet != null)
			{
				randBullet.transform.position = bossBulletHardPointR.position;
				randBullet.transform.rotation = bossBulletHardPointR.rotation;
				randBullet.SetActive(true);

				randBullet = null;
			}

			randBullet = ObjectPooler.Instance.getPooledObject(allBullets.RandomItem());

			if(randBullet != null)
			{
				randBullet.transform.position = bossBulletHardPointM.position;
				randBullet.transform.rotation = bossBulletHardPointM.rotation;
				randBullet.SetActive(true);

				randBullet = null;
			}

			randBullet = ObjectPooler.Instance.getPooledObject(allBullets.RandomItem());

			if(randBullet != null)
			{
				randBullet.transform.position = bossBulletHardPointL.position;
				randBullet.transform.rotation = bossBulletHardPointL.rotation;
				randBullet.SetActive(true);

				randBullet = null;
			}

			currentAttackPattern = AttackPattern.ATTACK_PATTERN_1;
		}
	}
}
