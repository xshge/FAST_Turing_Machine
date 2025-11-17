using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FAST;
public class Prompt_Screen : ActivityScreen
{
    [SerializeField]
    private Image promptImage;
    [SerializeField]
    private Image placementPromptImage;

    private ImageFromFile promptImgFile;
    protected override void Awake()
    {
        base.Awake();
        promptImgFile = promptImage.GetComponent<ImageFromFile>();
    }

    override public void OnScanStart()
    {
        placementPromptImage.enabled = false;
    }

    override public void OnScanDone()
    {
        placementPromptImage.enabled = true;
    }

    override protected IEnumerator PlayScreenAnimation()
    {   //load image and audio
        audioLUT["Prompt-Question-Narration.mp3"].baseFileName = string.Format("Prompt-Question-Narration-{0}.mp3",
            screenManager.teaserName);
        audioLUT["Prompt-Question-Narration.mp3"].Load(FAST.Application.language);

        promptImgFile.baseFileName = string.Format("Prompt-{0}.png", screenManager.teaserName);
        promptImgFile.Load(FAST.Application.language);


        promptImage.enabled = true;
        promptImage.CrossFadeAlpha(0f, 0f, false);
        placementPromptImage.enabled=true;
        placementPromptImage.CrossFadeAlpha(0f,0f, false);

        promptImage.CrossFadeAlpha(1f, 0.5f, false);
        placementPromptImage.CrossFadeAlpha(1f, 0.5f, false);
        audioPlayer.Play(new AudioClip[] { audioLUT["Prompt-Question-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        yield break;
    }
}
