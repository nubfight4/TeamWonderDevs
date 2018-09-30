using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour {
    public RawImage rawImage;
    public VideoPlayer videoPlayer;

	// Use this for initialization
	void Start () {
        StartCoroutine(PlayVideo());
	}
	
    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.3f);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
