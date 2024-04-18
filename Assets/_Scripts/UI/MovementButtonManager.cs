using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementButtonManager : MonoBehaviour
{
    [SerializeField] Button _upButton;
    [SerializeField] Button _downButton;
    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;

    UIInputManager _uiInputManager;

    void Start()
    {
        // Get the UIInputManager instance
        _uiInputManager = UIInputManager.Instance;

        // Check if the UIInputManager is null
        if (_uiInputManager == null)
        {
            Debug.LogError("UIInputManager is null");
            return;
        }

        // Subscribe to the button click events
        _upButton?.onClick.AddListener(() => _uiInputManager.MoveUp());
        _downButton?.onClick.AddListener(() => _uiInputManager.MoveDown());
        _leftButton?.onClick.AddListener(() => _uiInputManager.MoveLeft());
        _rightButton?.onClick.AddListener(() => _uiInputManager.MoveRight());

        // Subscribe to the button click feedback events
        _upButton?.onClick.AddListener(ButtonClickFeedback);
        _downButton?.onClick.AddListener(ButtonClickFeedback);
        _leftButton?.onClick.AddListener(ButtonClickFeedback);
        _rightButton?.onClick.AddListener(ButtonClickFeedback);
    }

    void ButtonClickFeedback() => AudioManager.Instance.PlayAudio2D(EAudioEvent.ButtonClick);
}
