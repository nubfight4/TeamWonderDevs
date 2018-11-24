using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBulletScript : MonoBehaviour {

    public Transform PlayerModel;
    public GameObject RhythmBar;

    private Animator anim;

    private float time;
    public bool beatCheck;
    public bool beatSwitch;

    private void Start()
    {
        //beatSwitch is to check if bullet is onbeat/offbeat. Onbeat = true, Offbeat = false
        beatSwitch = false;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        time = RhythmManager.mInstance.time;
        beatCheck = RhythmManager.mInstance.beatCheck;

        anim.SetFloat("RhythmTimeTicker", time);
        anim.SetBool("RhythmBeatCheck", beatCheck);
        
        float distance = Vector3.Distance(PlayerModel.transform.position, transform.position);

        if (distance < 2.5f)
        {
            RhythmBar.SetActive(true);
        }
        else
        {
            RhythmBar.SetActive(false);
        }
	}
}
