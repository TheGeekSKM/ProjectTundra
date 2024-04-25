using System;
using TMPro;
using UnityEngine;

public class TextCutsceneController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textComponent;

    [Header("Typing Settings")]
    public TypewriterSettings settings = new TypewriterSettings();
    private float originDelayBetweenChars;
    private bool lastCharPunctuation = false;
    private char charComma;
    private char charPeriod;
    [SerializeField] StringSO story;

    [Header("Audio Settings")]
    [Tooltip("When true requires AudioSource on this object.")]
    public bool useAudio = true;
    [Range(0f, 2f)]
    public float volume = .3f;
    [Tooltip("GameObject with AudioSource component.")]
    public GameObject AudioTyping;
    private AudioSource TypingFX;

    [Header("Extra Settings")]
    [SerializeField] bool _clearOnStart = true;
    [SerializeField] bool _clearOnFinish = true;

    Coroutine _typingCoroutine;

    public UnityEngine.Events.UnityEvent OnTypingFinished;

    void Awake()
    {
        if (useAudio)
        {
            TypingFX = GetComponent<AudioSource>();
            TypingFX.clip = AudioTyping.GetComponent<AudioSource>().clip;
        }

        if (_textComponent == null) _textComponent = GetComponent<TextMeshProUGUI>();
        originDelayBetweenChars = settings.delayBetweenChars;

        charComma = Convert.ToChar(44);
        charPeriod = Convert.ToChar(46);
    }

    void Start()
    {
        if (_clearOnStart && _textComponent) _textComponent.text = "";

        StartTyping(story.value);
    }

    public void StartTyping(string text)
    {
        story.value = text;
        _typingCoroutine = StartCoroutine(TypeText());
    }

    public void StopTyping()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }
        
        if (useAudio) TypingFX.Stop();
        _textComponent.text = story.value;

        Finished();
    }

    private System.Collections.IEnumerator TypeText()
    {
        yield return new WaitForSeconds(settings.delayToStart);
        foreach (char c in story.value)
        {
            settings.delayBetweenChars = originDelayBetweenChars;

            if (lastCharPunctuation)  //If previous character was a comma/period, pause typing
            {
                if (useAudio) TypingFX.Pause();
                yield return new WaitForSeconds(settings.delayBetweenChars = settings.delayAfterPunctuation);
                lastCharPunctuation = false;
            }

            if (c == charComma || c == charPeriod)
            {
                if (useAudio) TypingFX.Pause();
                lastCharPunctuation = true;
            }

            if (useAudio) TypingFX.PlayOneShot(TypingFX.clip, volume);
            _textComponent.text += c;
            yield return new WaitForSeconds(settings.delayBetweenChars);
        }

        if (useAudio) TypingFX.Stop();

        yield return new WaitForSeconds(settings.delayAfterTyping);
        if (_clearOnFinish) _textComponent.text = "";

        Finished();
    }

    void Finished()
    {
        OnTypingFinished?.Invoke();
        var sceneFSM = SceneController.Instance.SceneFSM;
        sceneFSM.ChangeState(sceneFSM.MainMenuState);
    }

}


[Serializable]
public class TypewriterSettings
{
    public float delayToStart;
    public float delayBetweenChars;
    public float delayAfterPunctuation;
    public float delayAfterTyping;

    public TypewriterSettings()
    {
        delayToStart = 0.5f;
        delayBetweenChars = 0.05f;
        delayAfterPunctuation = 0.1f;
        delayAfterTyping = 1f;
    }
}

