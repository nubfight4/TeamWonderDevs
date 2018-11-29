using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {
    
    public PlayerModelScript player;
    public BossAIScript boss;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "BulletHitbox")
        {
            player.charge++;
            SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_HIT_BY_PLAYER_ONBEAT);

            if (player.onBeat)
            {
                player.swordOnBeat = true;
                player.onBeat = false;
            }

            if (player.missBeat)
            {
                player.swordMissBeat = true;
                player.missBeat = false;
            }
        }
    }
    
}
