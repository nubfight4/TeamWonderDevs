using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCresendoCollisionBox : MonoBehaviour
{
    private GameObject player;
    public GameObject cresendoText;
    private bool cresendoHasCharged = false;
    PlayerModelScript playerModelScript;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        cresendoText.SetActive(false);
        playerModelScript = player.GetComponent<PlayerModelScript>();
    }
	

	void Update()
    {
		
	}


    void OnTriggerEnter(Collider other)
    {
        if(cresendoHasCharged == false)
        {
            if(other.tag == "Player")
            {
                cresendoText.SetActive(true);
                playerModelScript.charge = playerModelScript.maxCharge;
                cresendoHasCharged = true;
            }
        }
    }
}
