using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamagePopupController : MonoBehaviour
{
    [Header("Damage Popup Components")]
    [SerializeField] TextMeshPro _damageText;
 

    [Header("Damage Popup Debug Variables")]
    [SerializeField] private float _popupMoveSpeed = 1f;
    [SerializeField] private float _popupDuration = 1f;
    [SerializeField] private float _popupVerticalMoveAmount = 1f;

    public void Setup(int damageAmount = 0, Vector3 position = default, float popupMoveSpeed = 1f, 
            float popupDuration = 1f, float popupVerticalMoveAmount = 1f) 
    {
        // set all the values for the popup
        _damageText.SetText(damageAmount.ToString());
        _popupMoveSpeed = popupMoveSpeed;
        _popupDuration = popupDuration;
        _popupVerticalMoveAmount = popupVerticalMoveAmount;
        transform.position = position;

        // move the popup up
        MoveUp();
    }

    void MoveUp()
    {
        // move the popup up using DOTween
        transform.DOMoveY(transform.position.y + _popupVerticalMoveAmount, 
            _popupMoveSpeed).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // fade out the popup using DOTween
            _damageText.DOFade(0, _popupDuration).OnComplete(() =>
            {
                // destroy the popup
                Destroy(gameObject);
            });
        });
    }
}
