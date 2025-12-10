//=============================================================================
// FAST Object Investigation
//
// A FAST digital exhibit experience that allows for exploration of real
// objects or physical models, identification or classification, and
// learning about a variety of items.
//
// Copyright (C) 2024 Museum of Science, Boston
// <https://www.mos.org/>
//
// This software was developed through a grant to the Museum of Science, Boston
// from the Institute of Museum and Library Services under
// Award #MG-249646-OMS-21. For more information about this grant, see
// <https://www.imls.gov/grants/awarded/mg-249646-oms-21>.
//
// This software is open source: you can redistribute it and/or modify
// it under the terms of the MIT License.
//
// This software is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// MIT License for more details.
//
// You should have received a copy of the MIT License along with this software.
// If not, see <https://opensource.org/license/MIT>.
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FAST;

public class TeaserScreen : ActivityScreen
{
    [Header("Images")]
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Image question2Image;
    [SerializeField]
    private Image DialogueImage;

    private ImageFromFile backgroundFromFile;
    private ImageFromFile question2ImageFromFile;
    private ImageFromFile dialogueImageFromFile;

    override protected void Awake()
    {
        base.Awake();

        question2ImageFromFile = question2Image.GetComponent<ImageFromFile>();
        dialogueImageFromFile = DialogueImage.GetComponent<ImageFromFile>();
        backgroundFromFile = backgroundImage.GetComponent<ImageFromFile>();
    }


    override protected IEnumerator PlayScreenAnimation()
    {   
        // Load and initialize

        audioLUT["Teaser-Dialogue-1-Narration.mp3"].baseFileName = string.Format("Teaser-Dialogue-1-{0}-Narration.mp3",
            screenManager.teaserName);
        audioLUT["Teaser-Dialogue-1-Narration.mp3"].Load(FAST.Application.language);

        audioLUT["Teaser-Question-2-Narration.mp3"].baseFileName = string.Format("Teaser-Question-2-{0}-Narration.mp3",
            screenManager.teaserName);
        audioLUT["Teaser-Question-2-Narration.mp3"].Load(FAST.Application.language);

        audioLUT["Teaser-Dialogue-3-Narration.mp3"].baseFileName = string.Format("Teaser-Dialogue-3-{0}-Narration.mp3",
            screenManager.teaserName);
        audioLUT["Teaser-Dialogue-3-Narration.mp3"].Load(FAST.Application.language);

        //images

        backgroundFromFile.baseFileName = string.Format("Teaser-Background-{0}.png", screenManager.teaserName);
        backgroundFromFile.Load(FAST.Application.language);
        backgroundImage.CrossFadeAlpha(0f, 0f, false);
        

        question2ImageFromFile.baseFileName = string.Format("Teaser-Line-2-{0}.png", screenManager.teaserName);
        question2ImageFromFile.Load(FAST.Application.language);
        question2Image.CrossFadeAlpha(0f, 0f, false);

        dialogueImageFromFile.baseFileName = string.Format("Teaser-Line-{0}.png", screenManager.teaserName);
        dialogueImageFromFile.Load(FAST.Application.language);
        DialogueImage.CrossFadeAlpha(0f, 0f, false);

        

        // Animate
        yield return null;

        yield return new WaitForSecondsRealtime(0.25f);

        backgroundImage.CrossFadeAlpha(1f, 0.5f, false);

        audioPlayer.Play(new AudioClip[] { audioLUT["Teaser-Dialogue-1-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);


        DialogueImage.CrossFadeAlpha(1f, 0.5f, false);
        audioPlayer.Play(new AudioClip[] { audioLUT["Teaser-Question-2-Narration.mp3"].audioClip });
        yield return new WaitWhile(() => audioPlayer.IsRunning);

        question2Image.CrossFadeAlpha(1f, 0.5f, false);
        audioPlayer.Play(new AudioClip[] { audioLUT["Teaser-Dialogue-3-Narration.mp3"].audioClip });
        
        yield return new WaitWhile(() => audioPlayer.IsRunning);
        if (screenManager.teaserName == "TAP") yield return new WaitForSeconds(1.5f);


        //TODO: change screen to the prompt screen;
        screenManager.ChangeScreen("prompt");
    }

 
}
