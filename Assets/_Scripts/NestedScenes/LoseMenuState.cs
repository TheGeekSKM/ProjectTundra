using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseMenuState : BaseState
{
    SceneController _sceneController;
    SceneFSM _sceneFSM;

    public LoseMenuState(SceneController sceneController, SceneFSM sceneFSM)
    {
        _sceneController = sceneController;
        _sceneFSM = sceneFSM;
    }

    public override void Enter()
    {
        _sceneController.LoseMenuStateIntro();
    }

    public override void Exit()
    {
        _sceneController.LoseMenuStateOutro();
    }
}
