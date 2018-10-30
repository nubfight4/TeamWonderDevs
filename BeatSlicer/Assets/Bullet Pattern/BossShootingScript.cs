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
        TURNING_RIGHT = 0,
        TURNING_LEFT,
        BOTH_TURNS,

        REST,
        SET_TYPE
    };

    public BossAIScript bossAIScript;
    public BulletPattern bulletPattern;
    GameObject player;

    public BulletPatternType currentBulletPattern = BulletPatternType.REST;

    [Space(10)] // Just to look nice
    public float bulletStandardHeight = 2.8f;

    [Space(10)] // Just to look nice
    public bool bulletTurningInitialRun = false;
    public bool isBulletTurningActive = false;
    private float bulletTurningTimerValue = 1.0f;
    private float bulletTurningCounter = 0.0f;
    private int bulletTurningWaveCount = 0;

    public bool bombingRunInitialRun = false;
    public bool isBombingRunActive = false;
    private float bombingRunTimerValue = 1.0f;
    private float bombingRunCounter = 0.0f;

    public bool circleRainInitialRun = false;
    public bool isCircleRainActive = false;
    private float circleRainTimerValue = 0.5f;
    private float circleRainCounter = 0.0f;
    private int circleRainWaveCount = 0;

    public bool coneShotInitialRun = false;
    public bool isConeShotActive = false;
    private float coneShotTimerValue = 0.2f;
    private float coneShotCounter = 0.0f;
    private int coneShotWaveCount = 0;

    public bool chaosVortexInitialRun = false;
    public bool isChaosVortexActive = false;
    private float chaosVortexTimerValue = 3.0f;
    private float chaosVortexCounter = 0.0f;

    public bool superMegaUltraDeathBombInitialRun = false;
    public bool isSuperMegaUltraDeathBombActive = false;
    private float superBombTimerValue = 0.4f;
    private float superBombCounter = 0.0f;
    private float superMegaUltraDeathBombTimerValue = 4.0f;
    private float superMegaUltraDeathBombCounter = 0.0f;
    private bool dropSuperMegaUltraDeathBomb;

    private float wailOfTheBansheeTurning;

    public bool wailOfTheBansheeInitialRun = false;
    public bool isWailOfTheBansheeActive = false;
    private float wailOfTheBansheeTimerValue = 0.1f;
    private float wailOfTheBansheeCounter = 0.0f;

    public bool wailOfTheBansheeRandomInitialRun = false;
    public bool isWailOfTheBansheeRandomActive = false;
    private float wailOfTheBansheeRandomTimerValue = 1.0f;
    private float wailOfTheBansheeRandomCounter = 0.0f;

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


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dropSuperMegaUltraDeathBomb = false;
        currentBulletPattern = BulletPatternType.TURNING_RIGHT;
    }


    void Update()
    {
        BulletTurningCallFunction();
        BombingRunCallFunction();
        CircleRainCallFunction();
        ConeShotCallFunction();
        ChaosVortexCallFunction();
        SuperMegaUltraDeathBombCallFunction();
        WailOfTheBansheeCallFunction();
        WailOfTheBansheeRandomCallFunction();
    }


    #region Bullet Turning Functions
    public void BulletTurningCallFunction()
    {
        if(bulletTurningInitialRun == true)
        {
            currentBulletPattern = BulletPatternType.TURNING_RIGHT;

            BulletTurningMainFunction();
            bossAIScript.playBossAttackingAnimation = true;

            isBulletTurningActive = true;
            bulletTurningInitialRun = false;
        }

        if(isBulletTurningActive == true)
        {
            bulletTurningCounter += Time.deltaTime;

            if(bulletTurningCounter >= bulletTurningTimerValue)
            {
                BulletTurningMainFunction();
                bossAIScript.playBossAttackingAnimation = true;
            }
        }
        else
        {
            bulletTurningCounter = 0.0f;
        }
    }

    void BulletTurningMainFunction()
    {
        if(currentBulletPattern == BulletPatternType.TURNING_RIGHT)
        {
            for(int i = 0; i < 45; i++)
            {
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
                bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

                if(redBullet != null)
                {
                    redBullet.transform.position = new Vector3(transform.position.x,bulletStandardHeight,transform.position.z);
                    redBullet.transform.rotation = transform.rotation;
                    redBullet.transform.rotation *= Quaternion.Euler(0,i * (360f / 45f),0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * (360f / 45f);
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 7f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_RIGHT;
                    redBullet.SetActive(true);

                    if(i == 0)
                    {
                        SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_BOMBING_RUN_TOUCHDOWN);

                        if(bossAIScript.playBossAttackingAnimation == true)
                        {
                            bossAIScript.bossAnimator.Play("BossAttackAnimation",-1,0.0f);

                            bossAIScript.playBossAttackingAnimation = false;
                        }
                    }
                }
            }

            bulletTurningWaveCount++;
            bulletTurningCounter = 0.0f;

            if(bulletTurningWaveCount >= 4)
            {
                currentBulletPattern = BulletPatternType.BOTH_TURNS;
            }
            else
            {
                currentBulletPattern = BulletPatternType.TURNING_LEFT;
            }
        }
        else if(currentBulletPattern == BulletPatternType.TURNING_LEFT)
        {
            for(int i = 0; i < 45; i++)
            {
                GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); // Non-Rhythm Bullet (Non-Rhythm Bullet - Actual)
                bulletPattern = (BulletPattern)blueBullet.GetComponent(typeof(BulletPattern));

                if(blueBullet != null)
                {
                    blueBullet.transform.position = new Vector3(transform.position.x,bulletStandardHeight,transform.position.z);
                    blueBullet.transform.rotation = transform.rotation;
                    blueBullet.transform.rotation *= Quaternion.Euler(0,i * (360f / 45f),0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * (360f / 45f);
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 7f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_LEFT;
                    blueBullet.SetActive(true);
                }

                if(i == 0)
                {
                    SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_BOMBING_RUN_TOUCHDOWN);

                    if(bossAIScript.playBossAttackingAnimation == true)
                    {
                        bossAIScript.bossAnimator.Play("BossAttackAnimation",-1,0.0f);

                        bossAIScript.playBossAttackingAnimation = false;
                    }
                }
            }

            bulletTurningWaveCount++;
            bulletTurningCounter = 0.0f;

            if(bulletTurningWaveCount >= 4)
            {
                currentBulletPattern = BulletPatternType.BOTH_TURNS;
            }
            else
            {
                currentBulletPattern = BulletPatternType.TURNING_RIGHT;
            }
        }
        else if(currentBulletPattern == BulletPatternType.BOTH_TURNS)
        {
            for(int i = 0; i < 45; i++)
            {
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
                bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

                if(redBullet != null)
                {
                    redBullet.transform.position = new Vector3(transform.position.x,bulletStandardHeight,transform.position.z);
                    redBullet.transform.rotation = transform.rotation;
                    redBullet.transform.rotation *= Quaternion.Euler(0,i * (360f / 45f),0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * (360f / 45f);
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 7f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_RIGHT;
                    redBullet.SetActive(true);
                }

                GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); // Non-Rhythm Bullet (Non-Rhythm Bullet - Actual)
                bulletPattern = (BulletPattern)blueBullet.GetComponent(typeof(BulletPattern));

                if(blueBullet != null)
                {
                    blueBullet.transform.position = new Vector3(transform.position.x,bulletStandardHeight,transform.position.z);
                    blueBullet.transform.rotation = transform.rotation;
                    blueBullet.transform.rotation *= Quaternion.Euler(0,i * (360f / 45f),0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * (360f / 45f);
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 7f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_LEFT;
                    blueBullet.SetActive(true);
                }

                if(i == 0)
                {
                    SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_BOMBING_RUN_TOUCHDOWN);

                    if(bossAIScript.playBossAttackingAnimation == true)
                    {
                        bossAIScript.bossAnimator.Play("BossAttackAnimation",-1,0.0f);

                        bossAIScript.playBossAttackingAnimation = false;
                    }
                }
            }

            bulletTurningWaveCount = 0;
            bulletTurningCounter = -1.0f;

            currentBulletPattern = BulletPatternType.TURNING_RIGHT;
        }
    }
    #endregion


    #region Bombing Run Functions
    public void BombingRunCallFunction()
    {
        if(bombingRunInitialRun == true)
        {
            if(bossAIScript.isOutside != true)
            {
                BombingRunMainFunction();
                bossAIScript.playBossAttackingAnimation = true;
            }

            isBombingRunActive = true;
            bombingRunInitialRun = false;
        }

        if(isBombingRunActive == true)
        {
            bombingRunCounter += Time.deltaTime;

            if(bombingRunCounter >= bombingRunTimerValue && bossAIScript.isOutside != true)
            {
                BombingRunMainFunction();
                bossAIScript.playBossAttackingAnimation = true;

                bombingRunCounter = 0.0f;
            }
        }
        else
        {
            bombingRunCounter = 0.0f;
        }
    }

    void BombingRunMainFunction()
    {
        float randomAngle = Random.Range(-180,180);
        GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); // Non-Rhythm Bullet (Non-Rhythm Bullet - Actual)

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
                bossAIScript.bossAnimator.Play("BossAttackAnimation",-1,0.0f);

                bossAIScript.playBossAttackingAnimation = false;
            }
        }
    }
    #endregion


    #region Circle Rain Functions
    public void CircleRainCallFunction()
    {
        if(circleRainInitialRun == true)
        {
            CircleRainMainFunction();

            isCircleRainActive = true;
            circleRainInitialRun = false;
        }

        if(isCircleRainActive == true)
        {
            circleRainCounter += Time.deltaTime;

            if(circleRainCounter >= circleRainTimerValue)
            {
                if(circleRainWaveCount >= 7)
                {
                    isCircleRainActive = false;

                    circleRainWaveCount = 0;
                }
                else
                {
                    CircleRainMainFunction();
                }

                circleRainCounter = 0.0f;
            }
        }
        else
        {
            circleRainCounter = 0.0f;
        }
    }

    void CircleRainMainFunction()
    {
        for(int i = 0; i < 27; i++)
        {
            GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
            bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

            if(redBullet != null)
            {
                redBullet.transform.position = transform.position;
                redBullet.transform.rotation = transform.rotation;
                redBullet.transform.rotation *= Quaternion.Euler(0,i * (360f / 27f),0);
                bulletPattern.bulletSpeed = 10f;
                bulletPattern.selfDestructTimer = 10f;
                bulletPattern.rainFallStopTimer = 1.75f - (0.25f * circleRainWaveCount);
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

        circleRainWaveCount++;
    }
    #endregion


    #region Cone Shot Functions
    public void ConeShotCallFunction()
    {
        if(coneShotInitialRun == true)
        {
            ConeShotMainFunction();

            isConeShotActive = true;
            coneShotInitialRun = false;
        }

        if(isConeShotActive == true)
        {
            coneShotCounter += Time.deltaTime;

            if(coneShotCounter >= coneShotTimerValue)
            {
                if(coneShotWaveCount >= 5)
                {
                    isConeShotActive = false;

                    coneShotWaveCount = 0;
                }
                else
                {
                    ConeShotMainFunction();
                }

                coneShotCounter = 0.0f;
            }
        }
        else
        {
            coneShotCounter = 0.0f;
        } 
    }

    void ConeShotMainFunction()
    {
        for(int i = 0; i < 15; i++)
        {
            GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
            bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

            if(redBullet != null)
            {
                redBullet.transform.position = transform.position;
                redBullet.transform.rotation = transform.rotation;
                redBullet.transform.rotation *= Quaternion.Euler(i * 24,0,0);
                bulletPattern.bulletSpeed = 10f;
                bulletPattern.selfDestructTimer = 10f;
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
        coneShotWaveCount++;
    }
    #endregion


    #region Chaos Vortex Functions
    public void ChaosVortexCallFunction()
    {
        if(chaosVortexInitialRun == true)
        {
            ChaosVortexMainFunction();

            isChaosVortexActive = true;
            chaosVortexInitialRun = false;
        }

        if(isChaosVortexActive == true)
        {
            chaosVortexCounter += Time.deltaTime;

            if(chaosVortexCounter >= chaosVortexTimerValue)
            {
                ChaosVortexMainFunction();

                chaosVortexCounter = 0.0f;
            }
        }
        else
        {
            chaosVortexCounter = 0.0f;
        }
    }

    void ChaosVortexMainFunction()
    {
        for(int a = 0; a < 8; a++)
        {
            GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
            bulletPattern = (BulletPattern)redBullet.GetComponent(typeof(BulletPattern));

            if(redBullet != null)
            {
                redBullet.transform.position = new Vector3(player.transform.position.x,bulletStandardHeight,player.transform.position.z);
                redBullet.transform.rotation = transform.rotation;
                redBullet.transform.rotation *= Quaternion.Euler(0,a * 45,0);

                switch(a)
                {
                    case 0:
                        redBullet.transform.localPosition += new Vector3(0.0f,0.0f,8.0f);
                        break;

                    case 1:
                        redBullet.transform.localPosition += new Vector3(6.5f,0.0f,6.5f);
                        break;

                    case 2:
                        redBullet.transform.localPosition += new Vector3(8.0f,0.0f,0.0f);
                        break;

                    case 3:
                        redBullet.transform.localPosition += new Vector3(6.5f,0.0f,-6.5f);
                        break;

                    case 4:
                        redBullet.transform.localPosition += new Vector3(0.0f,0.0f,-8.0f);
                        break;

                    case 5:
                        redBullet.transform.localPosition += new Vector3(-6.5f,0.0f,-6.5f);
                        break;

                    case 6:
                        redBullet.transform.localPosition += new Vector3(-8.0f,0.0f,0.0f);
                        break;

                    case 7:
                        redBullet.transform.localPosition += new Vector3(-6.5f,0.0f,6.5f);
                        break;
                }



                bulletPattern.bulletSpeed = 10f;
                bulletPattern.selfDestructTimer = 10f;
                bulletPattern.aimPlayerTimer = 0.05f;
                bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.AIM_PLAYER;
                redBullet.SetActive(true);

                if(a == 0) // Activates the Audio Source component for only one Bullet and plays the 'Touchdown' sound
                {
                    redBullet.GetComponent<BulletPattern>().playBulletTouchdownSound = true;
                }
            }
        }
    }
    #endregion


    #region Super Mega Ultra Death Bomb Functions
    public void SuperMegaUltraDeathBombCallFunction()
    {
        if(superMegaUltraDeathBombInitialRun == true)
        {

            isSuperMegaUltraDeathBombActive = true;
            superMegaUltraDeathBombInitialRun = false;
        }

        if(isSuperMegaUltraDeathBombActive == true) // SUPER_MEGA_ULTRA_DEATH_BOMB <- Why the fancy name? XD
        {
            superBombCounter += Time.deltaTime;
            superMegaUltraDeathBombCounter += Time.deltaTime;

            if(superBombCounter >= superBombTimerValue && superMegaUltraDeathBombCounter < superMegaUltraDeathBombTimerValue)
            {
                SuperMegaUltraDeathBombMainFunction();
                bossAIScript.playBossAttackingAnimation = true;

                superBombCounter = 0.0f;
            }

            if(superMegaUltraDeathBombCounter >= superMegaUltraDeathBombTimerValue)
            {
                dropSuperMegaUltraDeathBomb = true;

                SuperMegaUltraDeathBombMainFunction();
                bossAIScript.playBossAttackingAnimation = true;

                dropSuperMegaUltraDeathBomb = false;

                superMegaUltraDeathBombCounter = 0.0f;

                isSuperMegaUltraDeathBombActive = false;
            }
        }
        else
        {
            superBombCounter = 0.0f;
            superMegaUltraDeathBombCounter = 0.0f;
        }
    }

    void SuperMegaUltraDeathBombMainFunction()
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
                    bossAIScript.bossAnimator.Play("BossAttackAnimation",-1,0.0f);

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
                bulletPattern.selfDestructTimer = 10f;
                bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.STRAIGHT;
                redBullet.SetActive(true);

                redBullet.GetComponent<BulletPattern>().playBulletDroppingSound = true;

                if(bossAIScript.playBossAttackingAnimation == true)
                {
                    bossAIScript.bossAnimator.Play("BossAttackAnimation",-1,0.0f);

                    bossAIScript.playBossAttackingAnimation = false;
                }
            }
        }
    }
    #endregion


    #region Wail Of The Banshee Functions
    public void WailOfTheBansheeCallFunction()
    {
        if(isWailOfTheBansheeActive)
        {
            wailOfTheBansheeTurning += Time.deltaTime * 20;

            wailOfTheBansheeCounter += Time.deltaTime;

            if(wailOfTheBansheeCounter >= wailOfTheBansheeTimerValue)
            {
                WailOfTheBansheeMainFunction();

                wailOfTheBansheeCounter = 0.0f;
            }
        }
        else
        {
            wailOfTheBansheeCounter = 0.0f;
        }
    }

    void WailOfTheBansheeMainFunction()
    {
        float rotatingAngle = wailOfTheBansheeTurning;

        for(int i = 0; i < 12; i++)
        {
            GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
            redBullet.GetComponent(typeof(BulletPattern));
            if(redBullet != null)
            {
                redBullet.transform.position = transform.position;
                redBullet.transform.rotation = transform.rotation;
                redBullet.transform.rotation *= Quaternion.Euler(0,(i * 30) + rotatingAngle,0);
                redBullet.GetComponent<BulletPattern>().bulletSpeed = 10f;
                redBullet.GetComponent<BulletPattern>().selfDestructTimer = 2f;
                redBullet.GetComponent<BulletPattern>().currentBulletPattern = BulletPattern.BulletPatternType.STRAIGHT;

                redBullet.SetActive(true);
            }
        }
    }
    #endregion


    #region Wail Of The Banshee Random Functions
    public void WailOfTheBansheeRandomCallFunction()
    {
        if(isWailOfTheBansheeRandomActive == true)
        {
            wailOfTheBansheeRandomCounter += Time.deltaTime;

            if(wailOfTheBansheeRandomCounter >= wailOfTheBansheeRandomTimerValue)
            {
                WailOfTheBansheeRandomMainFunction();

                wailOfTheBansheeRandomCounter = 0.0f;
            }
        }
        else
        {
            wailOfTheBansheeRandomCounter = 0.0f;
        }
    }

    void WailOfTheBansheeRandomMainFunction()
    {
        for(int i = 0; i < 24; i++)
        {
            float randomAngle = Random.Range(-180,180);
            GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
            redBullet.GetComponent(typeof(BulletPattern));

            if(redBullet != null)
            {
                redBullet.transform.position = transform.position;
                redBullet.transform.rotation = transform.rotation;
                redBullet.transform.rotation *= Quaternion.Euler(0,randomAngle,0);
                redBullet.GetComponent<BulletPattern>().bulletSpeed = 10f;
                redBullet.GetComponent<BulletPattern>().selfDestructTimer = 2f;
                redBullet.GetComponent<BulletPattern>().currentBulletPattern = BulletPattern.BulletPatternType.STRAIGHT;

                redBullet.SetActive(true);
            }
        }
    }
    #endregion
}
