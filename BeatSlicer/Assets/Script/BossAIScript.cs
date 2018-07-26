using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAIScript : MonoBehaviour
{
    [SerializeField]

    public enum MovementPattern
    {
        MOVE_PATTERN_1 = 0,
        MOVE_PATTERN_2,
        MOVE_PATTERN_3A,
        MOVE_PATTERN_3B,
        BOSS_STUN,
        SET_MOVE_PATTERN
    };

    private GameObject player;

    private bool isVanishingAndReappearing = false;
    private bool isFlyingUp = false;

    private bool bossFloatingBool;

    public float movementTimer;
    private float movementTimerTemp;

    private bool isMoving = false;

    private bool hasSetBulletPattens = true;
    private bool isOutside = false;

    private float tempNum = 0.0f;
    private bool tempTimerHasStarted = false;
    private bool hasShot = false;

    public List<GameObject> StageOnePoints;
    public List<GameObject> StageTwoPoints;

    private int selectedDestination;
    private int previousDestination;
    NavMeshAgent navMeshAgent;

    private readonly int[] bossStageOneMovement = new int[] {0, 1, 4};
    private readonly int[] bossStageTwoMovementStart = new int[] {0, 2, 4, 6, 7, 8};
    private readonly int[] bossStageTwoMovementEnd = new int[] {1, 3, 5, 7, 6, 8};
    private readonly int[] bossStageThreeMovement = new int[] {4, 2, 3, 1, 5};

    public MovementPattern currentMovementPattern = MovementPattern.SET_MOVE_PATTERN;
    MovementPattern previousMovementPattern = MovementPattern.SET_MOVE_PATTERN;


	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
        movementTimerTemp = movementTimer;
        previousDestination = 99; // Initializing with a number that is not 0
        navMeshAgent = this.GetComponent<NavMeshAgent>();
    }


	void Start()
	{
        currentMovementPattern = MovementPattern.MOVE_PATTERN_1;
        previousMovementPattern = currentMovementPattern;
        BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.TURNING_LEFT;

        navMeshAgent.baseOffset = 2.0f; // Move Boss to starting floating height

        StartCoroutine(TempMovePatternTimer());

        Debug.Log("Current Move Pattern = " + currentMovementPattern);
    }


	void Update()
	{
        LookAtPlayerFunction();
        BossMovementFunction();
        BulletPatternSetterFunction();

        TempMultiTimerFunction();
        TempMovePatternChangeButton(); // Temporary Movement Pattern Change Button 'J'
        TempBossStunnerButton(); // Temporary Boss Stunner (& Unstunner) Button 'K'
    }


    void FixedUpdate()
    {
        BossFloatingFunction();
    }


    void LookAtPlayerFunction()
    {
        if(currentMovementPattern != MovementPattern.BOSS_STUN)
        {
            transform.LookAt(player.transform);
        }

        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles = new Vector3(0,eulerAngles.y,0);
        transform.rotation = Quaternion.Euler(eulerAngles);
    }

	
	void BossMovementFunction()
	{
        if(currentMovementPattern == MovementPattern.BOSS_STUN || currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
        {
            navMeshAgent.speed = 0;
        }
        else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
        {
            if(selectedDestination == 5)
            {
                navMeshAgent.speed = 0;

                if(isFlyingUp != true && currentMovementPattern == MovementPattern.MOVE_PATTERN_2) // If MOVE_PATTERN_2, this will repeat the cycle
                {
                    movementTimerTemp -= Time.deltaTime;

                    if(movementTimerTemp <= 0)
                    {
                        movementTimerTemp = movementTimer;

                        isFlyingUp = true;
                    }
                }
            }
            else if(isMoving != true && isFlyingUp != true && isVanishingAndReappearing != true)
            {
                transform.position = StageTwoPoints[bossStageTwoMovementStart[selectedDestination]].transform.position;

                isMoving = true;
            }
            else if(isMoving == true)
            {
                navMeshAgent.speed = 8;

                navMeshAgent.SetDestination(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position);

                float distance = Vector3.Distance(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position,transform.position);

                if(distance <= 3.0f)
                {
                    selectedDestination = selectedDestination + 1;

                    if(selectedDestination == 5)
                    {
                        transform.position = StageTwoPoints[bossStageTwoMovementStart[selectedDestination]].transform.position;
                        navMeshAgent.SetDestination(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position);
                    }

                    isMoving = false;
                }
            }
        }
    }


    void BossFloatingFunction()
    {
        if(isVanishingAndReappearing == true)
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                if(navMeshAgent.baseOffset <= 1.7f || navMeshAgent.baseOffset >= 2.2f)
                {
                    navMeshAgent.baseOffset = 2.0f;
                }

                transform.position = StageOnePoints[selectedDestination].transform.position;
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                if(navMeshAgent.baseOffset <= 3.7f || navMeshAgent.baseOffset >= 4.2f)
                {
                    navMeshAgent.baseOffset = 4.0f;
                }

                selectedDestination = 0;
                
                transform.position = StageTwoPoints[selectedDestination].transform.position;
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                if(navMeshAgent.baseOffset <= 1.7f || navMeshAgent.baseOffset >= 2.2f)
                {
                    navMeshAgent.baseOffset = 2.0f;
                }

                transform.position = StageOnePoints[bossStageThreeMovement[selectedDestination]].transform.position;
            }

            isVanishingAndReappearing = false;
        }
        else if(isFlyingUp ==  true)
        {
            navMeshAgent.baseOffset += Time.deltaTime * 2.0f;

            if(navMeshAgent.baseOffset >= 11.0f && selectedDestination == 5)
            {
                navMeshAgent.baseOffset = 4.0f;
                selectedDestination = 0;

                transform.position = StageTwoPoints[selectedDestination].transform.position;

                hasSetBulletPattens = false; // For setting Bullet Pattern
                isFlyingUp = false;
            }
        }
        else // This is for managing the floating distance from the ground
        {
            if(currentMovementPattern == MovementPattern.BOSS_STUN)
            {
                if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                {
                    if(this.transform.position.x <= -13 || this.transform.position.x >= 13 || this.transform.position.z <= -13 || this.transform.position.z >= 13) // Known bug, does not actually work yet
                    {
                        isOutside = true;
                    }

                    if(isOutside == true)
                    {
                        transform.position = StageTwoPoints[bossStageTwoMovementStart[5]].transform.position;

                        isOutside = false;
                    }
                }

                if(navMeshAgent.baseOffset > 1.0f)
                {
                    navMeshAgent.baseOffset -= Time.deltaTime * 5.0f; // Time for Boss to fall when stunned set to Time.deltaTime * 5.0f;
                }
                else if(navMeshAgent.baseOffset <= 0.9f)
                {
                    navMeshAgent.baseOffset = 0.9f;
                }
            }
            else
            {
                if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
                {
                    if(navMeshAgent.baseOffset >= 2.2f)
                    {
                        bossFloatingBool = false;
                    }
                    else if(navMeshAgent.baseOffset <= 1.7f)
                    {
                        bossFloatingBool = true;
                    }
                }
                else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                {
                    if(navMeshAgent.baseOffset >= 4.2f)
                    {
                        bossFloatingBool = false;
                    }
                    else if(navMeshAgent.baseOffset <= 3.7f)
                    {
                        bossFloatingBool = true;
                    }
                }

                if(bossFloatingBool == true)
                {
                    if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                    {
                        if(navMeshAgent.baseOffset < 3.7f)
                        {
                            navMeshAgent.baseOffset += Time.deltaTime * 2.0f; // Speed up floating by 2.0f if Boss previous state was BOSS_STUN
                        }
                        else
                        {
                            navMeshAgent.baseOffset += Time.deltaTime * 0.25f;
                        }
                    }
                    else
                    {
                        navMeshAgent.baseOffset += Time.deltaTime * 0.25f;
                    }
                }
                else if(bossFloatingBool == false)
                {
                    navMeshAgent.baseOffset -= Time.deltaTime * 0.25f;
                }
            }
        }
    }


    void BulletPatternSetterFunction()
    {
        if(currentMovementPattern == MovementPattern.BOSS_STUN)
        {
            BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;
        }
        else
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                if(this.transform.position.x <= -13 || this.transform.position.x >= 13 || this.transform.position.z <= -13 || this.transform.position.z >= 13) // Might change to collider instead
                {
                    BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                    isOutside = true;
                }
                else
                {
                    hasSetBulletPattens = false;
                    isOutside = false;
                }
            }

            if(BossShootingScript.Instance.currentBulletPattern == BossShootingScript.BulletPatternType.REST || BossShootingScript.Instance.currentBulletPattern == BossShootingScript.BulletPatternType.BOMBING_RUN)
            {
                if(isFlyingUp == true || isOutside == true)
                {
                    BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;
                }
                else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
                {
                    hasSetBulletPattens = false;
                }
            }

            if(hasSetBulletPattens == false)
            {
                if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
                {
                    BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.TURNING_LEFT;
                }
                else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                {
                    if(selectedDestination == 5)
                    {
                        if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                        {
                            if(hasShot == false)
                            {
                                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.CONE_SHOT;

                                tempTimerHasStarted = true; // Temporary Timer to re-enable Cone Shot after 3.5 seconds
                            }       
                        }
                        else
                        {
                            if(hasShot == false)
                            {
                                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.CIRCLE_RAIN;

                                tempTimerHasStarted = true; // Temporary Timer to re-enable Circle Rain after 3.5 seconds
                            }
                        }
                    }
                    else
                    {
                        BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.BOMBING_RUN;
                    }
                }

                hasSetBulletPattens = true;
            }
        } 
    }


    /// Temporary Functions Below


    void TempMultiTimerFunction() // To re-enable hasShot boolean
    {
        if(tempTimerHasStarted == true || hasShot == true)
        {
            tempNum += Time.deltaTime;

            if(tempNum >= 3.5f && hasShot == true && (currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B))
            {
                tempNum = 0;

                hasShot = false;
                tempTimerHasStarted = false;
            }
            else if(tempNum >= 3.5f && hasShot == false && (currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B))
            {
                tempNum = 0;

                hasShot = true;
                tempTimerHasStarted = false;
            }
            else if(tempNum >= 5.0f && currentMovementPattern == MovementPattern.BOSS_STUN) // 5 seconds of the BOSS_STUN state before resuming previous state
            {
                tempNum = 0;

                currentMovementPattern = previousMovementPattern;
                tempTimerHasStarted = false;
            }
        }
    }


    void TempMovePatternChangeButton() // Press 'J' To Change Between Movement Patterns // Button combination will change in later builds
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_2;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                previousDestination = 99; // Reset to number other than 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3A;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                selectedDestination = 0; // Reset to 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_1;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                previousDestination = 99; // Reset to number other than 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
        }
    }


    void TempBossStunnerButton() // Press 'K' To Stun (& Unstun) The Boss // To be removed in later builds
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            if(currentMovementPattern != MovementPattern.BOSS_STUN)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.BOSS_STUN;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else
            {
                currentMovementPattern = previousMovementPattern;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
        }
    }


    private IEnumerator TempMovePatternTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(5,11));

            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                do
                {
                    selectedDestination = bossStageOneMovement[Random.Range(0,bossStageOneMovement.Length)];

                } while(selectedDestination == previousDestination); // Will repeat randomization until selectedDestination != previousDestination

                navMeshAgent.SetDestination(StageOnePoints[selectedDestination].transform.position);

                previousDestination = selectedDestination;

                isVanishingAndReappearing = true;
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                navMeshAgent.SetDestination(StageOnePoints[selectedDestination].transform.position);

                selectedDestination = selectedDestination + 1;

                if(selectedDestination == 5)
                {
                    selectedDestination = 0;
                }

                int randNum = Random.Range(0,6);

                if(randNum == 1) // May randomly switch to Movement Pattern 3B // To be removed in later builds
                {
                    previousMovementPattern = currentMovementPattern;
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;

                    Debug.Log("Current Move Pattern = " + currentMovementPattern);
                }

                isVanishingAndReappearing = true;
            }
        }
    }
}
