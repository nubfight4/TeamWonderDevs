using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShootingCollisionBoxScript : MonoBehaviour
{
    public GameObject bulletDispenser;
    private bool combatTutorialHasStarted = false;
    TutorialBulletDispenserScript tutorialBulletDispenserScript;

    void Start()
    {
        tutorialBulletDispenserScript = bulletDispenser.GetComponent<TutorialBulletDispenserScript>();
    }


    void Update()
    {
		
	}


    void OnTriggerEnter(Collider other)
    {
        if(combatTutorialHasStarted == false)
        {
            if(other.tag == "Player")
            {
                tutorialBulletDispenserScript.canShoot = true;
                combatTutorialHasStarted = true;
            }
        }
    }
}
