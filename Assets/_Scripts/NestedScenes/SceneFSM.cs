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
    public CreditsState CreditsState { get; private set; }

    void Awake()
    {
		if (_sceneController == null) _sceneController = GetComponent<SceneController>();

		MainMenuState = new MainMenuState(_sceneController, this);
        CharacterSelectState = new CharacterSelectState(_sceneController, this);
        GamePlayState = new GamePlayState(_sceneController, this);
        LoseMenuState = new LoseMenuState(_sceneController, this);
        WinMenuState = new WinMenuState(_sceneController, this);
        TextBasedCutsceneState = new TextBasedCutsceneState(_sceneController, this);
        CreditsState = new CreditsState(_sceneController, this);
    }
}

public enum SceneState
{
    TextBasedCutscene,
    MainMenu,
    CharacterSelect,
    GamePlay,
    LoseMenu,
    WinMenu,
    Credits
}
