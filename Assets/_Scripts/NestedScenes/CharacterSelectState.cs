using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectState : BaseState
{
    SceneController _sceneController;
    SceneFSM _sceneFSM;

    public CharacterSelectState(SceneController sC, SceneFSM sFSM)
    {
        _sceneController = sC;
        _sceneFSM = sFSM;
    }

    public override void Enter()
    {
        base.Enter();
        _sceneController.CharacterSelectStateIntro();
    }

    public override void Exit()
    {
        base.Exit();
        _sceneController.CharacterSelectStateOutro();
    }
}
