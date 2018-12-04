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
        MOVE_PATTERN_2, // Bombing Run then Circle Rain
        MOVE_PATTERN_3A, // Bullet Turning & Cone Shot
        MOVE_PATTERN_3B, // Bombing Run then Cone Shot
        BOSS_ULTIMATE_PHASE,
        BOSS_STUN,
        SEAN_FINAL_SOLUTION,
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
    public bool isOutside = false;
    private bool isAtCenterpoint = false;

    private float randNum;
    private float patternTimer = 0.0f;
    private float bossStunTimer = 0.0f;

    public List<GameObject> StageOnePoints;
    public List<GameObject> StageTwoPoints;

    private int selectedDestination;
    private int previousDestination;
    NavMeshAgent navMeshAgent;

    private int bombingRunPassCounter = 0;
    private int bombingRunRandNum;

    private readonly int[] bossStageOneMovement = new int[] { 0,1,4 };
    private readonly int[] bossStageTwoMovementStart = new int[] { 0,2,4,6,7,8 };
    private readonly int[] bossStageTwoMovementEnd = new int[] { 1,3,5,7,6,8 };
    private readonly int[] bossStageThreeMovement = new int[] { 4,2,3,1,5 };

    public MovementPattern currentMovementPattern = MovementPattern.SET_MOVE_PATTERN;
    private MovementPattern previousMovementPattern = MovementPattern.SET_MOVE_PATTERN;

    private int stageThreePatternCount = 0;
    private bool ultimateHasStarted = false;

    public Image healthBar; /// Boss Parameters

    public PlayerModelScript playerModelScript;
    public BossShootingScript bossShootingScript;
    public SceneTransitionScript sceneTransitionScript;
    public float health;
    private readonly float maxHealth = 1; // Was 5, set to 1

    public Animator bossAnimator;
    public bool playBossLaughAnimation = false;
    public bool playBossAttackingAnimation = false;
    public bool playBossUltimateAnimation = false;
    public bool playBossStunAnimation = false;
    //private bool lookAtPlayerAfterRecover = false;
    private bool isDeadTrigger = false;
    private float bossLaughTimer = 0.0f;
    private bool bossLaughTriggered = false;

    private bool isBulletTurningFirstStart = true;

    private bool isBombingRunFirstStart = true;

    private bool isCircleRainFirstStart = true;
    private float circleRainCounter = 0.0f;

    private bool isConeShotFirstStart = true;
    private float coneShotCounter = 0.0f;
    private float coneShotDelayValue = 7.0f;

    private bool isChaosVortexFirstStart = true;
    private float chaosVortexDelayValue = 5.0f;

    private bool isSuperMegaUltraDeathBombFirstStart = true;
    private float superMegaUltraDeathBombCounter = 0.0f;
    private float superMegaUltraDeathBombDelayValue = 9.0f;

    private float ultimatePhaseCounter = 0.0f;


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

        playBossLaughAnimation = true;
        bossLaughTriggered = false;
        bossLaughTimer = 0.0f;   

        currentMovementPattern = MovementPattern.MOVE_PATTERN_1;
        previousMovementPattern = currentMovementPattern;

        isBulletTurningFirstStart = true;

        navMeshAgent.baseOffset = 2.0f; // Move Boss to starting floating height

        randNum = Random.Range(6.0f, 8.0f);
        bombingRunRandNum = Random.Range(0,5);

        bossAppearingSound = SoundManagerScript.mInstance.FindAudioClip(AudioClipID.SFX_BOSS_APPEARING);
        bossAudioSource.PlayOneShot(bossAppearingSound, 1.0f);
    }


    void Update()
	{
        healthBar.fillAmount = health / maxHealth;

        if(playBossLaughAnimation == true)
        {
            bossLaughTimer += Time.deltaTime;

            if(bossLaughTimer >= 1.0f && bossLaughTriggered == false)
            {
                bossAnimator.Play("BossLaughAnimation",-1,0.0f);

                bossLaughTriggered = true;
            }

            if(bossLaughTimer >= 3.6f)
            {
                playBossLaughAnimation = false;

                bossAnimator.Play("BossAttack2Animation",-1,0.0f);
            }

            SoundManagerScript.mInstance.bgmAudioSource.volume += Time.deltaTime * 0.2f; // Test Value - 8/10/2018
        }
        else
        {
            BossHealthStunnerAndPatternChanger();        
            BossMovementFunction();
            BulletPatternSetterFunction();
            TheMultiFunction();
        }

        LookAtPlayerFunction();

        #region This Checks If Development Settings Is Enabled
        if(developmentSettingsEnabled == true)
        {
            if(playerModelScript != null)
            {
                playerModelScript.health = 9999;
            }
            
            TempMovePatternChangeButton(); // Temporary Movement Pattern Change Button 'J'
            //TempBossStunnerButton(); // Temporary Boss Stunner (& Unstunner) Button 'K' -- Too Buggy To Be Used
        }
        #endregion
    }


    void FixedUpdate()
    {
        BossFloatingFunction();
    }


    void LookAtPlayerFunction()
    {
        if((currentMovementPattern != MovementPattern.BOSS_STUN && isDeadTrigger != true) /*|| (lookAtPlayerAfterRecover == true && isDeadTrigger != true)*/)
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

                isMoving = false;
            }
            else if(isMoving != true && isFlyingUp != true && isVanishingAndReappearing != true && isAtCenterpoint != true)
            {
                transform.position = StageTwoPoints[bossStageTwoMovementStart[selectedDestination]].transform.position;

                isMoving = true;
            }
            else if(isMoving == true)
            {
                navMeshAgent.speed = 12;

                navMeshAgent.SetDestination(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position);

                float distance = Vector3.Distance(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position, transform.position);

                if(distance <= 3.0f)
                {
                    if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                    {
                        bombingRunPassCounter++;

                        if(bombingRunPassCounter >= 1) // Set To 1 Pass Only
                        {
                            selectedDestination = 5;

                            bombingRunPassCounter = 0;
                            previousDestination = 99;
                        }
                        else
                        {
                            do
                            {
                                selectedDestination = Random.Range(0,4);

                            } while(selectedDestination == previousDestination); // Will repeat randomization until selectedDestination != previousDestination
                        }
                    }
                    else
                    {
                        selectedDestination = selectedDestination + 1;
                    }

                    if(selectedDestination == 5)
                    {
                        if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 && isFlyingUp != true)
                        {
                            isAtCenterpoint = true;
                        }

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
        if(isAtCenterpoint == true)
        {
            movementTimerTemp -= Time.deltaTime;

            if(movementTimerTemp <= 0.0f)
            {
                movementTimerTemp = movementTimer;

                isFlyingUp = true;
                isAtCenterpoint = false;
            }
        }
        else
        {
            movementTimerTemp = movementTimer;
        }

        if(isFlyingUp == true)
        {
            navMeshAgent.baseOffset += Time.deltaTime * 5.0f;

            if(navMeshAgent.baseOffset >= 19.0f && selectedDestination == 5)
            {
                navMeshAgent.baseOffset = 4.8f;
                selectedDestination = 0;

                transform.position = StageTwoPoints[selectedDestination].transform.position;

                isFlyingUp = false;
            }
        }

        if(isVanishingAndReappearing == true)
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                if(navMeshAgent.baseOffset <= 2.5f || navMeshAgent.baseOffset >= 3.0f)
                {
                    navMeshAgent.baseOffset = 2.8f;
                }

                transform.position = StageOnePoints[selectedDestination].transform.position;
                bossAudioSource.PlayOneShot(bossAppearingSound,1.0f);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                if(navMeshAgent.baseOffset <= 4.5f || navMeshAgent.baseOffset >= 5.0f)
                {
                    navMeshAgent.baseOffset = 4.8f;
                }

                if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
                {
                    selectedDestination = bombingRunRandNum;
                    previousDestination = selectedDestination;
                }
                
                transform.position = StageTwoPoints[selectedDestination].transform.position;
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                if(navMeshAgent.baseOffset <= 2.5f || navMeshAgent.baseOffset >= 3.0f)
                {
                    navMeshAgent.baseOffset = 2.8f;
                }

                transform.position = StageOnePoints[bossStageThreeMovement[selectedDestination]].transform.position;
                bossAudioSource.PlayOneShot(bossAppearingSound,1.0f);
            }
            else if(currentMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
            {
                if(navMeshAgent.baseOffset <= 4.5f || navMeshAgent.baseOffset >= 5.0f)
                {
                    navMeshAgent.baseOffset = 4.8f;
                }

                selectedDestination = 5;

                navMeshAgent.SetDestination(StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position);
                transform.position = StageTwoPoints[bossStageTwoMovementEnd[selectedDestination]].transform.position;
            }

            isVanishingAndReappearing = false;
        }
        else // This is for managing the floating distance from the ground
        {
            if(currentMovementPattern == MovementPattern.BOSS_STUN)
            {
                /*
                if(navMeshAgent.baseOffset > 1.9f)
                {
                    navMeshAgent.baseOffset -= Time.deltaTime * 5.0f; // Time for Boss to fall when stunned set to Time.deltaTime * 5.0f;
                }
                else if(navMeshAgent.baseOffset <= 2.9f)
                {
                    navMeshAgent.baseOffset = 2.9f;  
                }
                */

                if(navMeshAgent.baseOffset > 1.5f)
                {
                    navMeshAgent.baseOffset -= Time.deltaTime * 5.0f; // Time for Boss to fall when stunned set to Time.deltaTime * 5.0f;
                }
                else
                {
                    navMeshAgent.baseOffset = 1.5f;
                }
            }
            else
            {
                if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
                {
                    if(navMeshAgent.baseOffset >= 3.0f)
                    {
                        bossFloatingBool = false;
                    }
                    else if(navMeshAgent.baseOffset <= 2.5f)
                    {
                        bossFloatingBool = true;
                    }
                }
                else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B || currentMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
                {
                    if(navMeshAgent.baseOffset >= 5.0f)
                    {
                        bossFloatingBool = false;
                    }
                    else if(navMeshAgent.baseOffset <= 4.5f)
                    {
                        bossFloatingBool = true;
                    }
                }

                if(bossFloatingBool == true)
                {
                    if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2 || currentMovementPattern == MovementPattern.MOVE_PATTERN_3B || currentMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
                    {
                        if(navMeshAgent.baseOffset < 4.5f)
                        {
                            navMeshAgent.baseOffset += Time.deltaTime * 2.5f; // Sped up floating by 2.0f if Boss previous state was BOSS_STUN
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

            RestBulletActiveFunction();
            ResetCounterFunction();
        }

        if(isFlyingUp == true)
        {
            RestBulletActiveFunction();
            ResetCounterFunction();

            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
            {
                isBombingRunFirstStart = true;
            }
        }
        else
        {
            if(currentMovementPattern == MovementPattern.MOVE_PATTERN_1)
            {
                if(BossShootingScript.Instance.isBulletTurningActive == false)
                {
                    if(isBulletTurningFirstStart == true)
                    {
                        BossShootingScript.Instance.bulletTurningInitialRun = true;

                        isBulletTurningFirstStart = false;
                    }
                }
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
            {
                if(selectedDestination != 5 && isOutside != true)
                {
                    if(BossShootingScript.Instance.isBombingRunActive == false)
                    {
                        if(isBombingRunFirstStart == true)
                        {
                            BossShootingScript.Instance.bombingRunInitialRun = true;

                            isBombingRunFirstStart = false;
                        }
                    }
                }
                else if(selectedDestination == 5)
                {
                    BossShootingScript.Instance.isBombingRunActive = false;

                    if(BossShootingScript.Instance.isCircleRainActive == false)
                    {
                        if(isCircleRainFirstStart == true)
                        {
                            BossShootingScript.Instance.circleRainInitialRun = true;

                            isCircleRainFirstStart = false;
                        }
                        else
                        {
                            circleRainCounter += Time.deltaTime;

                            if(circleRainCounter >= 4.5f)
                            {
                                BossShootingScript.Instance.circleRainInitialRun = true;

                                circleRainCounter = 0.0f;
                            }
                        }
                    }
                    else
                    {
                        circleRainCounter = 0.0f;
                    }
                }
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                if(BossShootingScript.Instance.isBulletTurningActive == false)
                {
                    if(isBulletTurningFirstStart == true)
                    {
                        BossShootingScript.Instance.bulletTurningInitialRun = true;

                        isBulletTurningFirstStart = false;
                    }
                }

                if(BossShootingScript.Instance.isConeShotActive == false)
                {
                    if(isConeShotFirstStart == true)
                    {
                        BossShootingScript.Instance.coneShotInitialRun = true;

                        isConeShotFirstStart = false;
                    }
                    else
                    {
                        coneShotCounter += Time.deltaTime;

                        if(coneShotCounter >= 4.5f)
                        {
                            BossShootingScript.Instance.coneShotInitialRun = true;

                            coneShotCounter = 0.0f;
                        }
                    }
                }
                else
                {
                    coneShotCounter = 0.0f;
                }
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                if(selectedDestination != 5 && isOutside != true)
                {
                    if(BossShootingScript.Instance.isBombingRunActive == false)
                    {
                        if(isBombingRunFirstStart == true)
                        {
                            BossShootingScript.Instance.bombingRunInitialRun = true;

                            isBombingRunFirstStart = false;
                        }
                    }
                }
                else if(selectedDestination == 5)
                {
                    BossShootingScript.Instance.isBombingRunActive = false;
 
                    if(BossShootingScript.Instance.isConeShotActive == false)
                    {
                        if(stageThreePatternCount >= 3)
                        {
                            RestBulletActiveFunction();
                            ResetCounterFunction();

                            currentMovementPattern = MovementPattern.BOSS_ULTIMATE_PHASE;
                            ultimateHasStarted = true;

                            if(playBossUltimateAnimation == false)
                            {
                                bossAnimator.Play("BossUltimateAnimation",-1,0.0f);

                                playBossUltimateAnimation = true;
                            }

                            isChaosVortexFirstStart = true;
                            isSuperMegaUltraDeathBombFirstStart = true;
                            isConeShotFirstStart = true;
                        }
                        else if(ultimateHasStarted != true)
                        {
                            if(isConeShotFirstStart == true)
                            {
                                BossShootingScript.Instance.coneShotInitialRun = true;

                                stageThreePatternCount++;

                                isConeShotFirstStart = false;
                            }
                            else
                            {
                                coneShotCounter += Time.deltaTime;

                                if(coneShotCounter >= 4.5f)
                                {
                                    BossShootingScript.Instance.coneShotInitialRun = true;

                                    stageThreePatternCount++;

                                    coneShotCounter = 0.0f;
                                }
                            }
                        }
                    }
                    else
                    {
                        coneShotCounter = 0.0f;
                    }

                }
            }
            else if(currentMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
            {
                if(ultimatePhaseCounter <= 10.0f && playBossUltimateAnimation == true)
                {
                    ultimatePhaseCounter += Time.deltaTime;
                }

                if(ultimatePhaseCounter >= chaosVortexDelayValue)
                {
                    if(BossShootingScript.Instance.isChaosVortexActive == false)
                    {
                        if(isChaosVortexFirstStart == true)
                        {
                            BossShootingScript.Instance.chaosVortexInitialRun = true;

                            if(ultimateMusicHasStarted == false)
                            {
                                if(developmentSettingsEnabled == true)
                                {
                                    //SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_ULTIMATE_ATTACK);
                                    SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_4_INTRO);
                                }
                                else
                                {
                                    SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_4_INTRO);
                                }

                                SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                                SoundManagerScript.mInstance.bgmAudioSource.volume = 1.0f;

                                ultimateMusicHasStarted = true;
                            }

                            isChaosVortexFirstStart = false;
                        }
                    }
                    else
                    {
                        isChaosVortexFirstStart = false;
                    }
                }

                if(ultimatePhaseCounter >= superMegaUltraDeathBombDelayValue)
                {
                    if(BossShootingScript.Instance.isSuperMegaUltraDeathBombActive == false)
                    {
                        if(isSuperMegaUltraDeathBombFirstStart == true)
                        {
                            BossShootingScript.Instance.isSuperMegaUltraDeathBombActive = true;

                            isSuperMegaUltraDeathBombFirstStart = false;
                        }
                        else
                        {
                            superMegaUltraDeathBombCounter += Time.deltaTime;

                            if(superMegaUltraDeathBombCounter >= 3.0f)
                            {
                                BossShootingScript.Instance.isSuperMegaUltraDeathBombActive = true;

                                superMegaUltraDeathBombCounter = 0.0f;
                            }
                        }
                    }
                    else
                    {
                        superMegaUltraDeathBombCounter = 0.0f;
                    }
                }

                if(ultimatePhaseCounter >= coneShotDelayValue)
                {
                    if(BossShootingScript.Instance.isConeShotActive == false)
                    {
                        if(isConeShotFirstStart == true)
                        {
                            BossShootingScript.Instance.coneShotInitialRun = true;

                            isConeShotFirstStart = false;
                        }
                        else
                        {
                            coneShotCounter += Time.deltaTime;

                            if(coneShotCounter >= 3.0f)
                            {
                                BossShootingScript.Instance.coneShotInitialRun = true;

                                coneShotCounter = 0.0f;
                            }
                        }
                    }
                    else
                    {
                        coneShotCounter = 0.0f;
                    }
                }
            }
        }
    }
    #endregion


    #region Boss Stunner Function
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

                //Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }

            SoundManagerScript.mInstance.bgmAudioSource.volume -= Time.deltaTime * soundCrossfadeEffectValue; // This works for now (Creates the crossfade sound effect) // is set 0.5f
        }
    }
    #endregion


    #region The 'MultiFunction' Function
    void TheMultiFunction()
    {
        if(currentMovementPattern == MovementPattern.BOSS_STUN)
        {
            bossStunTimer += Time.deltaTime;

            if(bossStunTimer >= (bossStunTimerValue - 4.0f) && (previousMovementPattern != MovementPattern.BOSS_ULTIMATE_PHASE || previousMovementPattern != MovementPattern.MOVE_PATTERN_3B))
            {
                bossAnimator.SetTrigger("HasRecovered");

                //lookAtPlayerAfterRecover = true;
            }

            if(bossStunTimer >= bossStunTimerValue)
            {
                if(previousMovementPattern == MovementPattern.MOVE_PATTERN_1)
                {
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_2;

                    isBombingRunFirstStart = true;
                    isCircleRainFirstStart = true;

                    SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_2_INTRO);
                    SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                    SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                    previousDestination = 99; // Reset to number other than 0
                    selectedDestination = 0; // Reset to 0
                }
                else if(previousMovementPattern == MovementPattern.MOVE_PATTERN_2)
                {
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_3A;

                    isBulletTurningFirstStart = true;
                    isConeShotFirstStart = false;

                    SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_1_INTRO);
                    SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                    SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                    selectedDestination = 0; // Reset to 0
                }
                else if(previousMovementPattern == MovementPattern.MOVE_PATTERN_3A)
                {
                    currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;

                    isBombingRunFirstStart = true;
                    isConeShotFirstStart = true;

                    SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_3_INTRO);
                    //SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                    //SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;
                }
                else if(previousMovementPattern == MovementPattern.MOVE_PATTERN_3B || previousMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
                // Might need to adjust this to transition to 'Win Scene' more smoothly
                {
                    previousDestination = 99; // Reset to number other than 0

                }

                BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

                isVanishingAndReappearing = true;
                isFlyingUp = false;
                isMoving = false;

                health = maxHealth;

                bossStunTimer = 0.0f;

                playBossStunAnimation = false;
                bossAnimator.ResetTrigger("HasRecovered");

                //lookAtPlayerAfterRecover = false;
            }
        }
        else
        {
            bossStunTimer = 0.0f;
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
                        //currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;

                        selectedDestination = 0;
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


    #region Reset Counter & Bullet Active Function
    void ResetCounterFunction()
    {
        circleRainCounter = 0.0f;
        coneShotCounter = 0.0f;
        superMegaUltraDeathBombCounter = 0.0f;
        ultimatePhaseCounter = 0.0f;
    }

    void RestBulletActiveFunction()
    {
        BossShootingScript.Instance.isBulletTurningActive = false;
        BossShootingScript.Instance.isBombingRunActive = false;
        BossShootingScript.Instance.isCircleRainActive = false;
        BossShootingScript.Instance.isConeShotActive = false;
        BossShootingScript.Instance.isChaosVortexActive = false;
        BossShootingScript.Instance.isSuperMegaUltraDeathBombActive = false;
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

                isBombingRunFirstStart = true;
                isCircleRainFirstStart = true;

                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_2_INTRO);
                SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                previousDestination = 99; // Reset to number other than 0
                selectedDestination = 0; // Reset to 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_2)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3A;

                isBulletTurningFirstStart = true;
                isConeShotFirstStart = false;

                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_1_INTRO);
                SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                selectedDestination = 0; // Reset to 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3A)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_3B;

                isBombingRunFirstStart = true;
                isConeShotFirstStart = true;

                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_3_INTRO);
                //SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                //SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.MOVE_PATTERN_3B)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.BOSS_ULTIMATE_PHASE;

                SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                ultimateHasStarted = true;

                if(playBossUltimateAnimation == false)
                {
                    bossAnimator.Play("BossUltimateAnimation", -1, 0.0f);

                    playBossUltimateAnimation = true;
                }

                isChaosVortexFirstStart = true;
                isSuperMegaUltraDeathBombFirstStart = true;
                isConeShotFirstStart = true;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else if(currentMovementPattern == MovementPattern.BOSS_ULTIMATE_PHASE)
            {
                previousMovementPattern = currentMovementPattern;
                currentMovementPattern = MovementPattern.MOVE_PATTERN_1;

                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_1_INTRO);
                SoundManagerScript.mInstance.bgmAudioSource.loop = false;
                SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;

                ultimateHasStarted = false;
                playBossUltimateAnimation = false;
                ultimateMusicHasStarted = false;

                previousDestination = 99; // Reset to number other than 0

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }


            RestBulletActiveFunction();
            ResetCounterFunction();

            BossShootingScript.Instance.currentBulletPattern = BossShootingScript.BulletPatternType.REST;

            isVanishingAndReappearing = true;
            isFlyingUp = false;
            isMoving = false;

            health = maxHealth;

            bossStunTimer = 0.0f;

            playBossStunAnimation = false;
            bossAnimator.ResetTrigger("HasRecovered");
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

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
            else
            {
                currentMovementPattern = previousMovementPattern;

                Debug.Log("Current Move Pattern = " + currentMovementPattern);
            }
        }
    }
    #endregion
}