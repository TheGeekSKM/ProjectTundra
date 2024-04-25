using System.Collections;
using System.Collections.Generic;
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

    [Header("Settings")]
    [SerializeField] Color _infoColor;
    [SerializeField] Color _warningColor;
    [SerializeField] Color _errorColor;

    [Header("State")]
    [SerializeField] ENotificationState _notificationState;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        _notificationPanelYPos = _notificationPanel.anchoredPosition.y;
        _notificationState = ENotificationState.Hiding;
    }

    public void Notify(NotificationData notificationData)
    {
        // if notification is already displaying, just update the text
        if (_notificationState == ENotificationState.Displaying)
        {
            _titleText.text = notificationData.Title;
            _messageText.text = notificationData.Message;
        }

        // if notification is hiding, display the new notification
        _notificationPanel.DOAnchorPosY(0, 0.5f).OnComplete(() =>
        {
            _notificationState = ENotificationState.Displaying;
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
        _notificationPanel.DOAnchorPosY(_notificationPanelYPos, 0.5f).OnComplete(() =>
        {
            _notificationState = ENotificationState.Hiding;
        });
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
