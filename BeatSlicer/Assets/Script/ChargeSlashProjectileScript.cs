using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSlashProjectileScript : MonoBehaviour
{
    public float scaleSpeed;

    private void Update()
    {
        //rigidbody.velocity = transform.right * speed;
        transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
    }
}
