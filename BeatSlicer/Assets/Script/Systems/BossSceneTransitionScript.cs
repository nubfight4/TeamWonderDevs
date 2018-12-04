using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneTransitionScript : MonoBehaviour {

    public SceneTransitionScript sceneTransitionScript;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BossSceneTransit()
    {

        SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_INGAME_1); // <- To Change to Win BGM in future? - 12-10-2018

        sceneTransitionScript.gameObject.SetActive(true);
        sceneTransitionScript.sceneName = "Victory Screen";
        sceneTransitionScript.startTransitWhite = true;  // Loads Win Scene
    }
}
