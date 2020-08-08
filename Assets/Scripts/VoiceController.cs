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

        CheckAndSetImg(leftIndex, LeftImg);
        CheckAndSetImg(rightIndex, RightImg);
    }

    void CheckPermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
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

        //CheckResults(result);
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
            CheckAndSetImg(++leftIndex, LeftImg);
        }
        else if (result == "punto blu" || result == "Punto Blu")
        {
            CheckAndSetImg(++rightIndex, RightImg);
        }
        else if (result == "leva rosso" || result == "Leva Rosso")
        {
            CheckAndSetImg(--leftIndex, LeftImg);
        }
        else if (result == "leva blu" || result == "Leva Blu")
        {
            CheckAndSetImg(--rightIndex, RightImg);
        }
    }

    void CheckAndSetImg(int index, Image image)
    {
        index = Mathf.Clamp(index, 0, 9);
        image.sprite = numbers[index];
    }
}