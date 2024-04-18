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

    [Header("Main Menu")]
    [SerializeField] Object _mainMenuScene;
    [SerializeField] Object _characterSelectMenu;


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
        SceneManager.LoadSceneAsync(_mainMenuScene.name, LoadSceneMode.Additive).completed += (AsyncOperation obj) => TransitionPanelOff();
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
        SceneManager.LoadSceneAsync(_characterSelectMenu.name, LoadSceneMode.Additive).completed += (AsyncOperation obj) => TransitionPanelOff();
    }

    public void CharacterSelectStateOutro()
    {
        TransitionPanelOn();
        SceneManager.UnloadSceneAsync(_characterSelectMenu.name);
    }
}
