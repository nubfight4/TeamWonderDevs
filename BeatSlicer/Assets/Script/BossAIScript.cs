using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossAIScript : MonoBehaviour
{
    [SerializeField]

    public enum MovementPattern
    {
        MOVE_PATTERN_1 = 0,
        MOVE_PATTERN_2, // Circle Rain
        MOVE_PATTERN_3A,
        MOVE_PATTERN_3B, // Cone Shot
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

    private bool bulletPatternReady = true;
    private bool isOutside = false;

    private float tempNum = 0.0f; // Temporary
    private bool tempTimerHasStarted = false; // Temporary

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
    private MovementPattern previousMovementPattern = MovementPattern.SET_MOVE_PATTERN;

    /// Boss Parameters
    public Image healthBar;

    public float health;
    private readonly float maxHealth = 1; // Was 5, set to 1

    public AudioClip bossWooshSound;
    public GameObject bossDamagedVFX;

    void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
        movementTimerTemp = movementTimer;
        navMeshAgent = this.GetComponent<NavMeshAgent>();
    }


	void Start()
	{
        health = maxHealth;

        previousDestination = 99; // Initializing with a number that is not 0

        currentMovementPattern = MovementPattern.MOVE_PATTERN_1;
        previousMovementPattern = currentMovementPattern;
        BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.TURNING_LEFT;

        navMeshAgent.baseOffset = 2.0f; // Move Boss to starting floating height

        StartCoroutine(TempMovePatternTimer()); // To be removed, edited or refined later

        //Debug.Log("Current Move Pattern = " + currentMovementPattern);
        Debug.Log("Temporary Movement Pattern Change Button 'J'");
        Debug.Log("Temporary Boss Stunner (& Unstunner) Button 'K'");
    }


    void Update()
	{
        healthBar.fillAmount = health / maxHealth;

        BossHealthStunnerAndPatternChanger(); // Boss Health, Stunner and Movement Pattern Changer Function
        LookAtPlayerFunction();
        BossMovementFunction();
        BulletPatternSetterFunction();

        TempMultiTimerFunction();
        TempMovePatternChangeButton(); // Temporary Movement Pattern Change Button 'J' // To disable by Public Playtest
        TempBossStunnerButton(); // Temporary Boss Stunner (& Unstunner) Button 'K' // To disable by Public Playtest
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

                if(isFlyingUp != true && currentMovementPattern == MovementPattern.MOVE_PATTERN_2 && currentMovementPattern != MovementPattern.BOSS_STUN) // If MOVE_PATTERN_2, this will repeat the cycle
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

                float distance = Vector3.Distance(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position, transform.position);

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
                gameObject.GetComponent<AudioSource>().PlayOneShot(bossWooshSound,1.0f);
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
                gameObject.GetComponent<AudioSource>().PlayOneShot(bossWooshSound,1.0f);
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

                isFlyingUp = false;
            }
        }
        else // This is for managing the floating distance from the ground
        {
            if(currentMovementPattern == MovementPattern.BOSS_STUN)
            {
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
                            navMeshAgent.baseOffset += Time.deltaTime * 2.0f; // Sped up floating by 2.0f if Boss previous state was BOSS_STUN
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

        if(isFlyingUp == true || isOutside == true)
        {
            BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;
        }
        else
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                if(bulletPatternReady == true)
                {
                    BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.TURNING_LEFT;

                    bulletPatternReady = false;
                    tempTimerHasStarted = true; // Temporary Timer to re-enable attack after 4.0f seconds
                }
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                if(selectedDestination == 5)
                {
                    if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                    {
                        if(bulletPatternReady == true)
                        {
                            BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.CONE_SHOT;

                            bulletPatternReady = false;
                            tempTimerHasStarted = true; // Temporary Timer to re-enable Cone Shot after 4.5f seconds
                        }
                    }
                    else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
                    {
                        if(bulletPatternReady == true)
                        {
                            BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.CIRCLE_RAIN;

                            bulletPatternReady = false;
                            tempTimerHasStarted = true; // Temporary Timer to re-enable Circle Rain after 4.5f seconds
                        }
                    }
                }
                else if(selectedDestination != 5)
                {
                    if(bulletPatternReady == true)
                    {
                        BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.BOMBING_RUN;

                        bulletPatternReady = false;
                        tempTimerHasStarted = true; // Temporary Timer to re-enable Bombing Run after 1.5f seconds
                    }
                }
            }
        }
    }


    void BossHealthStunnerAndPatternChanger()
    {
        if(health <= 0)
        {
            if(currentMovementPattern != MovementPattern.BOSS_STUN)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.BOSS_STUN;

                if(isOutside == true) // Spawn Boss back inside if Boss was stunned outside
                {
                    transform.position = StageTwoPoints[bossStageTwoMovementEnd[5]].transform.position;

                    isOutside = false;
                }

                tempTimerHasStarted = true;

                //Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
        }
    }


    void OnTriggerEnter(Collider other) // For isOutside boolean usage & Boss Health/Stun
    {
        if(other.tag == "ChargeSlashProjectile")
        {
            if(health <= 0)
            {
                health = 0;
            }
            else
            {
                health--;
            }
            Instantiate(bossDamagedVFX, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation, transform.parent);
        }

        if(other.tag == "Boundary")
        {
            isOutside = false;
        }
    }


    void OnTriggerStay(Collider other) // For isOutside boolean usage
    {
        if(other.tag == "Boundary")
        {
            isOutside = false;
        }
        else
        {
            isOutside = false;
        }
    }


    void OnTriggerExit(Collider other) // For isOutside boolean usage
    {
        if(other.tag == "Boundary")
        {
            isOutside = true;
        }
    }


    /// Temporary Functions Below


    void TempMultiTimerFunction() // To re-enable bulletPatternReady boolean and other IMPORTANT functions // To be edited and refined later
    {
        if(tempTimerHasStarted == true)
        {
            tempNum += Time.deltaTime;

            if(tempNum >= 4.5f && (currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)) // For Left, Right, Both Attacks
            {
                tempNum = 0;

                bulletPatternReady = true;
                tempTimerHasStarted = false;
            }
            else if(tempNum >= 1.5f && (currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B) && selectedDestination != 5) // For Bombing Run
            {
                tempNum = 0;

                bulletPatternReady = true;
                tempTimerHasStarted = false;
            }
            else if(tempNum >= 4.5f && (currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B) && selectedDestination == 5) // For Cone Shot or Circle Rain
            {
                tempNum = 0;

                bulletPatternReady = true;
                tempTimerHasStarted = false;
            }
            else if(tempNum >= 5.0f && currentMovementPattern == MovementPattern.BOSS_STUN) // 5 seconds of the BOSS_STUN state before resuming previous state // May adjust values later
            {
                tempNum = 0;

                if(previousMovementPattern == MovementPattern.MOVE_PATTERN_1)
                {
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_2;

                    previousDestination = 99; // Reset to number other than 0
                }
                else if(previousMovementPattern == MovementPattern.MOVE_PATTERN_2)
                {
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_3A;

                    selectedDestination = 0; // Reset to 0
                }
                else if(previousMovementPattern == MovementPattern.MOVE_PATTERN_3A)
                {
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;
                }
                else if(previousMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                {
                    previousDestination = 99; // Reset to number other than 0

                    bulletPatternReady = true;
                    tempTimerHasStarted = false;

                    SceneManager.LoadScene("Win Screen"); // Loads Win Scene
                }

                //Debug.Log("Current Move Pattern = " + currentMovementPattern);

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                health = maxHealth;

                bulletPatternReady = true;
                tempTimerHasStarted = false;
            }
        }
        else if(tempTimerHasStarted == false)
        {
            tempNum = 0;
        }
    }


    void TempMovePatternChangeButton() // Press 'J' To Change Between Movement Patterns // To be edited or removed in later builds
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_2;

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                bulletPatternReady = true;
                tempTimerHasStarted = false;

                previousDestination = 99; // Reset to number other than 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3A;

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                bulletPatternReady = true;
                tempTimerHasStarted = false;

                selectedDestination = 0; // Reset to 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                bulletPatternReady = true;
                tempTimerHasStarted = false;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_1;

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                bulletPatternReady = true;
                tempTimerHasStarted = false;

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

                if(isOutside == true) // Spawn Boss back inside if Boss was stunned outside
                {
                    transform.position = StageTwoPoints[bossStageTwoMovementEnd[5]].transform.position;

                    isOutside = false;
                }

                tempTimerHasStarted = true;
                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else
            {
                currentMovementPattern = previousMovementPattern;

                bulletPatternReady = true;
                tempTimerHasStarted = false;
                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
        }
    }


    private IEnumerator TempMovePatternTimer() // Old Temporary Movement Pattern Timer & Setter // To be refined, edited (or removed) later
    {
        while(true) // Currently randomizes Boss position during Move Pattern 1 & Move Pattern 3A
        {
            yield return new WaitForSeconds(Random.Range(5,11));

            if(currentMovementPattern != MovementPattern.BOSS_STUN)
            {
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

                    isVanishingAndReappearing = true;
                }
            }
        }
    }
}
