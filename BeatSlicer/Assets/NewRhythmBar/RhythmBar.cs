using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmBar : MonoBehaviour {

    public float time;
    public float bpm = 1;
    public float beats = 0;
    public float minReq;
    public float maxReq;
    float beatTimer;
    float beatCountdown;

    public Animator anim;
    public PlayerModelScript playerModel;

    float bpmMultipler;

    bool timeTicker = true;
    public bool onBeat = false;
    public bool missBeat = false;

    //public Slider testSlider;

	// Use this for initialization
	void Start () {
        time = 0.5f;
        beatTimer = 0.6f;
        bpmMultipler = bpm / 60;
        //anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        beatsTicker();

        if(time <= 0)
        {
            timeTicker = true;
        }

        else if(time >= 1)
        {
            timeTicker = false;
        }
        
        if(timeTicker)
        {
            time += bpmMultipler * Time.deltaTime;
        }
        
        else
        {
            time -= bpmMultipler * Time.deltaTime;
        }

        //testSlider.value = time;

        anim.SetFloat("RhythmTimeTicker", time);
        anim.SetBool("OnBeat", onBeat);
        anim.SetBool("MissBeat", missBeat);

        if (onBeat || missBeat)
        {
            beatCountdown += Time.deltaTime;

            if (beatCountdown >= beatTimer)
            {
                onBeat = false;
                missBeat = false;
                beatCountdown = 0;
            }
        }

    }

    void beatsTicker()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (time >= minReq && time <= maxReq)
            {
                beats++;
                onBeat = true;
                playerModel.onBeat = true;
            }

            else
            {
                beats--;
                missBeat = true;
                playerModel.missBeat = true;
            }
        }
    }
}
