using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {

    public PlayerModelScript player;
    public PlayerController playerController;
    public BossAIScript boss;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Rhythm Bullet")
        {
            player.charge++;
            SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_BULLET_HIT_BY_PLAYER_ONBEAT);

            if(playerController.onBeat)
            {
                playerController.swordOnBeat = true;
                playerController.onBeat = false;
            }

            if(playerController.missBeat)
            {
                playerController.swordMissBeat = true;
                playerController.missBeat = false;
            }
        }
    }
}
