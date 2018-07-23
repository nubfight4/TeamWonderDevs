using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : MonoBehaviour {

    public GameObject ArenaSlice;
    public GameObject ArenaSlicePointLight;

    // Use this for initialization
    void Awake () {
        for (float i=0.0f; i<12.0f;i+=2)
        {
            Instantiate(ArenaSlicePointLight, new Vector3(0.0f,33.0f,0.0f), Quaternion.Euler(-90.0f, i * 30.0f, 0.0f), transform);
        }

        for (float i = 0.0f; i < 12.0f; i++)
        {
            Instantiate(ArenaSlice, new Vector3(0.0f, 33.0f, 0.0f), Quaternion.Euler(-90.0f, i * 30.0f, 0.0f), transform);
        }


        /*for (float i = 0.0f; i < 12.0f; i++)
        {
            Instantiate(ArenaSlice, new Vector3(
                Mathf.Cos(i* 180.0f ) * Mathf.PI / 90.0f * radius, 
                16.7f, 
                Mathf.Sin(i * 180.0f ) * Mathf.PI / 90.0f * radius), 
                Quaternion.Euler(-90.0f, i * 30.0f, 0.0f), transform);
        }*/
    }
}
