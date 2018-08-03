using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour {

	void Start () {
        SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_INGAME);
	}
}
