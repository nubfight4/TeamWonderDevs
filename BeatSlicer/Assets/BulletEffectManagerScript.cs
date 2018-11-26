using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectManagerScript : MonoBehaviour
{
    /* 
     private float wailOfTheBansheeTurning;
     private float wailOfTheBansheeTimerValue = 0.3f;
     [SerializeField]
     private float wailOfTheBansheeCounter = 0.0f;

     // Update is called once per frame
     void Update () {
         wailOfTheBansheeTurning += Time.deltaTime * 20;

         wailOfTheBansheeCounter += Time.deltaTime;

         if (wailOfTheBansheeCounter >= wailOfTheBansheeTimerValue)
         {
             WailOfTheBansheeMainFunction();

             wailOfTheBansheeCounter = 0.0f;
         }
     }

     void WailOfTheBansheeMainFunction()
     {
         float rotatingAngle = wailOfTheBansheeTurning;

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
                 redBullet.GetComponent<BulletPattern>().turningAngle = i * (360f / 8f);
                  redBullet.GetComponent<BulletPattern>().smoothing  = 2f;
                 redBullet.GetComponent<BulletPattern>().selfDestructTimer = 2f;
                 redBullet.GetComponent<BulletPattern>().currentBulletPattern = BulletPattern.BulletPatternType.TURNING_RIGHT;

                 redBullet.SetActive(true);
             }
         }
     }
     */



    private float bulletTurningTimerValue = 1f;
    private float bulletTurningCounter = 0.0f;

    private void Update()
    {
        bulletTurningCounter += Time.deltaTime;

        if (bulletTurningCounter >= bulletTurningTimerValue)
        {
            BulletTurningMainFunction();
            bulletTurningCounter = 0.0f;     
        }
    }

    void BulletTurningMainFunction()
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject redBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
            redBullet.GetComponent(typeof(BulletPattern));

            if (redBullet != null)
            {
                redBullet.transform.position = transform.position;
                redBullet.transform.rotation = transform.rotation;
                redBullet.transform.rotation *= Quaternion.Euler(0, i * (360f / 30f), 0);
                redBullet.GetComponent<BulletPattern>().bulletSpeed = 10f;
                redBullet.GetComponent<BulletPattern>().turningAngle = i * (360f / 30f);
                redBullet.GetComponent<BulletPattern>().smoothing = 2f;
                redBullet.GetComponent<BulletPattern>().selfDestructTimer = 7f;
                redBullet.GetComponent<BulletPattern>().currentBulletPattern = BulletPattern.BulletPatternType.TURNING_LEFT;
                redBullet.SetActive(true);

            }
        }
    }
}
