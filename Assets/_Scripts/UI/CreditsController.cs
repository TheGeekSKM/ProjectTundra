using UnityEngine;
using DG.Tweening;

public class CreditsController : MonoBehaviour
{
    RectTransform _creditsPanel;
    [SerializeField] float _bottomPosition = -1920f;
    [SerializeField] float _topPosition = 1920f;

    void Start()
    {
        _creditsPanel = GetComponent<RectTransform>();
        // move the credits panel to the bottom of the screen
        _creditsPanel.anchoredPosition = new Vector2(0, _bottomPosition);
        
        // animate the credits panel to the top of the screen
        _creditsPanel.DOAnchorPosY(_topPosition, 60f).SetEase(Ease.Linear).OnComplete(() => {
            // wait for 60 seconds before returning to the main menu
            var sceneFSM = SceneController.Instance.SceneFSM;
            sceneFSM.ChangeState(sceneFSM.MainMenuState);
        });

        
    }
}
