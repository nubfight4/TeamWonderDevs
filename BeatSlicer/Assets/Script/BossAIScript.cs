﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField]

    //Transform destination;
    NavMeshAgent navMeshAgent;

    public float wanderRadius;
    public float wanderTimer;
    private float wanderTimerTemp;

    enum MovementPattern
    {
        MOVE_PATTERN_1 = 0,
        MOVE_PATTERN_2,
        MOVE_PATTERN_3,
        SET_MOVE_PATTERN
    };

    enum AttackPattern
	{
		ATTACK_PATTERN_1 = 0,
		ATTACK_PATTERN_2,
		ATTACK_PATTERN_3,
		SET_ATTACK_PATTERN
	};
		
	public GameObject player;

	private Rigidbody bossRigidbody;

	private string[] allBullets = {"Bullet Red", "Bullet Blue", "Bullet Green"};
	//private string[] shootableBullets = {"Bullet Red", "Bullet Green"};
	//private string[] unshootableBullets = {"Bullet Blue"};

	public float nextFire = 2.0f;
	private float nextFireTemp;

	AttackPattern currentAttackPattern = AttackPattern.SET_ATTACK_PATTERN;
    MovementPattern currentMovementPattern = MovementPattern.SET_MOVE_PATTERN;


	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		bossRigidbody = GetComponent<Rigidbody>();
		nextFireTemp = nextFire;
        wanderTimerTemp = wanderTimer;
        navMeshAgent = this.GetComponent<NavMeshAgent>();
    }


	void Start()
	{
		nextFire = nextFireTemp;
		currentAttackPattern = AttackPattern.ATTACK_PATTERN_1;
        currentMovementPattern = MovementPattern.MOVE_PATTERN_1;

    }


	void Update()
	{
		bossAIShootingFunction();
        bossMovementFunction();
        //SetDestination();
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
        if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
        {
            wanderTimerTemp += Time.deltaTime;

            if (wanderTimerTemp >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                navMeshAgent.SetDestination(newPos);
                wanderTimerTemp = 0;
            }
        }
    }

    /*
    void SetDestination()
    {
        if(destination != null)
        {
            Vector3 targetVector = destination.transform.position;
            navMeshAgent.SetDestination(targetVector);
        }
    }
    */


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }


    /*
	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Boundary")
		{
			bossMovementVelocity = -bossMovementVelocity.normalized;
			bossMovementVelocity = bossMovementVelocity * 2.0f;
		}
	}
    */


	void setAttackPattern()
	{
		if(currentAttackPattern == AttackPattern.ATTACK_PATTERN_1) // Set Bullets
		{
			GameObject redBullet = ObjectPooler.Instance.getPooledObject("Bullet Red");

			if(redBullet != null)
			{
				redBullet.transform.position = transform.position + (transform.forward * 2) + (transform.up * -2);
				redBullet.transform.rotation = transform.rotation;
				redBullet.SetActive(true);
			}

			GameObject redBullet1 = ObjectPooler.Instance.getPooledObject("Bullet Red");

			if(redBullet1 != null)
			{
				redBullet1.transform.position = transform.position + (transform.forward * 2) + (transform.right * 2) + (transform.up * -2);
				redBullet1.transform.rotation = transform.rotation;
				redBullet1.SetActive(true);
			}

			GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Bullet Blue");

			if(blueBullet != null)
			{
				blueBullet.transform.position = transform.position + (transform.forward * 2) + (transform.up * 2);
				blueBullet.transform.rotation = transform.rotation;
				blueBullet.SetActive(true);
			}

			GameObject greenBullet = ObjectPooler.Instance.getPooledObject("Bullet Green");

			if(blueBullet != null)
			{
				greenBullet.transform.position = transform.position + (transform.forward * 2) + (transform.right * -2) + (transform.up * -2);
				greenBullet.transform.rotation = transform.rotation;
				greenBullet.SetActive(true);
			}

			currentAttackPattern = AttackPattern.ATTACK_PATTERN_2;
		}
		else if(currentAttackPattern == AttackPattern.ATTACK_PATTERN_2) // Randomized Bullets
		{
			GameObject randBullet = ObjectPooler.Instance.getPooledObject(allBullets.RandomItem());

			if(randBullet != null)
			{
				randBullet.transform.position = transform.position + (transform.forward * 2) + (transform.up * -2);
				randBullet.transform.rotation = transform.rotation;
				randBullet.SetActive(true);

				randBullet = null;
			}

			randBullet = ObjectPooler.Instance.getPooledObject(allBullets.RandomItem());

			if(randBullet != null)
			{
				randBullet.transform.position = transform.position + (transform.forward * 2) + (transform.right * 2) + (transform.up * -2);
				randBullet.transform.rotation = transform.rotation;
				randBullet.SetActive(true);

				randBullet = null;
			}

			randBullet = ObjectPooler.Instance.getPooledObject(allBullets.RandomItem());

			if(randBullet != null)
			{
				randBullet.transform.position = transform.position + (transform.forward * 2) + (transform.up * 2);
				randBullet.transform.rotation = transform.rotation;
				randBullet.SetActive(true);

				randBullet = null;
			}

			randBullet = ObjectPooler.Instance.getPooledObject(allBullets.RandomItem());

			if(randBullet != null)
			{
				randBullet.transform.position = transform.position + (transform.forward * 2) + (transform.right * -2) + (transform.up * -2);
				randBullet.transform.rotation = transform.rotation;
				randBullet.SetActive(true);

				randBullet = null;
			}

			currentAttackPattern = AttackPattern.ATTACK_PATTERN_1;
		}
	}
}
