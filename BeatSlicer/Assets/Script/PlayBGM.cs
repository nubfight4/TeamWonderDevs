using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour {
    /*
	void Start () {
        SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_INGAME);
	}
    */
    private void Awake()
    {
        SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_INGAME_1);
    }

    // Possibly move to thr Sound Manager in the future?
}
