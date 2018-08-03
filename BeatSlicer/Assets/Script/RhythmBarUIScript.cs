using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBarUIScript : MonoBehaviour
{
    Animator animator;

    public bool offbeatHit;
    public bool rhythmBarHit;
    public float animationTime;
    public float rhythmBarTimer;
    public float perfectBeats;

    public float bonusTimer;
    public float bonusTime;
    public PlayerController playerController;
    public PlayerModelScript playerModelScript;

    public bool isPlayerAttacking;

    void Start()
    {
        perfectBeats = 0.0f;
        offbeatHit = false;
        rhythmBarHit = false;
        rhythmBarTimer = 0.0f;
        animator = GetComponent<Animator>();
        isPlayerAttacking = false;
    }

    void Update()
    {
        //Check if player is attacking
        if (Input.GetButtonDown("Attack"))
        {
            {
                isPlayerAttacking = true;
                rhythmBarTimer = animationTime;
            }
        }

        //Check if 10 perfect beats
        if (perfectBeats >= 10)
        {
            perfectBeats = 0;
            playerModelScript.charge += 5;
        }

        //Timer
        if (rhythmBarTimer <= 0)
        {
            rhythmBarTimer = 0;
        }
        else
        {
            rhythmBarTimer -= Time.deltaTime;
        }

        if (rhythmBarTimer <= 0)
        {
            isPlayerAttacking = false;
        }
    }

    public void RhythmBarHit()
    {
        if (isPlayerAttacking)
        {
            rhythmBarHit = true;
            perfectBeats++;
            animator.Play("RhythmBarHit");
        }
    }

    public void OffbeatHit()
    {
        if (isPlayerAttacking)
        {
            offbeatHit = true;
            perfectBeats = 0;
            animator.Play("RhythmBarMiss");
        }
    }

    public void SetUIActive()
    {
        gameObject.SetActive(false);
    }
}
