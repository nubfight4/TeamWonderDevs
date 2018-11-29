using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float attackTimer;
    private float attackTimeCounter;
    public float gravityScale;

    public Transform pivot;
    public GameObject playerModel;
    public PlayerModelScript player;
    private Animator anim;

    void Update()
    {

        //Move the player in different direction based on camera
        /*
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0f, moveInput.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
        */

        //Rhythm Bar Functions
        /*
        if (rhythmBarUIScript.rhythmBarHit)
        {
            moveSpeed = setBonusSpeed;
            bonusTime = bonusTimer;
            rhythmBarUIScript.rhythmBarHit = false;
        }

        else if (rhythmBarUIScript.offbeatHit)
        {
            moveSpeed = setOffbeatSpeed;
            bonusTime = bonusTimer;
            rhythmBarUIScript.offbeatHit = false;
        }

        bonusTime -= Time.deltaTime;

        if (bonusTime <= 0)
        {
            bonusTime = 0;
            moveSpeed = originalSpeed;
        }
        */
    }

}
