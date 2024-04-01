using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFSM : BaseStateMachine
{
    [SerializeField] SceneController _sceneController;

    public MainMenuState MainMenuState { get; private set; }

    void OnValidate()
    {
        if (_sceneController == null)
        {
            _sceneController = GetComponent<SceneController>();
        }
    }

    void Awake()
    {
        MainMenuState = new MainMenuState(_sceneController, this);
    }

    void Start()
    {
        ChangeState(MainMenuState, 0.5f, 0f);
    }
}
