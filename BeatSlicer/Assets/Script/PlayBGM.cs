using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    public BossAIScript bossAIScript;

    void Awake()
    {
        SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_1_INTRO);
        SoundManagerScript.mInstance.bgmAudioSource.loop = false;
        SoundManagerScript.mInstance.bgmAudioSource.volume = 0.0f;
    }


    void Update()
    {
        if(SoundManagerScript.mInstance.bgmAudioSource.isPlaying == false)
        {
            if(bossAIScript.currentMovementPattern == BossAIScript.MovementPattern.MOVE_PATTERN_1 && bossAIScript.currentMovementPattern != BossAIScript.MovementPattern.BOSS_STUN)
            {
                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_1_LOOP);
                SoundManagerScript.mInstance.bgmAudioSource.loop = true;
            }
            else if(bossAIScript.currentMovementPattern == BossAIScript.MovementPattern.MOVE_PATTERN_2 && bossAIScript.currentMovementPattern != BossAIScript.MovementPattern.BOSS_STUN)
            {
                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_2_LOOP);
                SoundManagerScript.mInstance.bgmAudioSource.loop = true;
            }
            else if(bossAIScript.currentMovementPattern == BossAIScript.MovementPattern.MOVE_PATTERN_3A && bossAIScript.currentMovementPattern != BossAIScript.MovementPattern.BOSS_STUN)
            {
                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_1_LOOP);
                SoundManagerScript.mInstance.bgmAudioSource.loop = true;
            }
        }
    }
}
