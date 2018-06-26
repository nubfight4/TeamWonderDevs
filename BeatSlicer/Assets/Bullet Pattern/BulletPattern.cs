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

    public GameObject player;

    public float bulletSpeed = 10.0f;
    public float selfDestructTimer = 5.0f;
    private float tempSelfDestructTimer;

    private Rigidbody bulletRigidbody;
    private Transform bulletTransform;

    public BulletType bulletType = BulletType.SET_TYPE;
    public bool isToBeDestroyed = false;

    public Vector3 m_EulerAngleVelocity;
    public float turningAngle;
    public float smoothing;

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

        turningAngle += Time.deltaTime * 40;
        Quaternion targetAngle = Quaternion.Euler(0, turningAngle, 0);
        bulletTransform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * smoothing);
        bulletRigidbody.velocity = transform.forward * bulletSpeed;
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
                    isToBeDestroyed = true;
                }
            }
        }
    }

}
