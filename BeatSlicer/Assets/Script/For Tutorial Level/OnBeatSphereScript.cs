using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBeatSphereScript : MonoBehaviour {

    public GameObject bulletDestroyedVFX;
    public PlayerModelScript player;
    public bool active;
    Renderer rend;
    SphereCollider collider;

    private void Start()
    {
        active = true;
        rend = GetComponent<Renderer>();
        collider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if (!active)
        {
            rend.enabled = false;
            collider.enabled = false;
            StartCoroutine(ReActivate());
        }
    }



    IEnumerator ReActivate()
    {
        active = true;    
        yield return new WaitForSeconds(1.5f);
        rend.enabled = true;
        collider.enabled = true;    
    }
}
