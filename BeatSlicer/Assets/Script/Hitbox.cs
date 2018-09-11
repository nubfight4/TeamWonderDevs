using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

    public PlayerModelScript player;
    public GameObject playerDamagedVFX;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Rhythm Bullet" && !player.isPlayerAttacking)
        {
            SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_PLAYER_HIT_BY_BULLET);
            Instantiate(playerDamagedVFX, new Vector3(transform.position.x,transform.position.y * 3/4, transform.position.z), transform.rotation, transform.parent);
            player.health--;
        }
    }
}
