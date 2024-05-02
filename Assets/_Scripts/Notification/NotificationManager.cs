using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] RectTransform _notificationPanel;
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _messageText;
    float _notificationPanelYPos;
    Coroutine _notificationCoroutine;

    [Header("ItemInformation")]
    [SerializeField] RectTransform _itemInfoPanel;
    [SerializeField] TextMeshProUGUI _itemNameText;
    [SerializeField] TextMeshProUGUI _itemDescriptionText;
    float _itemInfoPanelYPos;
    Coroutine _itemInfoCoroutine;

    [Header("Settings")]
    [SerializeField] Color _infoColor;
    [SerializeField] Color _warningColor;
    [SerializeField] Color _errorColor;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        _notificationPanelYPos = _notificationPanel.anchoredPosition.y;
        _itemInfoPanelYPos = _itemInfoPanel.anchoredPosition.y;
    }

    public void DisplayConsumableInfo(ConsumableItemData item, float duration)
    {
        _itemNameText.text = item.ItemName;
        var description = $"The {item.ItemName} will, upon consumption, will increase your {item.StatType} by {item.AmountPerUse}. It has {item.Durability} uses left.";
        _itemDescriptionText.text = description;
       
        _itemInfoPanel.DOAnchorPosY(0, 0.5f).OnComplete(() =>
        {
            if (_itemInfoCoroutine != null) StopCoroutine(_itemInfoCoroutine);
            _itemInfoCoroutine = StartCoroutine(DisplayItemInfo(duration));
        });
    }

    public void DisplayWeaponItemInfo(WeaponItemData item, float duration)
    {
        _itemNameText.text = item.ItemName;
        var description = $"The {item.ItemName} is a {item.AttackType} weapon that deals an additional {item.DamageBonus} damage. It has {item.Durability} uses left.";
        _itemDescriptionText.text = description;
       
        _itemInfoPanel.DOAnchorPosY(0, 0.5f).OnComplete(() =>
        {
            if (_itemInfoCoroutine != null) StopCoroutine(_itemInfoCoroutine);
            _itemInfoCoroutine = StartCoroutine(DisplayItemInfo(duration));
        });
    }

    IEnumerator DisplayItemInfo(float duration)
    {
        yield return new WaitForSeconds(duration);
        HideItemInfo();
    }

    void HideItemInfo()
    {
        _itemInfoPanel.DOAnchorPosY(_itemInfoPanelYPos, 0.5f);
    }

    public void Notify(NotificationData notificationData)
    {
        _titleText.text = notificationData.Title;
        _messageText.text = notificationData.Message;

        switch (notificationData.Type)
        {
            case ENotificationType.Info:
                _titleText.color = _infoColor;
                break;
            case ENotificationType.Warning:
                _titleText.color = _warningColor;
                break;
            case ENotificationType.Error:
                _titleText.color = _errorColor;
                break;
        }


        // if notification is hiding, display the new notification
        _notificationPanel.DOAnchorPosY(0, 0.5f).OnComplete(() =>
        {
            
            if (_notificationCoroutine != null) StopCoroutine(_notificationCoroutine);
            StartCoroutine(DisplayNotification(notificationData.Duration));
        });
    }

    IEnumerator DisplayNotification(float duration)
    {
        yield return new WaitForSeconds(duration);
        HideNotification();
    }

    void HideNotification()
    {
        _notificationPanel.DOAnchorPosY(_notificationPanelYPos, 0.5f);
    }
}

public struct NotificationData
{
    public string Title;
    public string Message;
    public float Duration;
    public ENotificationType Type;

    public NotificationData(string message, string title, float duration, ENotificationType type)
    {
        Title = title;
        Message = message;
        Duration = duration;
        Type = type;
    }
}

public enum ENotificationType
{
    Info,
    Warning,
    Error
}

public enum ENotificationState
{
    Displaying,
    Hiding
}
