using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : BaseState
{
    SceneController _sceneController;
    SceneFSM _sceneFSM;

    public MainMenuState(SceneController sceneController, SceneFSM sceneFSM)
    {
        _sceneController = sceneController;
        _sceneFSM = sceneFSM;
    }

    public override void Enter()
    {
        base.Enter();
        _sceneController.MainMenuStateIntro();
    }

    public override void Exit()
    {
        base.Exit();
        _sceneController.MainMenuStateOutro();
    }
}
