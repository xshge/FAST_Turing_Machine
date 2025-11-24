using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Transition_Screen : ActivityScreen
{
    [SerializeField]
    private Image CorrectFile;
    [SerializeField]
    private TMP_Text Date;

    public string[] dates;
    override protected IEnumerator PlayScreenAnimation()
    {   //load Audio 
        audioLUT["Transition-Narration.mp3"].baseFileName = string.Format($"Transition-Narration-{screenManager.teaserIndex + 1}.mp3",
          screenManager.teaserName);
        audioLUT["Transition-Narration.mp3"].Load(FAST.Application.language);


        Date.text = dates[screenManager.teaserIndex];
        CorrectFile.enabled = true;
        CorrectFile.CrossFadeAlpha(0f, 0f, false);
        yield return null;

        CorrectFile.CrossFadeAlpha(1f, 0.5f, false);
        screenManager.gotAnswerRight = true;
        audioPlayer.Play(new AudioClip[] { audioLUT["Transition-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        screenManager.ChangeScreen("summary");

        yield break;
    }
}
