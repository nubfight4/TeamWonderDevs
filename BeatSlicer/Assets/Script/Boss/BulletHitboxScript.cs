using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitboxScript : MonoBehaviour {

    public GameObject bulletDestroyedVFX;
    public BulletPattern bulletPatternScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            Instantiate(bulletDestroyedVFX);
            bulletPatternScript.tempSelfDestructTimer = 0f;
        }
    }
}
