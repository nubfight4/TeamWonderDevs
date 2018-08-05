using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSlashProjectileScript : MonoBehaviour
{
    public float scaleSpeed;
    public float timeToDestroy;
    float timer = 0.0f;

    private void Start()
    {
        timer = timeToDestroy;
    }

    private void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }


        transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
    }
}
