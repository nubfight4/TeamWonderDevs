using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMovementCollisionBoxScript : MonoBehaviour
{
    private GameObject player;
    public GameObject bulletDispenser;
    private bool combatTutorialHasStarted = false;
    public float centerpointDistanceChecker;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	void Update()
    {
        if(combatTutorialHasStarted == false)
        {
            centerpointDistanceChecker = Vector3.Distance(player.transform.position,transform.position);

            if(centerpointDistanceChecker >= 5.0f)
            {
                bulletDispenser.SetActive(true);

                combatTutorialHasStarted = true;
            }
        }
    }
}
