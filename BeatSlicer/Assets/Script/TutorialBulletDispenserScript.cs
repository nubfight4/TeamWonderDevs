using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBulletDispenserScript : MonoBehaviour
{
    private GameObject player;
    public float health;
    private readonly float maxHealth = 1;
    public GameObject shootingPoint;
    public BulletPattern bulletPattern;
    private float bulletSpawnTimer;

    //private float tempTimer;

    public GameObject door;
    private BoxCollider doorBoxCollider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        doorBoxCollider = door.GetComponent<BoxCollider>();
        doorBoxCollider.isTrigger = false;
        health = maxHealth;
        bulletSpawnTimer = 0.0f;
        //tempTimer = 0.0f;
    }
	

	void Update()
    {
        LookAtPlayerFunction();

        if(health <= 0)
        {
            /*
            tempTimer += Time.deltaTime;

            if(tempTimer >= 3.0f) // Temporary Pause Before Scene Change // Might Change Later
            {
                SceneManager.LoadScene("BeatSlicerTestScene"); // Loads Main Game Scene

                tempTimer = 0.0f;
            }
            */

            doorBoxCollider.isTrigger = true;
            gameObject.SetActive(false);
        }
        else
        {
            bulletSpawnTimer += Time.deltaTime;

            if(bulletSpawnTimer >= 2.0f)
            {
                CreateBullet();

                bulletSpawnTimer = 0.0f;
            }  
        }
    }


    void LookAtPlayerFunction()
    {
        transform.LookAt(player.transform);
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles = new Vector3(0,eulerAngles.y,0);
        transform.rotation = Quaternion.Euler(eulerAngles);
    }


    void CreateBullet()
    {
        GameObject tutorialBullet = ObjectPooler.Instance.getPooledObject("Rhythm Bullet");
        bulletPattern = (BulletPattern)tutorialBullet.GetComponent(typeof(BulletPattern));

        if(tutorialBullet != null)
        {
            tutorialBullet.transform.position = new Vector3(shootingPoint.transform.position.x, shootingPoint.transform.position.y, shootingPoint.transform.position.z);
            tutorialBullet.transform.rotation = transform.rotation;
            tutorialBullet.transform.rotation *= Quaternion.Euler(0 * 24,0,0);
            bulletPattern.bulletSpeed = 10f;
            bulletPattern.selfDestructTimer = 5f;
            bulletPattern.aimPlayerTimer = 0.1f;
            bulletPattern.currentBulletPattern = BulletPattern.BulletPatternType.AIM_PLAYER;
            tutorialBullet.SetActive(true);

            SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_BOMBING_RUN_TOUCHDOWN);
        }
    }


    void OnTriggerEnter(Collider other)
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
    }
}
