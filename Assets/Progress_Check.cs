using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FAST;
using UnityEngine.UI;

public class Progress_Check : MonoBehaviour
{
    [SerializeField]
    ActivityScreenManager screenManager;
    [SerializeField] Image progress;
    [SerializeField] AudioSource audioPlayer;
    [SerializeField] AudioClip chime;
    void Start()
    {
        
    }

   public IEnumerator Progress()
    {
        screenManager.progressDuration = chime.length * 1000;
        if (screenManager.gotAnswerRight)
        {
            progress.fillAmount += .333f;
        }
        else
        {
            progress.fillAmount += .1665f;

        }
        audioPlayer.clip = chime;
        audioPlayer.Play();

        yield return new WaitWhile(() => audioPlayer.isPlaying);
    }
}
