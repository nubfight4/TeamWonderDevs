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
    Animator animator;

    public Image healthBar;
    public Image chargeBar;

    // Use this for initialization
    void Start() {
        maxHealth = 10;
        health = maxHealth;
        charge = 0;
        maxCharge = 5;

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
    }

    void OnCollisionEnter(Collider collision)
    {
        if (collision.tag == "Bullet Red" || collision.tag == "Bullet Green" || collision.tag == "Bullet Blue")
        {
            health--;
        }
    }
}
