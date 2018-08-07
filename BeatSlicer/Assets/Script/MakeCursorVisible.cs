using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCursorVisible : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
	}
}
