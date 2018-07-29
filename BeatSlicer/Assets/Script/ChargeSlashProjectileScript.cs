using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSlashProjectileScript : MonoBehaviour
{
    public float speed;
    Rigidbody rigidbody;
    public PlayerModelScript player;
    //public BossModelScript boss;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rigidbody.velocity = transform.right * speed;
    }
    /*
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "BossHitbox")
        {
            Debug.Log("hit");
            boss.health --;
            Destroy(collision.gameObject);
        }
    }
    */
}
