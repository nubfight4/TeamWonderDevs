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

        REST,
        SET_TYPE
    };

    float timer;
    float timerCountdown;
    float rest;
    float restCountdown;
    float waveCount;
    float bombingTimer;
    float bombingTimerCountdown;
    float circleRainTimer;
    float circleRainTimerCountdown;
    float coneShotTimer;
    float coneShotTimerCountdown;

    public BulletPattern bulletPattern;

    public BulletPatternType currentBulletPattern = BulletPatternType.REST;

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
        timer = 1f;
        waveCount = 0;
        rest = 2f;
        bombingTimer = 0.8f;
        circleRainTimer = 0.5f;
        coneShotTimer = 0.2f;
        currentBulletPattern = BulletPatternType.TURNING_RIGHT;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBulletPattern == BulletPatternType.TURNING_LEFT || currentBulletPattern == BulletPatternType.TURNING_RIGHT)
        {
            timerCountdown += Time.deltaTime;

            if (timerCountdown >= timer)
            {
                SetBulletPattern();

                if (waveCount >= 4)
                {
                    currentBulletPattern = BulletPatternType.BOTH_TURNS;
                    SetBulletPattern();
                }
                timerCountdown = 0;
            }
        }

        else if (currentBulletPattern == BulletPatternType.BOMBING_RUN)
        {
            bombingTimerCountdown += Time.deltaTime;
            if (bombingTimerCountdown >= bombingTimer)
            {
                SetBulletPattern();
                bombingTimerCountdown = 0;
            }
        }

        else if (currentBulletPattern == BulletPatternType.CIRCLE_RAIN)
        {
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

        else if (currentBulletPattern == BulletPatternType.CONE_SHOT)
        {
            coneShotTimerCountdown += Time.deltaTime;
            if (coneShotTimerCountdown >= coneShotTimer)
            {
                SetBulletPattern();
                if (waveCount >= 5)
                {
                    //SetBulletPattern();
                    currentBulletPattern = BulletPatternType.REST;
                    SetBulletPattern();
                    waveCount = 0;
                }
                coneShotTimerCountdown = 0;
            }
        }

        else if (currentBulletPattern == BulletPatternType.REST)
        {
            // Hi! I AM REST
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
                    redBullet.transform.position = transform.position;
                    redBullet.transform.rotation = transform.rotation;
                    redBullet.transform.rotation *= Quaternion.Euler(0, i * 10, 0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * 10;
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 5f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_RIGHT;
                    redBullet.SetActive(true);
                }
            }
            currentBulletPattern = BulletPatternType.TURNING_LEFT;
            waveCount++;
        }

        else if (currentBulletPattern == BulletPatternType.TURNING_LEFT)
        {
            for (int i = 0; i < 36; i++)
            {
                GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); // BulletTest0 (Non-Rhythm Bullet - Actual)
                bulletPattern = (BulletPattern)blueBullet.GetComponent(typeof(BulletPattern));

                if (blueBullet != null)
                {
                    blueBullet.transform.position = transform.position;
                    blueBullet.transform.rotation = transform.rotation;
                    blueBullet.transform.rotation *= Quaternion.Euler(0, i * 10, 0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * 10;
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 5f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_LEFT;
                    blueBullet.SetActive(true);
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
                    redBullet.transform.position = transform.position;
                    redBullet.transform.rotation = transform.rotation;
                    redBullet.transform.rotation *= Quaternion.Euler(0, i * 10, 0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * 10;
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 5f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_RIGHT;
                    redBullet.SetActive(true);
                }

                GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); // BulletTest0 (Non-Rhythm Bullet - Actual)
                bulletPattern = (BulletPattern)blueBullet.GetComponent(typeof(BulletPattern));



                if (blueBullet != null)
                {
                    blueBullet.transform.position = transform.position;
                    blueBullet.transform.rotation = transform.rotation;
                    blueBullet.transform.rotation *= Quaternion.Euler(0, i * 10, 0);
                    bulletPattern.bulletSpeed = 10f;
                    bulletPattern.turningAngle = i * 10;
                    bulletPattern.smoothing = 2f;
                    bulletPattern.selfDestructTimer = 5f;
                    bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.TURNING_LEFT;
                    blueBullet.SetActive(true);
                }
            }

            waveCount = 0;
            currentBulletPattern = BulletPatternType.REST;
        }

        //2nd stage bullet pattern
        else if (currentBulletPattern == BulletPatternType.BOMBING_RUN)
        {
            float randomAngle = Random.Range(-180, 180);
            GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet"); // BulletTest0 (Non-Rhythm Bullet - Actual)

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
                }
            }
            waveCount++;
        }

        //Third Stage Bullet
        else if(currentBulletPattern == BulletPatternType.CONE_SHOT)
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
                }
            }
            waveCount++;
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
}
