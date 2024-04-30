using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioEventSO[] tracks;
    [SerializeField] private AudioSource _audioSource1;
    [SerializeField] private AudioSource _audioSource2;
    bool _isPlaying1 = false;
    AudioEventSO _currentTrack;
    Coroutine _crossfadeCoroutine;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        if (!_audioSource1) _audioSource1 = gameObject.GetComponent<AudioSource>();
        if (!_audioSource2) _audioSource2 = gameObject.GetComponent<AudioSource>();
        _isPlaying1 = true;
    }

    public void SwapTrack(EAudioEvent newClip)
    {

        //find the requested AUdioEventSO in the tracks array
        foreach (AudioEventSO track in tracks)
        {
            if (track.audioEvent == newClip && track.audioClip != null)
            {
                _currentTrack = track;
                Debug.Log("Swapping to " + _currentTrack.audioEvent.ToString());
                
                if (_crossfadeCoroutine != null) StopCoroutine(_crossfadeCoroutine);

                _crossfadeCoroutine = StartCoroutine(Crossfade(_currentTrack.audioClip, 0.25f));
                _isPlaying1 = !_isPlaying1;
                
                return;
            }
        }

        Debug.LogWarning("Track not found");        
    }

    IEnumerator Crossfade(AudioClip newClip, float duration)
    {
        float timeElapsed = 0f;

        if (_isPlaying1)
        {
            _audioSource2.clip = newClip;
            _audioSource2.Play();
            
            while (timeElapsed < duration)
            {
                _audioSource1.volume = Mathf.Lerp(1, 0, timeElapsed / duration);
                _audioSource2.volume = Mathf.Lerp(0, 1, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            _audioSource1.Stop();
        }
        else
        {
            _audioSource1.clip = newClip;
            _audioSource1.Play();
            
            while (timeElapsed < duration)
            {
                _audioSource2.volume = Mathf.Lerp(1, 0, timeElapsed / duration);
                _audioSource1.volume = Mathf.Lerp(0, 1, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            _audioSource2.Stop();
        }
    }


}
