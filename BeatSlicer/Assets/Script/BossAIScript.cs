using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossAIScript:MonoBehaviour
{
    [SerializeField]

    public enum MovementPattern
    {
        MOVE_PATTERN_1 = 0,
        MOVE_PATTERN_2, // Bombing Run & Circle Rain
        MOVE_PATTERN_3A,
        MOVE_PATTERN_3B, // Bombing Run & Cone Shot
        BOSS_ULTIMATE_PHASE,
        BOSS_STUN,
        SET_MOVE_PATTERN
    };

    private GameObject player;

    private AudioSource bossAudioSource;
    private AudioClip bossAppearingSound;

    private bool isVanishingAndReappearing = false;
    private bool isFlyingUp = false;

    private bool bossFloatingBool;

    private float movementTimer;
    private float movementTimerTemp;

    private bool isMoving = false;

    private bool bulletPatternReady = true;
    private bool isOutside = false;

    private float randNum;
    private float patternTimer = 0.0f;
    private float multiTimer = 0.0f;
    private float bossStunTimer = 0.0f;
    private bool multiTimerHasStarted = false;

    public List<GameObject> StageOnePoints;
    public List<GameObject> StageTwoPoints;

    private int selectedDestination;
    private int previousDestination;
    NavMeshAgent navMeshAgent;

    private int bombingRunCounter = 0;
    private int bombingRunRandNum;

    private readonly int[] bossStageOneMovement = new int[] { 0,1,4 };
    private readonly int[] bossStageTwoMovementStart = new int[] { 0,2,4,6,7,8 };
    private readonly int[] bossStageTwoMovementEnd = new int[] { 1,3,5,7,6,8 };
    private readonly int[] bossStageThreeMovement = new int[] { 4,2,3,1,5 };

    public MovementPattern currentMovementPattern = MovementPattern.SET_MOVE_PATTERN;
    private MovementPattern previousMovementPattern = MovementPattern.SET_MOVE_PATTERN;

    private int stageThreePatternCount = 0;
    private bool ultimateHasStarted = false;
    private bool ultimateTimerHasStarted = false;
    private float ultimateTimer = 0.0f;

    public Image healthBar; /// Boss Parameters

    public PlayerModelScript playerModelScript;
    public BossShootingScript bossShootingScript;
    public float health;
    private readonly float maxHealth = 1; // Was 5, set to 1

    public Animator bossAnimator;
    public bool playBossAttackingAnimation = false;
    public bool playBossUltimateAnimation = false;
    public bool playBossStunAnimation = false;
    private bool lookAtPlayerAfterRecover = false;
    private bool isDeadTrigger = false;

    #region Public Variable Settings -- For Development Use
    [Header("Ultimate Timer Values")]
    public float ultimateOneTimerValue = 3.0f;
    public float ultimateTwoTimerValue = 6.0f;
    public float ultimateThreeTimerValue = 10.0f;

    [Header("Boss Stun Timer Value")]
    public float bossStunTimerValue = 8.0f;

    [Header("Miscellaneous Settings")]
    public bool ultimateMusicHasStarted = false;
    [Space(10)]
    public float soundCrossfadeEffectValue = 0.5f;
    [Space(10)]
    public bool developmentSettingsEnabled = false;
    #endregion


    void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
        movementTimerTemp = movementTimer;
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        bossAudioSource = GetComponent<AudioSource>();
        bossAnimator = GetComponentInChildren<Animator>();
    }


	void Start()
	{
        movementTimer = 8.0f;

        health = maxHealth;

        previousDestination = 99; // Initializing with a number that is not 0

        currentMovementPattern = MovementPattern.MOVE_PATTERN_1;
        previousMovementPattern = currentMovementPattern;
        BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.TURNING_LEFT;

        navMeshAgent.baseOffset = 2.0f; // Move Boss to starting floating height

        randNum = Random.Range(6.0f, 8.0f);
        bombingRunRandNum = Random.Range(0,5);

        bossAppearingSound = SoundManagerScript.mInstance.FindAudioClip(AudioClipID.SFX_BOSS_APPEARING);
        bossAudioSource.PlayOneShot(bossAppearingSound, 1.0f);
    }


    void Update()
	{
        healthBar.fillAmount = health / maxHealth;

        BossHealthStunnerAndPatternChanger();
        LookAtPlayerFunction();
        BossMovementFunction();
        BulletPatternSetterFunction();
        MultiTimerFunction();

        #region This Checks If Development Settings Is Enabled
        if(developmentSettingsEnabled == true)
        {
            if(playerModelScript != null)
            {
                playerModelScript.health = 9999;
            }
            
            TempMovePatternChangeButton(); // Temporary Movement Pattern Change Button 'J'
            TempBossStunnerButton(); // Temporary Boss Stunner (& Unstunner) Button 'K'
        }
        #endregion
    }


    void FixedUpdate()
    {
        BossFloatingFunction();
    }


    void LookAtPlayerFunction()
    {
        if((currentMovementPattern != MovementPattern.BOSS_STUN && isDeadTrigger != true) || (lookAtPlayerAfterRecover == true && isDeadTrigger != true))
        {
            transform.LookAt(player.transform);
        }

        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles = new Vector3(0,eulerAngles.y,0);
        transform.rotation = Quaternion.Euler(eulerAngles);
    }


    #region Boss Movement Function
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
                    if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                    {
                        bombingRunCounter++;

                        if(bombingRunCounter >= 2)
                        {
                            selectedDestination = 5;

                            bombingRunCounter = 0;
                            previousDestination = 99;
                        }
                        else
                        {
                            do
                            {
                                selectedDestination = Random.Range(0,5);

                            } while(selectedDestination == previousDestination); // Will repeat randomization until selectedDestination != previousDestination
                        }
                    }
                    else
                    {
                        selectedDestination = selectedDestination + 1;
                    }

                    if(selectedDestination == 5)
                    {
                        transform.position = StageTwoPoints[bossStageTwoMovementStart[selectedDestination]].transform.position;
                        navMeshAgent.SetDestination(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position);
                    }

                    isMoving = false;
                }
            }
        }

        if(isMoving == true)
        {
            //bossAnimator.Play("BossMoveForwardAnimation", -1, 0.0f);
        }
    }
    #endregion


    #region Boss Floating Function
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
                bossAudioSource.PlayOneShot(bossAppearingSound,1.0f);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                if(navMeshAgent.baseOffset <= 3.7f || navMeshAgent.baseOffset >= 4.2f)
                {
                    navMeshAgent.baseOffset = 4.0f;
                }

                if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                {
                    selectedDestination = bombingRunRandNum;
                    previousDestination = selectedDestination;
                }
                else
                {
                    selectedDestination = 0;
                }
                
                transform.position = StageTwoPoints[selectedDestination].transform.position;
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                if(navMeshAgent.baseOffset <= 1.7f || navMeshAgent.baseOffset >= 2.2f)
                {
                    navMeshAgent.baseOffset = 2.0f;
                }

                transform.position = StageOnePoints[bossStageThreeMovement[selectedDestination]].transform.position;
                bossAudioSource.PlayOneShot(bossAppearingSound,1.0f);
            }
            else if(currentMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
            {
                if(navMeshAgent.baseOffset <= 3.7f || navMeshAgent.baseOffset >= 4.2f)
                {
                    navMeshAgent.baseOffset = 4.0f;
                }

                selectedDestination = 5;

                navMeshAgent.SetDestination(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position);
                transform.position = StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position;
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
                else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B || currentMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
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
                    if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B || currentMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
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
    #endregion


    #region Bullet Pattern Setter Function
    void BulletPatternSetterFunction()
    {
        if(currentMovementPattern == MovementPattern.BOSS_STUN)
        {
            BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;
        }

        if(isFlyingUp == true /*|| isOutside == true*/)
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
                    multiTimerHasStarted = true;
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
                            multiTimerHasStarted = true;
                        }
                    }
                    else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
                    {
                        if(bulletPatternReady == true)
                        {
                            BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.CIRCLE_RAIN;

                            bulletPatternReady = false;
                            multiTimerHasStarted = true;
                        }
                    }
                }
                else if(selectedDestination != 5)
                {
                    if(isOutside != true)
                    {
                        if(bulletPatternReady == true)
                        {
                            BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.BOMBING_RUN;

                            bulletPatternReady = false;
                            multiTimerHasStarted = true;
                        }
                    }
                }
            }
            else if(currentMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
            {
                if(bulletPatternReady == true)
                {
                    BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.ULTIMATE_ATTACK;

                    ultimateTimerHasStarted = true;
                }
            }
        }
    }
    #endregion


    #region Boss Stunner & Phase Changer Function
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

                if(isFlyingUp == true)
                {
                    isFlyingUp = false;
                }

                if(playBossStunAnimation == false)
                {
                    bossAnimator.Play("BossDownAnimation", -1, 0.0f);

                    if(previousMovementPattern == MovementPattern.MOVE_PATTERN_3B || previousMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
                    {
                        bossAnimator.SetTrigger("IsDead");

                        isDeadTrigger = true;
                    }

                    playBossStunAnimation = true;
                }

                multiTimerHasStarted = false;
                bulletPatternReady = false;

                //Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }

            SoundManagerScript.mInstance.bgmAudioSource.volume -= Time.deltaTime * soundCrossfadeEffectValue; // This works for now (Creates the crossfade sound effect) // is set 0.5f
        }
    }
    #endregion


    #region MultiFunction & MultiTimer Function
    void MultiTimerFunction()
    {
        if(currentMovementPattern == MovementPattern.BOSS_STUN)
        {
            bossStunTimer += Time.deltaTime;

            if(bossStunTimer >= (bossStunTimerValue - 4.0f) && (previousMovementPattern != MovementPattern.BOSS_ULTIMATE_PHASE || previousMovementPattern != MovementPattern.MOVE_PATTERN_3B))
            {
                bossAnimator.SetTrigger("HasRecovered");

                lookAtPlayerAfterRecover = true;
            }

            if(bossStunTimer >= bossStunTimerValue)
            {
                if(previousMovementPattern == MovementPattern.MOVE_PATTERN_1)
                {
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_2;

                    SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_2_INTRO);
                    SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                    SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                    previousDestination = 99; // Reset to number other than 0
                }
                else if(previousMovementPattern == MovementPattern.MOVE_PATTERN_2)
                {
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_3A;

                    SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_1_INTRO);
                    SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                    SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                    selectedDestination = 0; // Reset to 0
                }
                else if(previousMovementPattern == MovementPattern.MOVE_PATTERN_3A)
                {
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;

                    SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_3_INTRO);
                    //SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                    //SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;
                }
                else if(previousMovementPattern == MovementPattern.MOVE_PATTERN_3B || previousMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
                // Might need to adjust this to transition to 'Win Scene' more smoothly
                {
                    previousDestination = 99; // Reset to number other than 0

                    bulletPatternReady = true;
                    multiTimerHasStarted = false;

                    SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_INGAME_1); // <- To Change to Win BGM in future? - 12-10-2018

                    SceneManager.LoadScene("Win Screen"); // Loads Win Scene
                }

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                health = maxHealth;

                bulletPatternReady = true;
                multiTimerHasStarted = false;

                bossStunTimer = 0.0f;

                playBossStunAnimation = false;
                bossAnimator.ResetTrigger("HasRecovered");

                lookAtPlayerAfterRecover = false;
            }
        }
        else
        {
            bossStunTimer = 0.0f;
        }

        if(multiTimerHasStarted == true)
        {
            multiTimer += Time.deltaTime;

            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A) // For Left, Right, Both Attacks
            {
                if(multiTimer >= 4.5f && BossShootingScript.Instance.bulletPatternReadyCheck == true)
                {
                    bulletPatternReady = true;
                    multiTimerHasStarted = false;

                    multiTimer = 0.0f;
                }
            }
            else if((currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B) && selectedDestination != 5) // For Bombing Run
            {
                if(multiTimer >= 1.5f && BossShootingScript.Instance.bulletPatternReadyCheck == true)
                {
                    bulletPatternReady = true;
                    multiTimerHasStarted = false;

                    multiTimer = 0.0f;
                }
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 && selectedDestination == 5) // Circle Rain
            {
                if(multiTimer >= 4.5f && BossShootingScript.Instance.bulletPatternReadyCheck == true)
                {
                    bulletPatternReady = true;
                    multiTimerHasStarted = false;

                    multiTimer = 0.0f;
                }
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B && selectedDestination == 5) // For Cone Shot
            {
                if(multiTimer >= 4.5f && BossShootingScript.Instance.bulletPatternReadyCheck == true)
                {
                    bulletPatternReady = true;
                    multiTimerHasStarted = false;

                    stageThreePatternCount++;

                    if(stageThreePatternCount >= 3)
                    {
                        currentMovementPattern = MovementPattern.BOSS_ULTIMATE_PHASE;

                        ultimateHasStarted = true;
                    }

                    multiTimer = 0.0f;
                }
            }
        }
        else if(multiTimerHasStarted == false)
        {
            multiTimer = 0.0f;
        }

        if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
        {
            patternTimer += Time.deltaTime;

            if(patternTimer >= randNum)
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
                        currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;
                    }

                    isVanishingAndReappearing = true;
                }

                patternTimer = 0.0f;
                randNum = Random.Range(6.0f, 8.0f);
            }
        }
        else
        {
            patternTimer = 0.0f;
        }

        if(ultimateTimerHasStarted == true)
        {
            if(playBossUltimateAnimation == false)
            {
                bossAnimator.Play("BossUltimateAnimation", -1, 0.0f);

                playBossUltimateAnimation = true;
            }

            ultimateTimer += Time.deltaTime;

            if((ultimateTimer >= (ultimateOneTimerValue - 0.5f) && ultimateTimer <= ultimateOneTimerValue || ultimateTimer >= (ultimateOneTimerValue + 2.5f) && ultimateTimer <= (ultimateOneTimerValue + 3.0f)) && BossShootingScript.Instance.ultimateOneReadyCheck == true)
            {
                if(BossShootingScript.Instance.ultimateOneReadyCheck == true)
                {
                    BossShootingScript.Instance.ultimate1 = true;
                }
            }

            if((ultimateTimer >= (ultimateTwoTimerValue - 0.5f) && ultimateTimer <= ultimateTwoTimerValue) && BossShootingScript.Instance.ultimateTwoReadyCheck == true)
            {
                if(BossShootingScript.Instance.ultimateTwoReadyCheck == true)
                {
                    BossShootingScript.Instance.ultimate2 = true;
                }
            }

            if(ultimateTimer >= ultimateThreeTimerValue && BossShootingScript.Instance.ultimateThreeReadyCheck == true)
            {
                BossShootingScript.Instance.ultimate3 = true;
                ultimateTimer = 0.0f;
            }
        }
        else
        {
            ultimateTimer = 0.0f;
        }

        if(SoundManagerScript.mInstance.bgmAudioSource.volume <= 1.0f && currentMovementPattern != MovementPattern.BOSS_STUN && currentMovementPattern != MovementPattern.BOSS_ULTIMATE_PHASE && playerModelScript.health != 0)
        {
            SoundManagerScript.mInstance.bgmAudioSource.volume += Time.deltaTime * 0.2f; // Test Value - 8/10/2018
        }

        if(ultimateHasStarted == true & ultimateMusicHasStarted == false || playerModelScript.health <= 0)
        {
            SoundManagerScript.mInstance.bgmAudioSource.volume -= Time.deltaTime * 0.7f;
        }
    }
    #endregion


    #region Collider Checkers
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
        }

        if(other.tag == "Boundary")
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
    #endregion


    /// Development Settings Functions Below


    #region Development Settings Functions
    void TempMovePatternChangeButton() // Press 'J' To Change Between Movement Patterns
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_2;

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_2_INTRO);
                SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                bulletPatternReady = true;
                multiTimerHasStarted = false;

                previousDestination = 99; // Reset to number other than 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3A;

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_1_INTRO);
                SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                bulletPatternReady = true;
                multiTimerHasStarted = false;

                selectedDestination = 0; // Reset to 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_INGAME_2);
                //SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                //SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                bulletPatternReady = true;
                multiTimerHasStarted = false;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.BOSS_ULTIMATE_PHASE;

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                previousDestination = 99; // Reset to number other than 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_1;

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_1_INTRO);
                SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                ultimateHasStarted = false;
                ultimateMusicHasStarted = false;
                bulletPatternReady = true;
                multiTimerHasStarted = false;

                previousDestination = 99; // Reset to number other than 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
        }
    }


    void TempBossStunnerButton() // Press 'K' To Stun (& Unstun) Boss
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

                multiTimerHasStarted = true;
                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else
            {
                currentMovementPattern = previousMovementPattern;

                bulletPatternReady = true;
                multiTimerHasStarted = false;
                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
        }
    }
    #endregion
}