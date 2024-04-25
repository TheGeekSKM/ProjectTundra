using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] Button _button;

    void Awake()
    {
        if (_button == null) _button = GetComponent<Button>();
    }
    void Start()
    {
        _button.onClick.AddListener(() => AudioManager.Instance.PlayAudio2D(EAudioEvent.ButtonClick));
    } 
}
