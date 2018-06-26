using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossModelScript : MonoBehaviour {
    //Delete this script when done and unnecessary, better to keep one player script only if possible. - Kevin

    public Image healthBar;

    public float health;
    private float maxHealth = 10;

    // Use this for initialization
    void Start () {
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        healthBar.fillAmount = health / maxHealth;
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "ChargeSlashProjectile")
        {
            health--;
            Destroy(collision.gameObject);
        }
    }
}
