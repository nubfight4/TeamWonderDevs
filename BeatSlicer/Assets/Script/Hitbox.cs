using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    public PlayerModelScript player;

	//Just put this in bullet script - Kevin.

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Bullet Red" || collision.tag == "Bullet Green" || collision.tag == "Bullet Blue")
        {
            player.health--;
        }
    }
}
