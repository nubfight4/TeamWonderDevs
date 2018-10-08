using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum AudioClipID
{
	BGM_MAIN_MENU = 0,
    BGM_INGAME_1 = 1,
    BGM_INGAME_2 = 2,
    BGM_SECTION_1_INTRO = 3,
    BGM_SECTION_1_LOOP = 4,
    BGM_SECTION_2_INTRO = 5,
    BGM_SECTION_2_LOOP = 6,

    SFX_BULLET_HIT_BY_PLAYER_ONBEAT = 101,
    SFX_BULLET_HIT_BY_PLAYER_OFFBEAT = 102,
    SFX_CHARGE_SLASH = 103,
    SFX_DETUNE = 104,
    SFX_PLAYER_HIT_BY_BULLET = 105,

    SFX_BOSS_APPEARING = 106,

    SFX_BULLET_BOMBING_RUN_DROPPING = 107,
    SFX_BULLET_BOMBING_RUN_TOUCHDOWN = 108,

    TOTAL = 9001
}

[System.Serializable]

public class AudioClipInfo
{
	public AudioClipID audioClipID;
	public AudioClip audioClip;
}

public class SoundManagerScript : MonoBehaviour 
{
    /*
	#region Singleton
	private static SoundManagerScript mInstance;

	public static SoundManagerScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				SoundManagerScript temp = ManagerControllerScript.Instance.soundManager;

				if(temp == null)
				{
					temp = Instantiate(ManagerControllerScript.Instance.soundManagerPrefab, Vector3.zero, Quaternion.identity).GetComponent<SoundManagerScript>();
				}
				mInstance = temp;
				ManagerControllerScript.Instance.soundManager = mInstance;
				DontDestroyOnLoad(mInstance.gameObject);
			}
			return mInstance;
		}
	}
	public static bool CheckInstanceExist()
	{
		return mInstance;
	}
	#endregion Singleton
    */
    public static SoundManagerScript mInstance;
    
	public float bgmVolume = 1.0f;
	public float sfxVolume = 1.0f;

	public List<AudioClipInfo> audioClipInfoList = new List<AudioClipInfo>();

	public AudioSource bgmAudioSource;
	public AudioSource sfxAudioSource;

    public List<AudioSource> sfxAudioSourceList = new List<AudioSource>();
	public List<AudioSource> bgmAudioSourceList = new List<AudioSource>();

	// Preload before any Start() rins in other scripts
	void Awake () 
	{
        /*
		if(SoundManagerScript.CheckInstanceExist())
		{
			Destroy(this.gameObject);
		}
        */

        if (mInstance == null)
        {
            DontDestroyOnLoad(gameObject);
            mInstance = this;
        }
        else if (mInstance != this)
        {
            Destroy(gameObject);
        }

        AudioSource[] audioSourceList = this.GetComponentsInChildren<AudioSource>();

		if(audioSourceList[0].gameObject.name == "BGMAudioSource")
		{
			bgmAudioSource = audioSourceList[0];
			sfxAudioSource = audioSourceList[1];
		}
		else 
		{
			bgmAudioSource = audioSourceList[1];
			sfxAudioSource = audioSourceList[0];
		}
	}

	public AudioClip FindAudioClip(AudioClipID audioClipID)
	{
		for(int i=0; i<audioClipInfoList.Count; i++)
		{
			if(audioClipInfoList[i].audioClipID == audioClipID)
			{
				return audioClipInfoList[i].audioClip;
			}
		}

		return null;
	}

	//! BACKGROUND MUSIC (BGM)
	public void PlayBGM(AudioClipID audioClipID)
	{
		bgmAudioSource.clip = FindAudioClip(audioClipID);
		bgmAudioSource.volume = bgmVolume;
		bgmAudioSource.loop = true;
		bgmAudioSource.Play();
	}

	public void PauseBGM()
	{
		if(bgmAudioSource.isPlaying)
		{
			bgmAudioSource.Pause();
		}
	}

	public void StopBGM()
	{
		if(bgmAudioSource.isPlaying)
		{
			bgmAudioSource.Stop();
		}
	}


	//! SOUND EFFECTS (SFX)
	public void PlaySFX(AudioClipID audioClipID)
	{
		sfxAudioSource.PlayOneShot(FindAudioClip(audioClipID), sfxVolume);
	}

	public void PlayLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList.Count; i++)
		{
			if(sfxAudioSourceList[i].clip == clipToPlay)
			{
				if(sfxAudioSourceList[i].isPlaying)
				{
					return;
				}

				sfxAudioSourceList[i].volume = sfxVolume;
				sfxAudioSourceList[i].Play();
				return;
			}
		}

		AudioSource newInstance = gameObject.AddComponent<AudioSource>();
		newInstance.clip = clipToPlay;
		newInstance.volume = sfxVolume;
		newInstance.loop = true;
		newInstance.Play();
		sfxAudioSourceList.Add(newInstance);
	}

	public void PauseLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToPause = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList.Count; i++)
		{
			if(sfxAudioSourceList[i].clip == clipToPause)
			{
				sfxAudioSourceList[i].Pause();
				return;
			}
		}
	}	

	public void StopLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList.Count; i++)
		{
			if(sfxAudioSourceList[i].clip == clipToStop)
			{
				sfxAudioSourceList[i].Stop();
				return;
			}
		}
	}

	public void ChangePitchLoopingSFX(AudioClipID audioClipID, float value)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		for(int i=0; i<sfxAudioSourceList.Count; i++)
		{
			if(sfxAudioSourceList[i].clip == clipToStop)
			{
				sfxAudioSourceList[i].pitch = value;
				return;
			}
		}
	}

	public void SetBGMVolume(float value)
	{
		bgmVolume = value;
		bgmAudioSource.volume = bgmVolume;
	}

	public void SetSFXVolume(float value)
	{
		sfxVolume = value;
		sfxAudioSource.volume = sfxVolume;
	}
}