using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : MonoBehaviour {

    public GameObject ArenaSlice;

	// Use this for initialization
	void Awake () {
        for (float i = 0.0f; i < 12.0f; i++)
        {
            Instantiate(ArenaSlice, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(-90.0f,i * 30.0f,0.0f));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
