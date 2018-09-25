using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // bonusTimer is to adjust how long the timer is
    public float bonusTimer = 2;
    public float bonusTime;
    float originalSpeed;
    public float setBonusSpeed = 15;
    public float setOffbeatSpeed = 7;

    public float moveSpeed;
    public float attackTimer;
    private float attackTimeCounter;
    public float rotationSpeed;
    public float gravityScale;

    public bool onBeat;
    public bool missBeat;
    public bool swordOnBeat;
    public bool swordMissBeat;
    float onBeatCharge;

    public Transform pivot;

    //public RhythmBarUIScript rhythmBarUIScript;
    public GameObject playerModel;
    public PlayerModelScript player;

    //private Transform player;
    private CharacterController characterController;
    private Animator anim;

    //private bool isPlayerMoving;
    //private bool isPlayerAttacking;

    private Vector3 moveInput;

    // Use this for initialization
    void Start()
    {
        GameObject rhythmBarUI = GameObject.FindGameObjectWithTag("Rhythm Bar");
        //rhythmBarUIScript = rhythmBarUI.GetComponent<RhythmBarUIScript>();
        originalSpeed = moveSpeed;

        //player = GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();

        onBeat = false;
        missBeat = false;
    }

    // Update is called once per frame
    void Update()
    {
        //isPlayerMoving = false;

        //New Rhythm Bar Functions
        if (swordOnBeat)
        {
            moveSpeed = setBonusSpeed;
            bonusTime = bonusTimer;
            onBeatCharge++;
            swordOnBeat = false;
        }

        if (swordMissBeat)
        {
            moveSpeed = setOffbeatSpeed;
            bonusTime = bonusTimer;
            onBeatCharge = 0;
            swordMissBeat = false;
        }

        if(onBeatCharge >= 5)
        {
            player.charge++;
            onBeatCharge = 0;
        }

        if (bonusTime >= 0)
        {
            bonusTime -= Time.deltaTime;
        }

        else
        {
            moveSpeed = originalSpeed;
        }

        float yStore = moveInput.y;
        moveInput = (transform.forward * Input.GetAxisRaw("Vertical") * moveSpeed) + (transform.right * Input.GetAxisRaw("Horizontal") * moveSpeed);
        moveInput = moveInput.normalized * moveSpeed;
        moveInput.y = yStore;

        if(characterController.isGrounded)
        {
            moveInput.y = 0f;
        }

        moveInput.y = moveInput.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        characterController.Move(moveInput * Time.deltaTime);

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
