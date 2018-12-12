using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerModelScript : MonoBehaviour {

    //Player Variables
    public float health;
    public float charge;
    public float maxHealth;
    public float maxCharge;
    public float moveSpeed;
    public float gravityScale;
    public float attackSpeedTimer;

    float countdown;
    public bool isPlayerAttacking = false;
    public bool isPlayerChargeSlashing = false;
    public bool isPlayerDamaged = false;
    Animator animator;
    float originalSpeed;
    private Vector3 moveInput;

    //Gameobjects
    private CharacterController characterController;
    GameObject chargeSlashAura;
    GameObject rhythmBarUI;
    public Image healthBar;
    public Image chargeBar;
    public GameObject chargeSlashProjectile;
    public GameObject playerDamagedVFX;
    public SceneTransitionScript sceneTransitionScript;

    //Rhythm Bar variables
    // bonusTimer is to adjust how long the timer is
    public bool onBeat;
    public bool missBeat;
    public bool swordOnBeat;
    public bool swordMissBeat;
    float onBeatCharge;
    public float bonusTimer;
    public float bonusTime;
    public float setBonusSpeed = 15;
    public float setOffbeatSpeed = 7;

    void Start() {
        health = maxHealth;
        chargeSlashAura = GameObject.FindGameObjectWithTag("ChargeSlashAura");
        animator = GetComponent<Animator>();
        originalSpeed = moveSpeed;
        characterController = GetComponent<CharacterController>();
        rhythmBarUI = GameObject.FindGameObjectWithTag("Rhythm Bar");
    }

    void Update() {
        healthBar.fillAmount = health / maxHealth;
        chargeBar.fillAmount = charge / maxCharge;
        animator.SetBool("isPlayerDamaged", isPlayerDamaged);
        animator.SetBool("isPlayerAttacking", isPlayerAttacking);
        animator.SetBool("isPlayerChargeSlashing", isPlayerChargeSlashing);
        animator.SetFloat("VelX", Input.GetAxis("Horizontal"));
        animator.SetFloat("VelY",Input.GetAxis("Vertical"));
        animator.SetFloat("playerHealth", health);

        if (isPlayerDamaged)
        {
            isPlayerDamaged = false;
            animator.Play("DamagedAnimationClip");
        }

        #region Movement Function
        if(health >= 1 && !isPlayerAttacking && !isPlayerChargeSlashing)
        {
            float yStore = moveInput.y;
            moveInput = (transform.forward * Input.GetAxisRaw("Vertical") * moveSpeed) + (transform.right * Input.GetAxisRaw("Horizontal") * moveSpeed);
            moveInput = moveInput.normalized * moveSpeed;
            moveInput.y = yStore;

            if (characterController.isGrounded)
            {
                moveInput.y = 0f;
            }

            moveInput.y = moveInput.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
            characterController.Move(moveInput * Time.deltaTime);
        }
        #endregion

        #region Attack Function
        if (Input.GetButtonDown("Attack") && !isPlayerChargeSlashing && health > 0 && Time.timeScale != 0)
        {
            {
                animator.Play("AttackAnimationClip");
                countdown = attackSpeedTimer;
                isPlayerAttacking = true;
            }
        }
        #endregion

        #region Rhythm Bar Functions
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

        if (onBeatCharge >= 5)
        {
            charge++;
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
        #endregion

        #region Charge Slash Function
        if (charge >= maxCharge)
        {
            chargeSlashAura.SetActive(true);
            charge = maxCharge;
        }
        else
        {
            chargeSlashAura.SetActive(false);
        }

        if (Input.GetButton("ChargeSlash") && charge >= maxCharge && health > 0 && Time.timeScale != 0)
        {
            {
                isPlayerChargeSlashing = true;
                animator.Play("CrescendoAnimationClip");                    
            }
        }
        #endregion
    }

    public void ChargeSlashAnim()
    {
        if(health>0)
        {
            SoundManagerScript.mInstance.PlaySFX(AudioClipID.SFX_CHARGE_SLASH);
            Instantiate(chargeSlashProjectile, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f));
            charge = 0;
        }     
    }

    public void StopChargeSlashAnim()
    {
        isPlayerChargeSlashing = false;
    }

    public void StopPlayerDamagedAnim()
    {
        isPlayerDamaged = false;
    }

    public void StopAttackAnim()
    {
        isPlayerAttacking = false;
    }

    public void DeathTransition()
    {
        sceneTransitionScript.gameObject.SetActive(true);
        sceneTransitionScript.sceneName = "Defeat Screen";
        sceneTransitionScript.startTransit = true;
        SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_LOSE_SCENE); // Play Lose BGM? <- Possible? 15-10-2018
    }
}
