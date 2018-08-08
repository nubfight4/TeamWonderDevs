using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerModelScript : MonoBehaviour {
    //Delete this script when done and unnecessary, better to keep one player script only if possible. - Kevin

    public float health;
    public float charge;
    public float maxHealth;
    public float maxCharge;

    public float attackSpeedTimer;
    public float countdown;

    public bool isPlayerAttacking = false;
    public bool isPlayerChargeSlashing = false;
    Animator animator;

    GameObject chargeSlashAura;
    public Image healthBar;
    public Image chargeBar;
    public GameObject chargeSlashProjectile;

    // Use this for initialization
    void Start() {
        health = maxHealth;
        chargeSlashAura = GameObject.FindGameObjectWithTag("ChargeSlashAura");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        healthBar.fillAmount = health / maxHealth;
        chargeBar.fillAmount = charge / maxCharge;
      
        #region Attack Function
        animator.SetBool("isPlayerAttacking", isPlayerAttacking);
        animator.SetBool("isPlayerChargeSlashing", isPlayerChargeSlashing);

        if(countdown <= 0)
        {
            countdown = 0;
        }
        else
        {
            isPlayerAttacking = false;
            countdown -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Attack") && countdown <= 0)
        {
            {
                countdown = attackSpeedTimer;
                isPlayerAttacking = true;
            }
        }

        if (Input.GetButtonUp("Attack"))
        {
            {
                countdown = 0;
                isPlayerAttacking = false;
            }
        }

        if(health <= 0)
        {
            SceneManager.LoadScene("Defeat Screen");
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

        if (Input.GetButton("ChargeSlash") && charge >= maxCharge)
        {
            {
                SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_CHARGE_SLASH);
                Instantiate(chargeSlashProjectile, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), Quaternion.Euler(0.0f, 0.0f, 0.0f));
                charge = 0;            
            }
        }
        #endregion
    }
}
