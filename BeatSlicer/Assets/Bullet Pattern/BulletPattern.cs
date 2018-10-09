using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPattern : MonoBehaviour {
    public enum BulletType
    {
        RED_BULLET = 0,
        BLUE_BULLET,
        GREEN_BULLET,
        SET_TYPE
    };

    public enum BulletPatternType
    {
        TURNING_RIGHT = 0,
        TURNING_LEFT,
        STRAIGHT,
        RAIN,
        AIM_PLAYER,
        SET_TYPE
    };


    public GameObject player;

    public Transform bossShootingSpot;

    public float bulletSpeed = 10.0f;
    public float selfDestructTimer = 5.0f;
    private float tempSelfDestructTimer;

    private Rigidbody bulletRigidbody;
    private Transform bulletTransform;

    public BulletType bulletType = BulletType.SET_TYPE;
    public bool isToBeDestroyed = false;
    public bool isBomb = false;
    [SerializeField]
    public bool bounceWall = false;
    public bool isSuperUltraMegaDeathBomb = false;
    bool rainFall = false;
    bool fallen = false;
    bool aimed = false;
    public bool stop = false;

    public Vector3 m_EulerAngleVelocity;
    public float turningAngle;
    public float smoothing;
    public float rainFallTimer;
    public float rainFallTimerCountdown;
    public float rainFallStopTimer;
    public float rainFallStopTimerCountdown;
    public float aimPlayerTimer;
    public float aimPlayerCountdown;

    public BulletPatternType currentBulletPattern = BulletPatternType.SET_TYPE;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletTransform = GetComponent<Transform>();
        tempSelfDestructTimer = selfDestructTimer; 
    }


    void OnEnable()
    {
        //selfDestructTimer = tempSelfDestructTimer;
        //bulletRigidbody.velocity = transform.forward * bulletSpeed;
        //bulletRigidbody = turningSpeed;

        //bulletRigidbody.velocity = transform.forward * bulletSpeed;
        //bulletYRotation = bulletTransform.rotation.y;

    }


    void Update()
    {
        bulletBaseScriptUpdate();

        if (currentBulletPattern == BulletPatternType.TURNING_RIGHT)
        {
            turningAngle += Time.deltaTime * 40;
            Quaternion targetAngle = Quaternion.Euler(0, turningAngle, 0);
            bulletTransform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * smoothing);
            bulletRigidbody.velocity = transform.forward * bulletSpeed;
        }

        else if (currentBulletPattern == BulletPatternType.TURNING_LEFT)
        {
            turningAngle -= Time.deltaTime * 40;
            Quaternion targetAngle = Quaternion.Euler(0, turningAngle, 0);
            bulletTransform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * smoothing);
            bulletRigidbody.velocity = transform.forward * bulletSpeed;
        }

        else if (currentBulletPattern == BulletPatternType.STRAIGHT)
        {
            turningAngle -= Time.deltaTime * 40;
            Quaternion targetAngle = Quaternion.Euler(turningAngle, 0, turningAngle);
            bulletTransform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * smoothing);
            bulletRigidbody.velocity = transform.forward * bulletSpeed;
        }

        else if (currentBulletPattern == BulletPatternType.RAIN)
        {
            if (fallen == false)
            {
                rainFallTimerCountdown += Time.deltaTime;

                if (stop == false)
                {
                    rainFallStopTimerCountdown += Time.deltaTime;
                    bulletRigidbody.velocity = transform.forward * bulletSpeed;
                }

                else
                {
                    bulletRigidbody.velocity = Vector3.zero;
                }

                if (rainFallStopTimerCountdown >= rainFallStopTimer)
                {
                    stop = true;
                }
            }

            if (rainFallTimerCountdown >= rainFallTimer)
            {
                fallen = true;
                transform.rotation = Quaternion.Euler(90, 0, 0);
                bulletRigidbody.velocity = transform.forward * bulletSpeed;
            }
        }

        else if(currentBulletPattern == BulletPatternType.AIM_PLAYER)
        {
            bulletRigidbody.velocity = transform.forward * bulletSpeed;

            if (aimed == false)
            {
                aimPlayerCountdown += Time.deltaTime;

                if(aimPlayerCountdown >= aimPlayerTimer)
                {
                    transform.LookAt(player.transform);
                    aimed = true;
                }
            }
        }
    }


    public void bulletBaseScriptUpdate()
    {
        selfDestructFunction();

        if (isToBeDestroyed == true)
        {
            gameObject.SetActive(false);
            isToBeDestroyed = false;
        }
    }


    void selfDestructFunction()
    {
        if (gameObject.activeSelf == true)
        {
            selfDestructTimer -= Time.deltaTime;

            if (selfDestructTimer <= 0.0f)
            {
                rainFallTimerCountdown = 0f;
                rainFallStopTimerCountdown = 0f;
                aimPlayerCountdown = 0.000f;
                fallen = false;
                stop = false;
                aimed = false;
                bounceWall = false;
                currentBulletPattern = BulletPatternType.SET_TYPE;
                isToBeDestroyed = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.activeSelf == true)
        {
            if (bulletType != BulletType.BLUE_BULLET) // Test Undestroyable Blue Bullet
            {
                if (other.tag == "PlayerHitbox" || other.tag == "Sword")
                {
                    selfDestructTimer = 0f;
                }
            }

            if(other.tag == "PlaneHitbox" && isBomb)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);

                //insert bomb script
                for (int i = 0; i < 8; i++)
                {
                    GameObject redBullet = ObjectPooler.Instance.getPooledObject("Bullet Red");
                    redBullet.GetComponent(typeof(BulletPattern));
                    if(redBullet != null)
                    {
                        redBullet.transform.position = transform.position;
                        redBullet.transform.rotation = transform.rotation;
                        redBullet.transform.rotation *= Quaternion.Euler(0, i * 45, 0);
                        redBullet.GetComponent<BulletPattern>().bulletSpeed = 10f;
                        redBullet.GetComponent<BulletPattern>().selfDestructTimer = 10f;
                        redBullet.GetComponent<BulletPattern>().bounceWall = true;
                        redBullet.GetComponent<BulletPattern>().currentBulletPattern = BulletPatternType.STRAIGHT;
                        
                        redBullet.SetActive(true);
                    }
                }

                isBomb = false;
                selfDestructTimer = 0f;
            }

            if (other.tag == "Wall" && bounceWall)
            {
                //fix
                currentBulletPattern = BulletPatternType.AIM_PLAYER;
                aimPlayerTimer = 0.1f;
                bounceWall = false;
            }

            if (other.tag == "PlaneHitbox" && isSuperUltraMegaDeathBomb)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);

                //insert bomb script
                //1st wave
                for (int i = 0; i < 16; i++)
                {
                    GameObject redBullet = ObjectPooler.Instance.getPooledObject("Bullet Red");
                    redBullet.GetComponent(typeof(BulletPattern));
                    if (redBullet != null)
                    {
                        redBullet.transform.position = transform.position;
                        redBullet.transform.rotation = transform.rotation;
                        redBullet.transform.rotation *= Quaternion.Euler(0, i * 22.5f, 0);
                        redBullet.GetComponent<BulletPattern>().bulletSpeed = 15f;
                        redBullet.GetComponent<BulletPattern>().selfDestructTimer = 5f;
                        //redBullet.GetComponent<BulletPattern>().turningAngle = i * 22.5f;
                        //redBullet.GetComponent<BulletPattern>().smoothing = 2f;
                        currentBulletPattern = BulletPatternType.STRAIGHT;
                        redBullet.SetActive(true);
                    }
                }

                //2nd wave
                for (int i = 0; i < 24; i++)
                {
                    GameObject redBullet = ObjectPooler.Instance.getPooledObject("Bullet Red");
                    redBullet.GetComponent(typeof(BulletPattern));
                    if (redBullet != null)
                    {
                        redBullet.transform.position = transform.position;
                        redBullet.transform.rotation = transform.rotation;
                        redBullet.transform.rotation *= Quaternion.Euler(0, i * 15, 0);
                        redBullet.GetComponent<BulletPattern>().bulletSpeed = 12f;
                        //redBullet.GetComponent<BulletPattern>().turningAngle = i * 15;
                        //.GetComponent<BulletPattern>().smoothing = 2f;
                        redBullet.GetComponent<BulletPattern>().selfDestructTimer = 5f;
                        currentBulletPattern = BulletPatternType.STRAIGHT;
                        redBullet.SetActive(true);
                    }
                }

                //3rd wave
                for (int i = 0; i < 32; i++)
                {
                    GameObject redBullet = ObjectPooler.Instance.getPooledObject("Bullet Red");
                    redBullet.GetComponent(typeof(BulletPattern));
                    if (redBullet != null)
                    {
                        redBullet.transform.position = transform.position;
                        redBullet.transform.rotation = transform.rotation;
                        redBullet.transform.rotation *= Quaternion.Euler(0, i * 11.25f, 0);
                        redBullet.GetComponent<BulletPattern>().bulletSpeed = 10f;
                        //redBullet.GetComponent<BulletPattern>().turningAngle = i * 11.25f;
                        //redBullet.GetComponent<BulletPattern>().smoothing = 2f;
                        redBullet.GetComponent<BulletPattern>().selfDestructTimer = 5f;
                        currentBulletPattern = BulletPatternType.STRAIGHT;
                        redBullet.SetActive(true);
                    }
                }

                isSuperUltraMegaDeathBomb = false;
                //turningAngle = 0f;
                //smoothing = 0f;
                selfDestructTimer = 0f;
            }

        }
    }

}
