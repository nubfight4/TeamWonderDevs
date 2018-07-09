using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShootingScript : MonoBehaviour {

    public enum BulletPatternType
    {
        TURNING_RIGHT = 0,
        TURNING_LEFT,
        BOTH_TURNS,
        REST,
        SET_TYPE
    };

    float timer;
    float timerCountdown;
    float rest;
    float restCountdown;
    float waveCount;
    public BulletPattern bulletPattern;

    BulletPatternType currentBulletPattern = BulletPatternType.SET_TYPE;

    // Use this for initialization
    void Start () {
        timer = 1f;
        waveCount = 0;
        rest = 2f;
	}
	
	// Update is called once per frame
	void Update () {
        timerCountdown += Time.deltaTime;

        if(currentBulletPattern != BulletPatternType.REST)
        {
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

        else if(currentBulletPattern == BulletPatternType.REST)
        {
            restCountdown += Time.deltaTime;
            if(restCountdown >= rest)
            {
                restCountdown = 0;
                currentBulletPattern = BulletPatternType.TURNING_RIGHT;
            }
        }
	}

    public void SetBulletPattern()
    {
        if (currentBulletPattern == BulletPatternType.SET_TYPE)
        {
            currentBulletPattern = BulletPatternType.TURNING_RIGHT;
        }

        if (currentBulletPattern == BulletPatternType.TURNING_RIGHT)
        {
            for (int i = 0; i < 36; i++)
            {
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Bullet Red");
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
                GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Bullet Blue");
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
                GameObject redBullet = ObjectPooler.Instance.getPooledObject("Bullet Red");
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

                GameObject blueBullet = ObjectPooler.Instance.getPooledObject("Bullet Blue");
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
    }
}
