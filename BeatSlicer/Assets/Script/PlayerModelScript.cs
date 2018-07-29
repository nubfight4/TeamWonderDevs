using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModelScript : MonoBehaviour {
    //Delete this script when done and unnecessary, better to keep one player script only if possible. - Kevin

    public float health;
    public float charge;
    public float maxHealth;
    public float maxCharge;

    private bool isPlayerAttacking = false;
    public bool isPlayerChargeSlashing = false;
    Animator animator;

    public Image healthBar;
    public Image chargeBar;
    public GameObject chargeSlashProjectile;

    // Use this for initialization
    void Start() {
        maxHealth = 10;
        health = maxHealth;
        charge = 0;
        maxCharge = 50;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        healthBar.fillAmount = health / maxHealth;
        chargeBar.fillAmount = charge / maxCharge;

        if (charge >= maxCharge)
        {
            charge = maxCharge;
        }

        animator.SetBool("isPlayerAttacking", isPlayerAttacking);
        animator.SetBool("isPlayerChargeSlashing", isPlayerChargeSlashing);

        if (Input.GetButton("Attack"))
        {
            {
                isPlayerAttacking = true;
            }
        }

        if (Input.GetButtonUp("Attack"))
        {
            {
                isPlayerAttacking = false;
            }
        }

        if (Input.GetButton("ChargeSlash") && charge >= maxCharge)
        {
            {
                isPlayerChargeSlashing = true;
                charge = 0;            
            }
        }
    }

    public void ChargeSlash()
    {
        Instantiate(chargeSlashProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0.0f,0.0f,0.0f));
    }

    public void StopChargeSlashAnim()
    {
        if(charge < maxCharge)
        isPlayerChargeSlashing = false;
    }
    /*
    void OnCollisionEnter(Collider collision)
    {
        if (collision.tag == "Bullet Red" || collision.tag == "Bullet Green" || collision.tag == "Bullet Blue")
        {
            health--;
        }
    }
    */
}
