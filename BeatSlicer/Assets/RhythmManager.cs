using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MonoBehaviour {
    #region Singleton
    public static RhythmManager mInstance;

    private void Awake()
    {
        mInstance = this;
    }
    #endregion Singleton

    public float time;
    public float bpm = 1;
    public float minReq;
    public float maxReq;
    float beatTimer;

    public PlayerModelScript playerModel;

    float bpmMultipler;

    bool timeTicker = true;

    //float timeCheck;

    //public Slider testSlider;

    // Use this for initialization
    void Start()
    {
        time = 0.5f;
        beatTimer = 0.6f;
        bpmMultipler = bpm / 60;
        //
    }

    // Update is called once per frame
    void Update()
    {
        beatsTicker();

        if (time <= 0)
        {
            timeTicker = true;
            time = 0.001f;
        }

        else if (time >= 1)
        { 
            timeTicker = false;
            time = 0.999f;
        }

        if (timeTicker)
        {
            time += bpmMultipler * Time.deltaTime;
        }

        else
        {
            time -= bpmMultipler * Time.deltaTime;
        }

        //Debug.Log(time);
        //timeCheck += Time.deltaTime;
    }

    void beatsTicker()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (time >= minReq && time <= maxReq)
            {
                playerModel.onBeat = true;
                //Debug.Log(timeCheck);
                //Debug.Log("On Beat");
                //timeCheck = 0;
            }

            else
            {
                playerModel.missBeat = true;
                //Debug.Log(timeCheck);
                //Debug.Log("Off Beat");
                //timeCheck = 0;
            }
        }
    }
}