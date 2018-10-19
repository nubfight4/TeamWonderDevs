using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShootingScript : MonoBehaviour
{
    //Singleton Implementation
    private static BossShootingScript instance;
    public static BossShootingScript Instance
    {
        get
        {
            return instance;
        }
    }

    public enum BulletPatternType
    {
        // 1st Bullet Pattern //
        TURNING_RIGHT = 0,
        TURNING_LEFT,
        BOTH_TURNS,

        // 2nd Bullet Pattern //
        BOMBING_RUN,
        CIRCLE_RAIN,

        // 3rd Bullet Pattern
        CONE_SHOT,

        // 4th Bullet Pattern //
        WAIL_OF_THE_BANSHEE,
        WAIL_RANDOM_SHOT,

        // Ultimate Bullet Pattern //
        ULTIMATE_ATTACK,
        CHAOS_VORTEX,
        SUPER_MEGA_ULTRA_DEATH_BOMB,

        REST,
        SET_TYPE
    };

    float timer;
    float timerCountdown;
    float rest;
    float restCountdown;
    float waveCount;
    float vortexWaveCount;
    float bombingTimer;
    float bombingTimerCountdown;
    float circleRainTimer;
    float circleRainTimerCountdown;
    float coneShotTimer;
    float coneShotTimerCountdown;
    float wailOfTheBansheeTimer;
    float wailOfTheBansheeTimerCountdown;
    float wailOfTheBansheeRandomShotTimer;
    float wailOfTheBansheeRandomShotCountdown;
    float wailOfTheBansheeTurning;
    float chaosVortexTimer;
    float chaosVortexTimerCountdown;
    float superBombTimer;
    float superBombTimerCountdown;
    float superMegaUltraDeathBombTimer;
    float superMegaUltraDeathBombTimerCountdown;

    bool dropSuperMegaUltraDeathBomb;

    public BossAIScript bossAIScript;
    public BulletPattern bulletPattern;
    GameObject player;

    public BulletPatternType currentBulletPattern = BulletPatternType.REST;

    [Space(10)] // Just to look nice
    public bool ultimate1 = false;
    public bool ultimate2 = false;
    public bool ultimate3 = false;

    [Space(10)] // Just to look nice
    public bool bulletPatternReadyCheck = true;
    public bool ultimateOneReadyCheck = true;
    public bool ultimateTwoReadyCheck = true;
    public bool ultimateThreeReadyCheck = true;

    [Space(10)] // Just to look nice
    public float bulletStandardHeight = 2.3f;

    //For Singleton usage
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        timer = 1f;
        waveCount = 0;
        vortexWaveCount = 0;
        rest = 2f;
        bombingTimer = 0.8f;
        circleRainTimer = 0.5f;
        coneShotTimer = 0.2f;
        wailOfTheBansheeTimer = 0.1f;
        wailOfTheBansheeRandomShotTimer = 1.0f;
        chaosVortexTimer = 2.0f;
        superBombTimer = 0.4f;
        superMegaUltraDeathBombTimer = 4.0f;
        dropSuperMegaUltraDeathBomb = false;
        currentBulletPattern = BulletPatternType.TURNING_RIGHT;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBulletPattern == BulletPatternType.TURNING_LEFT || currentBulletPattern == BulletPatternType.TURNING_RIGHT)
        {
            bulletPatternReadyCheck = false;

            timerCountdown += Time.deltaTime;

            if(timerCountdown >= timer)
            {
                bossAIScript.playBossAttackingAnimation = true;
                SetBulletPattern();

                if(waveCount >= 4)
                {
                    currentBulletPattern = BulletPatternType.BOTH_TURNS;
                    bossAIScript.playBossAttackingAnimation = true;
                    SetBulletPattern();
                }

                timerCountdown = 0;
            }
        }
        else if(currentBulletPattern != BulletPatternType.TURNING_LEFT || currentBulletPattern != BulletPatternType.TURNING_RIGHT || currentBulletPattern != BulletPatternType.BOTH_TURNS)
        {
            timerCountdown = 0;
        }

        if (currentBulletPattern == BulletPatternType.BOMBING_RUN)
        {
            bulletPatternReadyCheck = false;

            bombingTimerCountdown += Time.deltaTime;
            if (bombingTimerCountdown >= bombingTimer)
            {
                bossAIScript.playBossAttackingAnimation = true;
                SetBulletPattern();
                bombingTimerCountdown = 0;
            }
        }
        else
        {
            bombingTimerCountdown = 0.0f;
        }

        if (currentBulletPattern == BulletPatternType.CIRCLE_RAIN)
        {
            bulletPatternReadyCheck = false;

            circleRainTimerCountdown += Time.deltaTime;
            if (circleRainTimerCountdown >= circleRainTimer)
            {
                SetBulletPattern();
                if (waveCount >= 7)
                {
                    //SetBulletPattern();
                    currentBulletPattern = BulletPatternType.REST;
                    SetBulletPattern();
                    waveCount = 0;
                }
                circleRainTimerCountdown = 0;
            }
        }
        else
        {
            circleRainTimerCountdown = 0.0f;
        }

        if (currentBulletPattern == BulletPatternType.CONE_SHOT || (currentBulletPattern == BulletPatternType.ULTIMATE_ATTACK && ultimate3 == true))
        {
            if(currentBulletPattern == BulletPatternType.CONE_SHOT)
            {
                bulletPatternReadyCheck = false;
            }
            else if((currentBulletPattern == BulletPatternType.ULTIMATE_ATTACK && ultimate3 == true))
            {
                ultimateThreeReadyCheck = false;
            }

            coneShotTimerCountdown += Time.deltaTime;

            if (coneShotTimerCountdown >= coneShotTimer)
            {
                SetBulletPattern();

                if (waveCount >= 5)
                {
                    //SetBulletPattern();

                    if(currentBulletPattern == BulletPatternType.ULTIMATE_ATTACK)
                    {
                        ultimate3 = false;
                        ultimateThreeReadyCheck = true;
                    }
                    else if(currentBulletPattern == BulletPatternType.CONE_SHOT)
                    {
                        currentBulletPattern = BulletPatternType.REST;
                    }

                    SetBulletPattern();
                    waveCount = 0;
                }

                coneShotTimerCountdown = 0;
            }
        }
        else
        {
            coneShotTimerCountdown = 0.0f;
        }

        if (currentBulletPattern == BulletPatternType.WAIL_OF_THE_BANSHEE)
        {
            wailOfTheBansheeTurning += Time.deltaTime * 20;
            wailOfTheBansheeRandomShotTimer += Time.deltaTime;
            wailOfTheBansheeTimerCountdown += Time.deltaTime;
            if (wailOfTheBansheeTimerCountdown >= wailOfTheBansheeTimer)
            {
                SetBulletPattern();
                wailOfTheBansheeTimerCountdown = 0;
            }
        }

        if (currentBulletPattern == BulletPatternType.WAIL_RANDOM_SHOT)
        {
            wailOfTheBansheeRandomShotCountdown += Time.deltaTime;
            if (wailOfTheBansheeRandomShotCountdown >= wailOfTheBansheeRandomShotTimer)
            {
                SetBulletPattern();
                wailOfTheBansheeRandomShotCountdown = 0;
            }
        }

        if (currentBulletPattern == BulletPatternType.ULTIMATE_ATTACK) // CHAOS_VORTEX
        {
            if(ultimate1 == true)
            {
                ultimateOneReadyCheck = false;

                chaosVortexTimerCountdown += Time.deltaTime;
                if(chaosVortexTimerCountdown >= chaosVortexTimer)
                {
                    //SetBulletPattern();
                    UltimatePatternOne();

                    if(vortexWaveCount >= 15)
                    {
                        //SetBulletPattern();
                        //currentBulletPattern = BulletPatternType.REST;
                        //SetBulletPattern();

                        ultimate1 = false;
                        ultimateOneReadyCheck = true;
                        vortexWaveCount = 0;
                    }

                    chaosVortexTimerCountdown = 0;
                }
            }

            if(ultimate2 == true) // SUPER_MEGA_ULTRA_DEATH_BOMB <- Why the fancy name? XD
            {
                ultimateTwoReadyCheck = false;

                superMegaUltraDeathBombTimerCountdown += Time.deltaTime;
                superBombTimerCountdown += Time.deltaTime;

                if(superBombTimerCountdown >= superBombTimer && superMegaUltraDeathBombTimerCountdown < superMegaUltraDeathBombTimer)
                {
                    //SetBulletPattern();
                    bossAIScript.playBossAttackingAnimation = true;
                    UltimatePatternTwo();
                    superBombTimerCountdown = 0;
                }

                if(superMegaUltraDeathBombTimerCountdown >= superMegaUltraDeathBombTimer)
                {
                    dropSuperMegaUltraDeathBomb = true;
                    bossAIScript.playBossAttackingAnimation = true;
                    UltimatePatternTwo();

                    //SetBulletPattern();
                    //currentBulletPattern = BulletPatternType.REST;
                    //SetBulletPattern();

                    ultimate2 = false;
                    ultimateTwoReadyCheck = true;
                    dropSuperMegaUltraDeathBomb = false;
                    superMegaUltraDeathBombTimerCountdown = 0;
                }
            }
        }

        #region Commented #1
        /*
        if (currentBulletPattern == BulletPatternType.CHAOS_VORTEX)
        {
            chaosVortexTimerCountdown += Time.deltaTime;
            if (chaosVortexTimerCountdown >= chaosVortexTimer)
            {
                SetBulletPattern();
                if (vortexWaveCount >= 15)
                {
                    //SetBulletPattern();
                    currentBulletPattern = BulletPatternType.REST;
                    SetBulletPattern();
                    vortexWaveCount = 0;
                }
                chaosVortexTimerCountdown = 0;
            }
        }

        if (currentBulletPattern == BulletPatternType.SUPER_MEGA_ULTRA_DEATH_BOMB)
        {
            superMegaUltraDeathBombTimerCountdown += Time.deltaTime;
            superBombTimerCountdown += Time.deltaTime;

            if (superBombTimerCountdown >= superBombTimer && superMegaUltraDeathBombTimerCountdown < superMegaUltraDeathBombTimer)
            {
                SetBulletPattern();
                superBombTimerCountdown = 0;
            }

            if (superMegaUltraDeathBombTimerCountdown >= superMegaUltraDeathBombTimer)
            {
                dropSuperMegaUltraDeathBomb = true;
                SetBulletPattern();
                currentBulletPattern = BulletPatternType.REST;
                SetBulletPattern();
                superMegaUltraDeathBombTimerCountdown = 0;
            }
        }
        */
        #endregion

        if(currentBulletPattern == BulletPatternType.REST)
        {
            // Hi! I AM REST
            bulletPatternReadyCheck = true;
        }


    }

    public void SetBulletPattern()
    {
        if (currentBulletPattern == BulletPatternType.SET_TYPE)
        {
            currentBulletPattern = BulletPatternType.TURNING_RIGHT;
        }

        // 1st stage bullet pattern
        if (currentBulletPattern == BulletPatternType.TURNING_RIGHT)
        {
            for (int i = 0; i < 36; i++)
            {
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
                bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

                if (redBullet != null)
                {
                    redBullet.transform.position = new Vector3(transform.position.x, bulletStandardHeight, transform.position.z);
                    redBullet.transform.rotation = transform.rotation;
                    redBullet.transform.rotation *= Quaternion.Euler(0, i * 10, 0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * 10;
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 5f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_RIGHT;
                    redBullet.SetActive(true);

                    if(i == 0)
                    {
                        SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_BOMBING_RUN_TOUCHDOWN);

                        if(bossAIScript.playBossAttackingAnimation == true)
                        {
                            bossAIScript.bossAnimator.Play("BossAttackAnimation", -1, 0.0f);

                            bossAIScript.playBossAttackingAnimation = false;
                        }
                    }
                }
            }
            currentBulletPattern = BulletPatternType.TURNING_LEFT;
            waveCount++;
        }

        else if (currentBulletPattern == BulletPatternType.TURNING_LEFT)
        {
            for (int i = 0; i < 36; i++)
            {
                GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); // Non-Rhythm Bullet (Non-Rhythm Bullet - Actual)
                bulletPattern = (BulletPattern)blueBullet.GetComponent(typeof(BulletPattern));

                if (blueBullet != null)
                {
                    blueBullet.transform.position = new Vector3(transform.position.x, bulletStandardHeight, transform.position.z);
                    blueBullet.transform.rotation = transform.rotation;
                    blueBullet.transform.rotation *= Quaternion.Euler(0, i * 10, 0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * 10;
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 5f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_LEFT;
                    blueBullet.SetActive(true);
                }

                if(i == 0)
                {
                    SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_BOMBING_RUN_TOUCHDOWN);

                    if(bossAIScript.playBossAttackingAnimation == true)
                    {
                        bossAIScript.bossAnimator.Play("BossAttackAnimation", -1, 0.0f);

                        bossAIScript.playBossAttackingAnimation = false;
                    }
                }
            }
            currentBulletPattern = BulletPatternType.TURNING_RIGHT;
            waveCount++;
        }

        else if (currentBulletPattern == BulletPatternType.BOTH_TURNS)
        {
            for (int i = 0; i < 36; i++)
            {
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
                bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

                if (redBullet != null)
                {
                    redBullet.transform.position = new Vector3(transform.position.x, bulletStandardHeight, transform.position.z);
                    redBullet.transform.rotation = transform.rotation;
                    redBullet.transform.rotation *= Quaternion.Euler(0, i * 10, 0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * 10;
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 5f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_RIGHT;
                    redBullet.SetActive(true);
                }

                GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); // Non-Rhythm Bullet (Non-Rhythm Bullet - Actual)
                bulletPattern = (BulletPattern)blueBullet.GetComponent(typeof(BulletPattern));

                if (blueBullet != null)
                {
                    blueBullet.transform.position = new Vector3(transform.position.x, bulletStandardHeight, transform.position.z);
                    blueBullet.transform.rotation = transform.rotation;
                    blueBullet.transform.rotation *= Quaternion.Euler(0, i * 10, 0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * 10;
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 5f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_LEFT;
                    blueBullet.SetActive(true);
                }

                if(i == 0)
                {
                    SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_BOMBING_RUN_TOUCHDOWN);

                    if(bossAIScript.playBossAttackingAnimation == true)
                    {
                        bossAIScript.bossAnimator.Play("BossAttackAnimation", -1, 0.0f);

                        bossAIScript.playBossAttackingAnimation = false;
                    }
                }
            }

            waveCount = 0;
            currentBulletPattern = BulletPatternType.REST;
        }

        //2nd stage bullet pattern
        else if (currentBulletPattern == BulletPatternType.BOMBING_RUN)
        {
            float randomAngle = Random.Range(-180, 180);
            GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); // Non-Rhythm Bullet (Non-Rhythm Bullet - Actual)

            bulletPattern = (BulletPattern)blueBullet.GetComponent(typeof(BulletPattern));

            if (blueBullet != null)
            {
                blueBullet.transform.position = transform.position;
                blueBullet.transform.rotation = transform.rotation;
                blueBullet.transform.rotation *= Quaternion.Euler(0, randomAngle, 0);
                bulletPattern.isBomb = true;
                bulletPattern.turningAngle = Random.Range(80, 100);
                bulletPattern.smoothing = 2f;
                bulletPattern.selfDestructTimer = 10f;
                bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.STRAIGHT;
                blueBullet.SetActive(true);

                blueBullet.GetComponent<BulletPattern>().playBulletDroppingSound = true;

                if(bossAIScript.playBossAttackingAnimation == true)
                {
                    bossAIScript.bossAnimator.Play("BossAttackAnimation", -1, 0.0f);

                    bossAIScript.playBossAttackingAnimation = false;
                }
            }

            currentBulletPattern = BulletPatternType.REST;
        }

        else if (currentBulletPattern == BulletPatternType.CIRCLE_RAIN)
        {
            for (int i = 0; i < 18; i++)
            {
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
                bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

                if (redBullet != null)
                {
                    redBullet.transform.position = transform.position;
                    redBullet.transform.rotation = transform.rotation;
                    redBullet.transform.rotation *= Quaternion.Euler(0, i * 20, 0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.selfDestructTimer = 6f;
                    bulletPattern.rainFallStopTimer = 1.75f - (0.25f * waveCount);
                    bulletPattern.rainFallTimer = 2.25f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.RAIN;
                    redBullet.SetActive(true);

                    if(i == 0)
                    {
                        redBullet.GetComponent<BulletPattern>().isCircleRainTriggerSound = true;
                        SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_BOMBING_RUN_TOUCHDOWN);
                    }
                }
            }
            waveCount++;
        }

        //Third Stage Bullet
        if(currentBulletPattern == BulletPatternType.CONE_SHOT || (currentBulletPattern == BulletPatternType.ULTIMATE_ATTACK && ultimate3 == true))
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
                bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

                if (redBullet != null)
                {
                    redBullet.transform.position = transform.position;
                    redBullet.transform.rotation = transform.rotation;
                    redBullet.transform.rotation *= Quaternion.Euler(i * 36, 0, 0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.selfDestructTimer = 6f;
                    bulletPattern.aimPlayerTimer = 1f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.AIM_PLAYER;
                    redBullet.SetActive(true);

                    if(i == 9)
                    {
                        redBullet.GetComponent<BulletPattern>().isConeShotTrigger = true;
                    }
                }
            }

            SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_BOMBING_RUN_TOUCHDOWN);
            waveCount++;
        }

        #region Commented #2 -- Moved to its own function below
        // Ultimate Attack
        /*
        if (currentBulletPattern == BulletPatternType.ULTIMATE_ATTACK && ultimate1 == true)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject redBullet2 = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
                bulletPattern = (BulletPattern)redBullet2.GetComponent(typeof(BulletPattern));

                if (redBullet2 != null)
                {
                    redBullet2.transform.position = player.transform.position;
                    redBullet2.transform.rotation = transform.rotation;
                    redBullet2.transform.rotation *= Quaternion.Euler(0, i * 45, 0);
                    switch (i)
                    {
                        case 0:
                            redBullet2.transform.localPosition += new Vector3(0.0f, 0.0f, 5.0f);
                            break;

                        case 1:
                            redBullet2.transform.localPosition += new Vector3(3.5f, 0.0f, 3.5f);
                            break;

                        case 2:
                            redBullet2.transform.localPosition += new Vector3(5.0f, 0.0f, 0.0f);
                            break;

                        case 3:
                            redBullet2.transform.localPosition += new Vector3(3.5f, 0.0f, -3.5f);
                            break;

                        case 4:
                            redBullet2.transform.localPosition += new Vector3(0.0f, 0.0f, -5.0f);
                            break;

                        case 5:
                            redBullet2.transform.localPosition += new Vector3(-3.5f, 0.0f, -3.5f);
                            break;

                        case 6:
                            redBullet2.transform.localPosition += new Vector3(-5.0f, 0.0f, 0.0f);
                            break;

                        case 7:
                            redBullet2.transform.localPosition += new Vector3(-3.5f, 0.0f, 3.5f);
                            break;
                    }



                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.selfDestructTimer = 6f;
                    bulletPattern.aimPlayerTimer = 0.05f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.AIM_PLAYER;
                    redBullet2.SetActive(true);
                }
            }
            vortexWaveCount++;
        }
        */
        #endregion

        #region Commented #3 -- Moved to its own function below
        /*
        if(currentBulletPattern == BulletPatternType.ULTIMATE_ATTACK && ultimate2 == true)
        {
            if(!dropSuperMegaUltraDeathBomb)
            {
                float randomAngle = Random.Range(-180,180);
                GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); //Non rhythm Bullet Please

                bulletPattern = (BulletPattern)blueBullet.GetComponent(typeof(BulletPattern));

                if(blueBullet != null)
                {
                    blueBullet.transform.position = transform.position;
                    blueBullet.transform.rotation = transform.rotation;
                    blueBullet.transform.rotation *= Quaternion.Euler(0,randomAngle,0);
                    bulletPattern.isBomb = true;
                    bulletPattern.turningAngle = Random.Range(80,100);
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 10f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.STRAIGHT;
                    blueBullet.SetActive(true);

                    blueBullet.GetComponent<BulletPattern>().playBulletDroppingSound = true;
                }
            }

            else
            {
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); //Non rhythm Bullet Please
                bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

                if(redBullet != null)
                {
                    redBullet.transform.position = transform.position;
                    redBullet.transform.localRotation = Quaternion.Euler(90,0,0);
                    bulletPattern.isSuperUltraMegaDeathBomb = true;
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.selfDestructTimer = 20f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.STRAIGHT;
                    redBullet.SetActive(true);

                    redBullet.GetComponent<BulletPattern>().playBulletDroppingSound = true;
                }
            }
        }
        */
        #endregion

        //Forth Stage Bullet
        else if (currentBulletPattern == BulletPatternType.WAIL_OF_THE_BANSHEE)
        {
            float rotatingAngle = wailOfTheBansheeTurning;
            //float randomShot = wailOfTheBansheeRandomShotTimer;

            for (int i = 0; i < 8; i++)
            {
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
                redBullet.GetComponent(typeof(BulletPattern));
                if (redBullet != null)
                {
                    redBullet.transform.position = transform.position;
                    redBullet.transform.rotation = transform.rotation;
                    redBullet.transform.rotation *= Quaternion.Euler(0, (i * 45) + rotatingAngle, 0);
                    redBullet.GetComponent<BulletPattern>().bulletSpeed = 10f;
                    redBullet.GetComponent<BulletPattern>().selfDestructTimer = 2f;
                    redBullet.GetComponent<BulletPattern>().currentBulletPattern = BulletPattern.BulletPatternType.STRAIGHT;

                    redBullet.SetActive(true);
                }
            }
        }

        else if (currentBulletPattern == BulletPatternType.WAIL_RANDOM_SHOT)
        {


            for (int i = 0; i < 16; i++)
            {
                float randomAngle = Random.Range(-180, 180);
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
                redBullet.GetComponent(typeof(BulletPattern));
                if (redBullet != null)
                {
                    redBullet.transform.position = transform.position;
                    redBullet.transform.rotation = transform.rotation;
                    redBullet.transform.rotation *= Quaternion.Euler(0, randomAngle, 0);
                    redBullet.GetComponent<BulletPattern>().bulletSpeed = 10f;
                    redBullet.GetComponent<BulletPattern>().selfDestructTimer = 2f;
                    redBullet.GetComponent<BulletPattern>().currentBulletPattern = BulletPattern.BulletPatternType.STRAIGHT;

                    redBullet.SetActive(true);
                }
            }
        }

        /*
        void function(GameObject bulletType)
        {
            bulletPattern = (BulletPattern)bulletType.GetComponent(typeof(BulletPattern));
            bulletType.transform.position = transform.position;
            bulletType.transform.rotation = transform.rotation;
            bulletType.transform.rotation *= Quaternion.Euler(0, i * 10, 0);
            bulletPattern.bulletSpeed = 10f;
            bulletPattern.turningAngle = i * 10;
            bulletPattern.smoothing = 2f;
            bulletPattern.selfDestructTimer = 5f;
            bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_LEFT;
            bulletType.SetActive(true);
        }
        */
    }


    void UltimatePatternOne() // CHAOS_VORTEX
    // Moved this to its own function to allow simultaneous execution with the other patterns
    {
        for(int a = 0; a < 8; a++)
        {
            GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
            bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

            if(redBullet != null)
            {
                redBullet.transform.position = new Vector3(player.transform.position.x, bulletStandardHeight, player.transform.position.z);
                redBullet.transform.rotation = transform.rotation;
                redBullet.transform.rotation *= Quaternion.Euler(0, a * 45, 0);

                switch(a)
                {
                    case 0:
                        redBullet.transform.localPosition += new Vector3(0.0f,0.0f,5.0f);
                        break;

                    case 1:
                        redBullet.transform.localPosition += new Vector3(3.5f,0.0f,3.5f);
                        break;

                    case 2:
                        redBullet.transform.localPosition += new Vector3(5.0f,0.0f,0.0f);
                        break;

                    case 3:
                        redBullet.transform.localPosition += new Vector3(3.5f,0.0f,-3.5f);
                        break;

                    case 4:
                        redBullet.transform.localPosition += new Vector3(0.0f,0.0f,-5.0f);
                        break;

                    case 5:
                        redBullet.transform.localPosition += new Vector3(-3.5f,0.0f,-3.5f);
                        break;

                    case 6:
                        redBullet.transform.localPosition += new Vector3(-5.0f,0.0f,0.0f);
                        break;

                    case 7:
                        redBullet.transform.localPosition += new Vector3(-3.5f,0.0f,3.5f);
                        break;
                }



                bulletPattern.bulletSpeed = 10f;
                bulletPattern.selfDestructTimer = 6f;
                bulletPattern.aimPlayerTimer = 0.05f;
                bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.AIM_PLAYER;
                redBullet.SetActive(true);

                if(a == 0) // Activates the Audio Source component for only one Bullet and plays the 'Touchdown' sound
                {
                    redBullet.GetComponent<BulletPattern>().playBulletTouchdownSound = true;

                    if(bossAIScript.ultimateMusicHasStarted == false)
                    {
                        if(bossAIScript.developmentSettingsEnabled == true)
                        {
                            SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_ULTIMATE_ATTACK);
                        }
                        else
                        {
                            SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_2_LOOP);
                        }
                        
                        SoundManagerScript.mInstance.bgmAudioSource.loop = true;
                        SoundManagerScript.mInstance.bgmAudioSource.volume = 1.0f;

                        bossAIScript.ultimateMusicHasStarted = true;
                    }
                }
            }
        }

        vortexWaveCount++;
    }


    void UltimatePatternTwo() // SUPER_MEGA_ULTRA_DEATH_BOMB
    // Moved this to its own function to allow simultaneous execution with the other patterns 
    {
        if(!dropSuperMegaUltraDeathBomb)
        {
            float randomAngle = Random.Range(-180,180);
            GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); //Non rhythm Bullet Please

            bulletPattern = (BulletPattern)blueBullet.GetComponent(typeof(BulletPattern));

            if(blueBullet != null)
            {
                blueBullet.transform.position = transform.position;
                blueBullet.transform.rotation = transform.rotation;
                blueBullet.transform.rotation *= Quaternion.Euler(0,randomAngle,0);
                bulletPattern.isBomb = true;
                bulletPattern.turningAngle = Random.Range(80,100);
                bulletPattern.smoothing = 2f;
                bulletPattern.selfDestructTimer = 10f;
                bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.STRAIGHT;
                blueBullet.SetActive(true);

                blueBullet.GetComponent<BulletPattern>().playBulletDroppingSound = true;

                if(bossAIScript.playBossAttackingAnimation == true)
                {
                    bossAIScript.bossAnimator.Play("BossAttackAnimation", -1, 0.0f);

                    bossAIScript.playBossAttackingAnimation = false;
                }
            }
        }

        else
        {
            GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); //Non rhythm Bullet Please
            bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

            if(redBullet != null)
            {
                redBullet.transform.position = transform.position;
                redBullet.transform.localRotation = Quaternion.Euler(90,0,0);
                bulletPattern.isSuperUltraMegaDeathBomb = true;
                bulletPattern.bulletSpeed = 10f;
                bulletPattern.selfDestructTimer = 20f;
                bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.STRAIGHT;
                redBullet.SetActive(true);

                redBullet.GetComponent<BulletPattern>().playBulletDroppingSound = true;

                if(bossAIScript.playBossAttackingAnimation == true)
                {
                    bossAIScript.bossAnimator.Play("BossAttackAnimation", -1, 0.0f);

                    bossAIScript.playBossAttackingAnimation = false;
                }
            }
        }
    }
}
