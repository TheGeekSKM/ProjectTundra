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
    [SerializeField] GameObject _mainCamera;
    [SerializeField] GameObject _eventSystem;

    [Header("Scenes")]
    [SerializeField] Object _mainMenuScene;
    [SerializeField] Object _characterSelectMenu;
    [SerializeField] Object _gamePlayScene;


    void Awake()
    {
        if (_sceneFSM == null)
        {
            _sceneFSM = GetComponent<SceneFSM>();
        }

        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void TransitionPanelOn()
    {
        TransitionPanel.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutCubic);
    }

    public void TransitionPanelOff()
    {
        TransitionPanel.DOAnchorPosY(_transitionPanelYPos, 0.5f).SetEase(Ease.OutCubic);
    }

    public void MainMenuStateIntro()
    {
        Debug.Log("MainMenuStateIntroFunctions");
        SceneManager.LoadSceneAsync(_mainMenuScene.name, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            // TransitionPanelOff();
            UpdateState(SceneState.MainMenu);
        };
    }

    public void MainMenuStateOutro()
    {
        // TransitionPanelOn();
        Debug.Log("MainMenuStateOutroFunctions");
        SceneManager.UnloadSceneAsync(_mainMenuScene.name);
    }

    public void CharacterSelectStateIntro()
    {
        Debug.Log("CharacterSelectStateIntroFunction");
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

        _mainCamera.SetActive(false);
        _eventSystem.SetActive(false);
        SceneManager.LoadSceneAsync(_gamePlayScene.name, LoadSceneMode.Additive).completed += (AsyncOperation obj) => 
        {
            TransitionPanelOff();
            UpdateState(SceneState.GamePlay);
        };
    }

    public void GamePlayStateOutro()
    {
        Debug.Log("GamePlayStateOutroFunction");
    }

    void UpdateState(SceneState _state)
    {
        _currentSceneState = _state;
        OnSceneStateChanged?.Invoke(_state);
    }
}
