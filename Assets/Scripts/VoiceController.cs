using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using UnityEngine.Android;
using System.Security.Permissions;
using System.Security;

public class VoiceController : MonoBehaviour
{

    const string LANG_CODE = "en-US";

    [SerializeField]
    Text uiText;

      void Start()
        {
            Setup(LANG_CODE);

        SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;

        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        }

    #region Speech to Text

    void CheckPermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone)){
            Permission.RequestUserPermission(Permission.Microphone);
        }
            
    }

    public void StartListening()
    {
        SpeechToText.instance.StartRecording();
    }

    public void StopListening()
    {
        SpeechToText.instance.StopRecording();
    }

    void OnFinalSpeechResult(string result)
    {
        uiText.text = result;
    }

    void OnPartialSpeechResult(string result)
    {
        uiText.text = result;
    }

    #endregion


  

    void Setup(string code)
    {
        SpeechToText.instance.Setting(code);
    }

}
