using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioEventSO[] audioEvents;
    [SerializeField] AudioPrefabController audioPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayAudio2D(EAudioEvent audioEvent)
    {
        foreach (AudioEventSO audioEventSO in audioEvents)
        {
            if (audioEventSO.audioEvent == audioEvent && audioEventSO.audioClip != null)
            {
                Instantiate(audioPrefab).Init(audioEventSO.audioClip);
                return;
            }
        }

        Debug.LogError("AudioEvent not found");
    }

    public void PlayAudio3D(EAudioEvent audioEvent, Vector3 position)
    {
        foreach (AudioEventSO audioEventSO in audioEvents)
        {
            if (audioEventSO.audioEvent == audioEvent && audioEventSO.audioClip != null)
            {
                Instantiate(audioPrefab).Init(audioEventSO.audioClip, position);
                return;
            }
        }

        Debug.LogError("AudioEvent not found");

    }
}
