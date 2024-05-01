using UnityEngine;
using DG.Tweening;

public class CreditsController : MonoBehaviour
{
    RectTransform _creditsPanel;
    [SerializeField] float _bottomPosition = -1920f;
    [SerializeField] float _topPosition = 1920f;

    [SerializeField] RectTransform _buttonTransform;

    void Start()
    {
        _creditsPanel = GetComponent<RectTransform>();
        // move the credits panel to the bottom of the screen
        _creditsPanel.anchoredPosition = new Vector2(0, _bottomPosition);

        _buttonTransform.DOAnchorPosX(50, 0.5f).SetEase(Ease.OutCubic);
        
        // animate the credits panel to the top of the screen
        _creditsPanel.DOAnchorPosY(_topPosition, 20f).SetEase(Ease.Linear).OnComplete(() => {
            EndCredits();
        });

        
    }

    public void EndCredits()
    {
        var sceneFSM = SceneController.Instance.SceneFSM;
        sceneFSM.ChangeState(sceneFSM.MainMenuState);
    }
}
