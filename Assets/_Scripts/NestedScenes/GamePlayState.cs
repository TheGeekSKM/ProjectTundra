using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayState : BaseState
{
    SceneController _sceneController;
    SceneFSM _sceneFSM;
    public GamePlayState(SceneController sceneController, SceneFSM sceneFSM)
    {
        _sceneController = sceneController;
        _sceneFSM = sceneFSM;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("GamePlayState OnStateEnter");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("GamePlayState OnStateExit");
    }
}
