using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    void Awake()
    {
        SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_INGAME_1);
    }
}
