using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBGM : MonoBehaviour
{
    public BossAIScript bossAIScript;
    private Scene currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene();

        if(currentSceneName.name == "Tutorial Level Scene")
        {
            SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_INGAME_1); // Temporary Music Until Decided
            SoundManagerScript.mInstance.bgmAudioSource.loop = true;
            SoundManagerScript.mInstance.bgmAudioSource.volume = 1.0f;
        }
        else if(currentSceneName.name == "MainMenu")
        {
            SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_MAIN_MENU);
            SoundManagerScript.mInstance.bgmAudioSource.loop = true;
            SoundManagerScript.mInstance.bgmAudioSource.volume = 1.0f;
        }
        else if(currentSceneName.name == "BeatSlicerTestScene")
        {
            SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_1_INTRO);
            SoundManagerScript.mInstance.bgmAudioSource.loop = false;
        }
        else
        {
            SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_INGAME_1); // Temporary Music Until Decided
            SoundManagerScript.mInstance.bgmAudioSource.loop = true;
            SoundManagerScript.mInstance.bgmAudioSource.volume = 1.0f;
        }
    }


    void Update()
    {
        currentSceneName = SceneManager.GetActiveScene();

        if(SoundManagerScript.mInstance.bgmAudioSource.isPlaying == false && currentSceneName.name == "BeatSlicerTestScene")
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
            else if((bossAIScript.currentMovementPattern == BossAIScript.MovementPattern.MOVE_PATTERN_3A || bossAIScript.currentMovementPattern == BossAIScript.MovementPattern.MOVE_PATTERN_3B) && bossAIScript.currentMovementPattern != BossAIScript.MovementPattern.BOSS_STUN)
            {
                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_3_LOOP);
                SoundManagerScript.mInstance.bgmAudioSource.loop = true;
            }
            else if(bossAIScript.currentMovementPattern == BossAIScript.MovementPattern.BOSS_ULTIMATE_PHASE && bossAIScript.currentMovementPattern != BossAIScript.MovementPattern.BOSS_STUN)
            {
                SoundManagerScript.mInstance.PlayBGM(AudioClipID.BGM_SECTION_4_LOOP);
                SoundManagerScript.mInstance.bgmAudioSource.loop = true;
            }
        }
    }
}
