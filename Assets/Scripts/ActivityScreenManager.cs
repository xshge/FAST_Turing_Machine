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
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FAST;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class ActivityScreenManager : FAST.ScreenManagerTemplate<ActivityScreen>
{
    [Header("Object Exploration Settings")]
    [SerializeField]
    private string previousScreenName;
    [SerializeField]
    private ScannerManager objectScanner;

    private const int maxNumObjects = 8;
    public int numTeasers;
    public int teaserIndex;
    public string teaserName;

    public bool isShowSummary = true;
    public float progressDuration;
    public int numGuesses;
    [SerializeField]
    private int maxNumGuesses;
    public int scannedIndex; // scanned object
    public int selectedIndex; // selected object

    public UnityEvent<bool> ChangeScanAudio;
    public bool gotAnswerRight = false;
    public Progress_Check progress;
    override protected void Start()
    {
        ActivitySettings settings = FAST.Application.settings;
        numTeasers = settings.objectSettings.objectData.Length;
        if (numTeasers > maxNumObjects) {
            string logTitle = "Too many objects defined!";
            string logMessage = $"Only the first {maxNumObjects} of {numTeasers} objects listed in the object settings can be used. " +
                "This restriction is a hard-coded limitation, which requires software modifications to be changed.\n"+
                $"The object settings file can be found within the {FAST.Application.skin} folder where this application is installed: " +
                $"\n\t{FAST.Application.activityDirectory}\n\t\tAssets\n\t\t\t{FAST.Application.skin}\n\t\t\t*-Object-settings.xml";
            Debug.LogWarning($"WARNING\n{logTitle}\n{logMessage}\n");
            numTeasers = maxNumObjects;
        }
        maxNumGuesses = settings.objectSettings.maxNumGuesses;

        teaserIndex = -1;
        teaserName = "None";
        scannedIndex = 0;
        selectedIndex = 0;

        base.Start();
    }

    public void OnRepeatNarration()
    {
        screens[currentScreenName].PlayScreen();
    }

    public void OnObjectChanged(int index)
    {
        bool isScan = true;
        if (currentScreenName.Equals("reward") || currentScreenName.Equals("summary")) {
            isScan = false;
        }

        objectScanner.OnObjectChanged(index, isScan);
    }

    public void OnScanStart()
    {
        screens[currentScreenName].OnScanStart();
    }

    public void OnScanDone(int index)
    {
        scannedIndex = index;

        if (currentScreenName.Equals("start")) {


            if (scannedIndex.Equals(0)) {
                if (teaserIndex == numTeasers -1)
                {
                    ChangeScreen("end");
                }
                else { 
                    
                    ChangeScreen("teaser");
                
                }
                    
            }
            
        }
        else if (scannedIndex > 0) {
            selectedIndex = scannedIndex;
            numGuesses++;

            if (selectedIndex == (teaserIndex + 1)) {
                //change to the Transition Screen
                if (currentScreenName.Equals("hint"))
                {
                    ChangeScreen("summary");
                }
                else
                {
                    ChangeScreen("transition");
                }
            }
            else {
                if (numGuesses >= maxNumGuesses) {
                    ChangeScreen("skip");
                }
                else {
                    ChangeScreen("hint");
                }
            }
        }

        screens[currentScreenName].OnScanDone();
    }

    override public async void ChangeScreen(string newScreenName)
    {
        if (newScreenName.Equals("start") && scannedIndex.Equals(0)) {
            
            newScreenName = "teaser";
        }

        if (newScreenName.Equals("teaser")) {
            selectedIndex = scannedIndex;
            numGuesses = 0;
            isShowSummary = true;
            teaserIndex = ++teaserIndex % numTeasers; // using remainder allows the number to reach six (the max and still goes back to the zero index;
            ActivitySettings settings = FAST.Application.settings;
            teaserName = settings.objectSettings.objectData[teaserIndex].name;
        }

        if (currentScreenName != null && screens.ContainsKey(currentScreenName)) {
            if (newScreenName == "summary")
            {
                StartCoroutine(progress.Progress());
                await Task.Delay((int)progressDuration);
            }
            screens[currentScreenName].gameObject.SetActive(false);
        }

        currentScreenName = newScreenName;
        if (currentScreenName != null && screens.ContainsKey(currentScreenName)) {
          
            screens[currentScreenName].gameObject.SetActive(true);
        }


        ChangeScanAudio.Invoke(currentScreenName.Equals("teaser"));//change the name when switch to a different screen 

    }
    protected override IEnumerator ChangeLanguage()
    {
        if (currentScreenName != null && screens.ContainsKey(currentScreenName)) {
            screens[currentScreenName].gameObject.SetActive(false);
        }
        FAST.Application.ChangeLanguage(FAST.Application.ChangeLanguageMode.Next);

        if (!currentScreenName.Equals("language")) {
            previousScreenName = currentScreenName;
        }
        currentScreenName = "language";

        ActivityScreen languageScreen = screens["language"];
        languageScreen.gameObject.SetActive(true);
        yield return new WaitWhile(() => languageScreen.IsPlaying);
        languageScreen.gameObject.SetActive(false);

        currentScreenName = previousScreenName;
        if (currentScreenName != null && screens.ContainsKey(currentScreenName)) {
            screens[currentScreenName].gameObject.SetActive(true);
        }
    }
}
