using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioEventSO[] audioEvents;
    [SerializeField] AudioPrefabController audioPrefab;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PlayAudio2D(EAudioEvent audioEvent)
    {
        foreach (AudioEventSO audioEventSO in audioEvents)
        {
            if (audioEventSO.audioEvent == audioEvent)
            {
                Instantiate(audioPrefab).Init(audioEventSO.audioClip);
                return;
            }
        }
    }

    public void PlayAudio3D(EAudioEvent audioEvent, Vector3 position)
    {
        foreach (AudioEventSO audioEventSO in audioEvents)
        {
            if (audioEventSO.audioEvent == audioEvent)
            {
                Instantiate(audioPrefab).Init(audioEventSO.audioClip, position);
                return;
            }
        }
    }
}