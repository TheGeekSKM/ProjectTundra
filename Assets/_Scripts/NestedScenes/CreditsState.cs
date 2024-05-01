using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsState : BaseState
{
    SceneController _sceneController;
    SceneFSM _sceneFSM;

    public CreditsState(SceneController sC, SceneFSM sFSM)
    {
        _sceneController = sC;
        _sceneFSM = sFSM;
    }

    public override void Enter()
    {
        base.Enter();
        _sceneController.CreditsIntro();
    }

    public override void Exit()
    {
        base.Exit();
        _sceneController.CreditsOutro();
    }
}
