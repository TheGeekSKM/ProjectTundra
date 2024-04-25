using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    [SerializeField] SceneFSM _sceneFSM;
    public SceneFSM SceneFSM => _sceneFSM;
    [SerializeField] RectTransform TransitionPanel;
    float _transitionPanelYPos = -1940f;
    [SerializeField] SceneState _currentSceneState;
    public SceneState CurrentSceneState => _currentSceneState;
    public System.Action<SceneState> OnSceneStateChanged;

    [Header("Scenes")]
    [SerializeField] Object _mainMenuScene;
    [SerializeField] Object _characterSelectMenu;
    [SerializeField] Object _gamePlayScene;
    [SerializeField] Object _loseMenuScene;
    [SerializeField] Object _winMenuScene;
    [SerializeField] Object _textBasedCutsceneScene;


    void Awake()
    {
        if (_sceneFSM == null) _sceneFSM = GetComponent<SceneFSM>();

        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void TransitionPanelOn() => TransitionPanel.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutCubic);
    public void TransitionPanelOff() => TransitionPanel.DOAnchorPosY(_transitionPanelYPos, 0.5f).SetEase(Ease.OutCubic);

    public void TextBasedIntro()
    {
        SceneManager.LoadSceneAsync(_textBasedCutsceneScene.name, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            UpdateState(SceneState.TextBasedCutscene);
            MusicManager.Instance.SwapTrack(EAudioEvent.ScrollBGM);
        };
    }

    public void TextBasedOutro()
    {
        TransitionPanelOn();
        SceneManager.UnloadSceneAsync(_textBasedCutsceneScene.name).completed += (AsyncOperation obj) => 
        {
            _sceneFSM.ChangeState(_sceneFSM.MainMenuState);
        };
    }

    public void MainMenuStateIntro()
    {
        Debug.Log("MainMenuStateIntroFunctions");
        SceneManager.LoadSceneAsync(_mainMenuScene.name, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            UpdateState(SceneState.MainMenu);
            UnloadGamePlayLevel();
            MusicManager.Instance.SwapTrack(EAudioEvent.MainMenuBGM);            
        };
    }

    public void MainMenuStateOutro()
    {
        TransitionPanelOn();
        Debug.Log("MainMenuStateOutroFunctions");
        SceneManager.UnloadSceneAsync(_mainMenuScene.name);
    }

    public void CharacterSelectStateIntro()
    {
        Debug.Log("CharacterSelectStateIntroFunction");
        SceneManager.LoadSceneAsync(_gamePlayScene.name, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            Debug.Log("GamePlaySceneLoaded");
        };
        SceneManager.LoadSceneAsync(_characterSelectMenu.name, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            UpdateState(SceneState.CharacterSelect);
        };
    }

    public void CharacterSelectStateOutro()
    {
        TransitionPanelOn();
        SceneManager.UnloadSceneAsync(_characterSelectMenu.name);
    }

    public void GamePlayStateIntro()
    {
        //game play scene should already be loaded in
        MusicManager.Instance.SwapTrack(EAudioEvent.NonCombatBGM);
        UpdateState(SceneState.GamePlay);
    }

    public void GamePlayStateOutro()
    {
        // this shouldn't unload the scene either
    }

    public void LoseMenuStateIntro()
    {
        SceneManager.LoadSceneAsync(_loseMenuScene.name, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            MusicManager.Instance.SwapTrack(EAudioEvent.MainMenuBGM);
            UpdateState(SceneState.LoseMenu);
        };
    }

    public void WinMenuStateIntro()
    {
        SceneManager.LoadSceneAsync(_winMenuScene.name, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            UpdateState(SceneState.WinMenu);

            //unload gameplay level
            UnloadGamePlayLevel();
            MusicManager.Instance.SwapTrack(EAudioEvent.MainMenuBGM);
        };
    }

    void UpdateState(SceneState _state)
    {
        _currentSceneState = _state;
        OnSceneStateChanged?.Invoke(_state);
    }

    void UnloadGamePlayLevel()
    {
        //unload game play scene only if it's loaded
        if (SceneManager.GetSceneByName(_gamePlayScene.name).isLoaded)
        {
            SceneManager.UnloadSceneAsync(_gamePlayScene.name);
        }
    }
}
