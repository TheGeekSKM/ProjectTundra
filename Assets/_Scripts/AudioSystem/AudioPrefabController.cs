using UnityEngine;

public class AudioPrefabController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    ///  This method initializes the audio prefab with the audio clip and location.
    /// </summary>
    public void Init(AudioClip audioClip)
    {
        audioSource.spatialBlend = 0;

        audioSource.clip = audioClip;
        audioSource.Play();

        Destroy(gameObject, audioClip.length);
    }

    public void Init(AudioClip audioClip, Vector3 position)
    {
        audioSource.spatialBlend = 1;
        transform.position = position;

        audioSource.clip = audioClip;
        audioSource.Play();

        Destroy(gameObject, audioClip.length);
    }
}
