using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKillSelf : MonoBehaviour {
    private ParticleSystem particleSystem;
    float duration;

	// Use this for initialization
	void Start () {
        particleSystem = GetComponent<ParticleSystem>();
        duration = particleSystem.duration + particleSystem.startLifetime;
        
    }
	
	// Update is called once per frame
	void Update () {
        Destroy(this.gameObject, duration);
    }
}
