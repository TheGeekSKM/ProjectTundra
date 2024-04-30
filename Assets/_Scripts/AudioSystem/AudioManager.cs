using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioEventSO[] audioEvents;
    [SerializeField] AudioPrefabController _audioUIPrefab;
    [SerializeField] AudioPrefabController _audioSFXPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayAudio2D(EAudioEvent audioEvent, bool isUI = false)
    {
        foreach (AudioEventSO audioEventSO in audioEvents)
        {
            if (audioEventSO.audioEvent == audioEvent && audioEventSO.audioClip != null)
            {
                if (isUI)
                {
                    Instantiate(_audioUIPrefab).Init(audioEventSO.audioClip);
                }
                else
                {
                    Instantiate(_audioSFXPrefab).Init(audioEventSO.audioClip);
                }
                return;
            }
        }

        Debug.LogError("AudioEvent not found");
    }

    public void PlayAudio3D(EAudioEvent audioEvent, Vector3 position, bool isUI = false)
    {
        foreach (AudioEventSO audioEventSO in audioEvents)
        {
            if (audioEventSO.audioEvent == audioEvent && audioEventSO.audioClip != null)
            {
                if (isUI)
                {
                    Instantiate(_audioUIPrefab).Init(audioEventSO.audioClip, position);
                }
                else
                {
                    Instantiate(_audioSFXPrefab).Init(audioEventSO.audioClip, position);
                }
                return;
            }
        }

        Debug.LogError("AudioEvent not found");
    }
    
}
