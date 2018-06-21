using System.Collections;
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

    public bool isFloatingActive = true;
    private bool bossFloatingBool;

    public float movementTimer;
    private float movementTimerTemp;

    private string[] allBullets = {"Bullet Red", "Bullet Blue", "Bullet Green"};
    //private string[] shootableBullets = {"Bullet Red", "Bullet Green"};
    //private string[] unshootableBullets = {"Bullet Blue"};

    public float nextFire = 2.0f;
    private float nextFireTemp;

    public List<GameObject> DestinationPoints;
    private int selectedDestination;
    NavMeshAgent navMeshAgent;

    int[] bossTopMovement = new int[] {0, 1, 2, 6, 7, 8, 9, 15};
    int[] bossOuterRingMovement = new int[] {0, 1, 2, 3, 4, 5, 6, 7};

    AttackPattern currentAttackPattern = AttackPattern.SET_ATTACK_PATTERN;
    MovementPattern currentMovementPattern = MovementPattern.SET_MOVE_PATTERN;


	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		nextFireTemp = nextFire;
        movementTimerTemp = movementTimer;
        navMeshAgent = this.GetComponent<NavMeshAgent>();
    }


	void Start()
	{
		nextFire = nextFireTemp;
		currentAttackPattern = AttackPattern.ATTACK_PATTERN_1;
        currentMovementPattern = MovementPattern.MOVE_PATTERN_1;

        Debug.Log("Current Move Pattern = " + currentMovementPattern);
        Debug.Log("Press 'J' to change Movement Pattern. This message will not repeat.");
    }


	void Update()
	{
		BossAIShootingFunction();
        BossMovementFunction();

        TempMovePatternChangeButton(); // Temporary Movement Pattern Change Button 'J'
    }


    void FixedUpdate()
    {
        if(isFloatingActive == true)
        {
            BossFloatingFunction();
        }
    }


    void BossAIShootingFunction()
	{
		nextFire -= Time.deltaTime;
		transform.LookAt(player.transform);
		Vector3 eulerAngles = transform.rotation.eulerAngles;
		eulerAngles = new Vector3(0, eulerAngles.y, 0);
		transform.rotation = Quaternion.Euler(eulerAngles);

		if(nextFire <= 0)
		{
			nextFire = nextFireTemp;
			SetAttackPattern();
		}
	}
		

	void BossMovementFunction()
	{
        if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
        {
            movementTimerTemp += Time.deltaTime;

            if(movementTimerTemp >= movementTimer)
            {
                selectedDestination = bossTopMovement[Random.Range(0,bossTopMovement.Length)];
                navMeshAgent.SetDestination(DestinationPoints[selectedDestination].transform.position);
                movementTimerTemp = 0;
            }
        }

        if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
        {
            movementTimerTemp += Time.deltaTime;

            if(movementTimerTemp >= movementTimer)
            {
                selectedDestination = Random.Range(0, DestinationPoints.Count);
                navMeshAgent.SetDestination(DestinationPoints[selectedDestination].transform.position);
                movementTimerTemp = 0;
            }
        }

        if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3)
        {
            movementTimerTemp += Time.deltaTime;

            if(movementTimerTemp >= movementTimer)
            {
                if(selectedDestination >= (bossOuterRingMovement.Length - 1))
                {
                    selectedDestination = 0;
                }
                else
                {
                    selectedDestination++;
                }

                navMeshAgent.SetDestination(DestinationPoints[selectedDestination].transform.position);
                movementTimerTemp = 0;
            }
        }
    }


    void BossFloatingFunction()
    {
        if(navMeshAgent.baseOffset >= 1.5f)
        {
            bossFloatingBool = false;
        }
        else if(navMeshAgent.baseOffset <= 0.95f)
        {
            bossFloatingBool = true;
        }

        if(bossFloatingBool == true)
        {
            navMeshAgent.baseOffset += Time.deltaTime * 0.25f;
        }
        else if(bossFloatingBool == false)
        {
            navMeshAgent.baseOffset -= Time.deltaTime * 0.25f;
        }
    }


	void SetAttackPattern()
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


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }


    void TempMovePatternChangeButton()
    {
         if(Input.GetKeyDown(KeyCode.J))
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                currentMovementPattern = MovementPattern.MOVE_PATTERN_2;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
            {
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3)
            {
                currentMovementPattern = MovementPattern.MOVE_PATTERN_1;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
        }
    }
}
