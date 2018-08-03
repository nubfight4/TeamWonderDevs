using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {

    public PlayerModelScript player;
    public BossModelScript boss;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Rhythm Bullet")
        {
            player.charge++;
            SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_BULLET_HIT_BY_PLAYER_ONBEAT);
        }
    }
}
