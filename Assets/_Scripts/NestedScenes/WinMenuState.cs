using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenuState : BaseState
{
    SceneController _sceneController;
    SceneFSM _sceneFSM;

    public WinMenuState(SceneController sceneController, SceneFSM sceneFSM)
    {
        _sceneController = sceneController;
        _sceneFSM = sceneFSM;
    }

    public override void Enter()
    {
        _sceneController.WinMenuStateIntro();
    }

    public override void Exit()
    {
        _sceneController.WinMenuStateOutro();        
    }
}

