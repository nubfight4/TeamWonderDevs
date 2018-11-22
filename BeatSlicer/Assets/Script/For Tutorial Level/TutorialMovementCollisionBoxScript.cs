using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMovementCollisionBoxScript : MonoBehaviour
{
    private GameObject player;
    public GameObject bulletDispenser;
    private bool botTutorialHasStarted = false; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	

	void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        if(botTutorialHasStarted == false)
        {
            if(other.tag == "Player")
            {
                bulletDispenser.SetActive(true);

                botTutorialHasStarted = true;
            }
        }
    }
}
