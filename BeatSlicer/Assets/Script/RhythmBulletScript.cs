using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBulletScript : MonoBehaviour {

    public Transform PlayerModel;
    public GameObject RhythmBar;

    private Animator anim;

    private float time;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        time = RhythmManager.mInstance.time;

        anim.SetFloat("RhythmTimeTicker", time);

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
