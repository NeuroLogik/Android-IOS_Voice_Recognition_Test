using TextSpeech;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "it";

    [SerializeField]
    Text UIText;

    public Image LeftImg, RightImg;

    public Sprite[] numbers;
    int leftIndex, rightIndex = 0;

    private void Start()
    {
        Setup(LANG_CODE);

#if PLATFORM_ANDROID
        SpeechToText.instance.onPartialResultsCallback = OnPartialFinalSpeechResult;
#endif
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.instance.onDoneCallback = OnSpeakStop;

        CheckPermission();
    }

    void CheckPermission()
    {
#if PLATFORM_ANDROID
        if(!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }

#region Text to Speech

    public void StartSpeaking(string message)
    {
        TextToSpeech.instance.StartSpeak(UIText.text);
    }

    public void StopSpeaking()
    {
        TextToSpeech.instance.StopSpeak();
    }

    void OnSpeakStart()
    {
        Debug.Log("Talking started...");
    }

    void OnSpeakStop()
    {
        Debug.Log("Talking stopped...");
    }

#endregion

#region Speech to Text

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
        UIText.text = result;

        CheckResults(result);
    }

    void OnPartialFinalSpeechResult(string result)
    {
        UIText.text = result;

        //if(result.Contains("punto rosso"))
        //{
        //    CubeLeft.transform.Translate(0f, 1f, 0f);
        //}
    }

#endregion

    void Setup(string code)
    {
        TextToSpeech.instance.Setting(code, 1, 1);
        SpeechToText.instance.Setting(code);
    }

    void CheckResults(string result)
    {
        if (result == "punto rosso" || result == "Punto Rosso")
        {
            leftIndex++;
            leftIndex = Mathf.Clamp(leftIndex, 0, 9);
            LeftImg.sprite = numbers[leftIndex];
        }

        if (result == "punto blu" || result == "Punto Blu")
        {
            rightIndex++;
            rightIndex = Mathf.Clamp(rightIndex, 0, 9);
            RightImg.sprite = numbers[rightIndex];
        }

        if (result == "leva rosso" || result == "Leva Rosso")
        {
            leftIndex--;
            leftIndex = Mathf.Clamp(leftIndex, 0, 9);
            LeftImg.sprite = numbers[leftIndex];
        }

        if (result == "leva blu" || result == "Leva Blu")
        {
            rightIndex--;
            rightIndex = Mathf.Clamp(rightIndex, 0, 9);
            RightImg.sprite = numbers[rightIndex];
        }
    }
}
