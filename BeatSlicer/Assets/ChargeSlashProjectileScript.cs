using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSlashProjectileScript : MonoBehaviour
{
    public PlayerModelScript player;
    public BossModelScript boss;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "BossHitbox")
        {
            boss.health-= 2;
        }
    }
}
