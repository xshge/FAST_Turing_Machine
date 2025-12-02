using FAST;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class End_Screen : ActivityScreen
{
    [Header("Video")]
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private VideoPlayerFromFile videoPlayerFromFile;
    [SerializeField]
    private RawImage videoImage;

    [SerializeField]
    GameObject tray, spotlight;
    override protected IEnumerator PlayScreenAnimation()
    {
        // Load and initialize
        videoImage.CrossFadeAlpha(0f, 0f, false);
        tray.SetActive(false);
        spotlight.SetActive(false);

        videoPlayerFromFile.baseFileName = string.Format("Closing-Video.mp4", screenManager.teaserName);
        videoPlayerFromFile.Load(FAST.Application.language);
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(0.1f);
        videoImage.CrossFadeAlpha(1f, 0.5f, false);

        yield return new WaitWhile(() => videoPlayer.isPlaying);
       
    }



}
