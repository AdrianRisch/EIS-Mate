using UnityEngine;
using UnityEngine.UI;
using TextSpeech;
using UnityEngine.XR.ARFoundation;

public class SampleSpeechToText : MonoBehaviour
{
    public GameObject loading;
    public InputField inputLocale;
    public InputField outputText;
    public float pitch;
    public float rate;
    public string result;

    public Text txtLocale;
    public Text txtPitch;
    public Text txtRate;
    public Text outputter;
    void Start()
    {
        Setting("de-DE");
        loading.SetActive(false);
        SpeechToText.instance.onResultCallback = OnResultSpeech;

    }
    

    public void StartRecording()
    {
#if UNITY_EDITOR
#else
        SpeechToText.instance.StartRecording("Speak any");
#endif
    }

    public void StopRecording()
    {
#if UNITY_EDITOR
        OnResultSpeech("Not support in editor.");
#else
        SpeechToText.instance.StopRecording();
#endif
#if UNITY_IOS
        loading.SetActive(true);
#endif
    }
    void OnResultSpeech(string _data)
    {
        result = _data;
    }
    public void OnClickSpeak()
    {
        TextToSpeech.instance.StartSpeak(outputText.text);
    }
    public void  OnClickStopSpeak()
    {
        TextToSpeech.instance.StopSpeak();
    }
    public void Setting(string code)
    {
        TextToSpeech.instance.Setting(code, pitch, rate);
        SpeechToText.instance.Setting(code);
        txtLocale.text = "Locale: " + code;
        // txtPitch.text = "Pitch: " + pitch;
        // txtRate.text = "Rate: " + rate;
    }
    public void OnClickApply()
    {
        Setting(inputLocale.text);
    }
}
