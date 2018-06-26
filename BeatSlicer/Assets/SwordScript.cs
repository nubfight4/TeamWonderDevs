using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {

    public PlayerModelScript player;
    public BossModelScript boss;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Bullet Red" || collision.tag == "Bullet Green" || collision.tag == "Bullet Blue")
        {
            player.charge++;
        }

        if (collision.tag == "BossHitbox")
        {
            boss.health --;
        }
    }
}
