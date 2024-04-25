using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFSM : BaseStateMachine
{
    [SerializeField] SceneController _sceneController;

    public MainMenuState MainMenuState { get; private set; }
    public CharacterSelectState CharacterSelectState { get; private set; }
    public GamePlayState GamePlayState { get; private set; }
    public LoseMenuState LoseMenuState { get; private set; }
    public WinMenuState WinMenuState { get; private set; }
    public TextBasedCutsceneState TextBasedCutsceneState { get; private set; }

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
        CharacterSelectState = new CharacterSelectState(_sceneController, this);
        GamePlayState = new GamePlayState(_sceneController, this);
        LoseMenuState = new LoseMenuState(_sceneController, this);
        WinMenuState = new WinMenuState(_sceneController, this);
        TextBasedCutsceneState = new TextBasedCutsceneState(_sceneController, this);
    }

    void Start()
    {
        ChangeState(TextBasedCutsceneState, 0.5f, 0f);
    }
}

public enum SceneState
{
    TextBasedCutscene,
    MainMenu,
    CharacterSelect,
    GamePlay,
    LoseMenu,
    WinMenu
}
