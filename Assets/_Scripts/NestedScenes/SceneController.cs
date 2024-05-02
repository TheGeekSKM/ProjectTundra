using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

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
    [SerializeField] SceneField _mainMenuScene;
    [SerializeField] SceneField _characterSelectMenu;
    [SerializeField] SceneField _gamePlayScene;
    [SerializeField] SceneField _loseMenuScene;
    [SerializeField] SceneField _winMenuScene;
    [SerializeField] SceneField _textBasedCutsceneScene;
    [SerializeField] SceneField _creditsScene;

    void Awake()
    {
        if (_sceneFSM == null) _sceneFSM = GetComponent<SceneFSM>();

        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

	void Start()
	{
		SceneFSM.ChangeState(SceneFSM.TextBasedCutsceneState, 0.5f, 0f);
	}

	public void TransitionPanelOn() => TransitionPanel.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutCubic);
    public void TransitionPanelOff() => TransitionPanel.DOAnchorPosY(_transitionPanelYPos, 0.5f).SetEase(Ease.OutCubic);

    public void TextBasedIntro()
    {
        SceneManager.LoadSceneAsync(_textBasedCutsceneScene, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            UpdateState(SceneState.TextBasedCutscene);
            MusicManager.Instance.SwapTrack(EAudioEvent.ScrollBGM);
        };
    }

    public void TextBasedOutro()
    {
        SceneManager.UnloadSceneAsync(_textBasedCutsceneScene).completed += (AsyncOperation obj) => 
        {
            _sceneFSM.ChangeState(_sceneFSM.MainMenuState);
        };
    }

    public void CreditsIntro()
    {
        SceneManager.LoadSceneAsync(_creditsScene, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            UpdateState(SceneState.Credits);
        };
    }

    public void CreditsOutro()
    {
        SceneManager.UnloadSceneAsync(_creditsScene);
    }

    public void MainMenuStateIntro()
    {
        Debug.Log("MainMenuStateIntroFunctions");
        SceneManager.LoadSceneAsync(_mainMenuScene, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            UpdateState(SceneState.MainMenu);
            UnloadGamePlayLevel();
            MusicManager.Instance.SwapTrack(EAudioEvent.MainMenuBGM);            
        };
    }

    public void MainMenuStateOutro()
    {
        SceneManager.LoadSceneAsync(_gamePlayScene, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            Debug.Log("GamePlaySceneLoaded");
        };
        
        TransitionPanel.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutCubic).OnComplete(() => { 
            SceneManager.UnloadSceneAsync(_mainMenuScene);
        });
        Debug.Log("MainMenuStateOutroFunctions");
    }

    public void CharacterSelectStateIntro()
    {
        Debug.Log("CharacterSelectStateIntroFunction");
        
        SceneManager.LoadSceneAsync(_characterSelectMenu, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            TransitionPanelOff();
            UpdateState(SceneState.CharacterSelect);
        };
    }

    public void CharacterSelectStateOutro()
    {
        
        SceneManager.UnloadSceneAsync(_characterSelectMenu);
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
        SceneManager.LoadSceneAsync(_loseMenuScene, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            MusicManager.Instance.SwapTrack(EAudioEvent.MainMenuBGM);
            UpdateState(SceneState.LoseMenu);
        };
    }

    public void LoseMenuStateOutro()
    {
        SceneManager.UnloadSceneAsync(_loseMenuScene);
    }

    public void WinMenuStateIntro()
    {
        SceneManager.LoadSceneAsync(_winMenuScene, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            UpdateState(SceneState.WinMenu);

            //unload gameplay level
            UnloadGamePlayLevel();
            MusicManager.Instance.SwapTrack(EAudioEvent.MainMenuBGM);
        };
    }

    public void WinMenuStateOutro()
    {
        SceneManager.UnloadSceneAsync(_winMenuScene);
    }

    void UpdateState(SceneState _state)
    {
        _currentSceneState = _state;
        OnSceneStateChanged?.Invoke(_state);
    }

    void UnloadGamePlayLevel()
    {
        //unload game play scene only if it's loaded
        if (SceneManager.GetSceneByName(_gamePlayScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(_gamePlayScene);
        }
    }
}
