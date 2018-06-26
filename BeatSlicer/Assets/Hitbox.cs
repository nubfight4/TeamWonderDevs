using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    public PlayerModelScript player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Bullet Red" || collision.tag == "Bullet Green" || collision.tag == "Bullet Blue")
        {
            player.health--;
        }
    }
}
