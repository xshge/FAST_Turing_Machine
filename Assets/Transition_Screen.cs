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
    {
        Date.text = dates[screenManager.teaserIndex];
        CorrectFile.enabled = true;
        CorrectFile.CrossFadeAlpha(0f, 0f, false);
        yield return null;

        CorrectFile.CrossFadeAlpha(1f, 0.5f, false);
        screenManager.gotAnswerRight = true;
        yield return new WaitForSeconds(2f);
        
        screenManager.ChangeScreen("summary");

        yield break;
    }
}
