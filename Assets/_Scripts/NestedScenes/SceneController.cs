using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneController : MonoBehaviour
{
    [SerializeField] SceneFSM _sceneFSM;
    [SerializeField] RectTransform TransitionPanel;
    float _transitionPanelYPos = -611f;

    [Header("Main Menu")]
    [SerializeField] Object _mainMenuScene;

    void OnValidate()
    {
        if (_sceneFSM == null)
        {
            _sceneFSM = GetComponent<SceneFSM>();
        }
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
}
