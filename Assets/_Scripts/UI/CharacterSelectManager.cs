using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _char1PosX;
    [SerializeField] float _char2PosX;
    [SerializeField] float _char3PosX;

    [Header("References")]
    [SerializeField] RectTransform _charsPanel;
    [SerializeField] CharPanelManager _rangerPanel;
    [SerializeField] CharPanelManager _magePanel;
    [SerializeField] CharPanelManager _scoutPanel;

    [Header("Buttons")]
    [SerializeField] Button _rangerEmbarkButton;
    [SerializeField] Button _mageEmbarkButton;
    [SerializeField] Button _scoutEmbarkButton;
    
    [Header("Debug")]
    [SerializeField] int _currentMenuIndex = 0;

    private void Start()
    {
        DisplayCharacter();
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.OnGameDataChanged += HideCharactersIfUsed;
        }
    }

    public void NextCharacter()
    {
        if (_currentMenuIndex >= 2) _currentMenuIndex = 0;
        else _currentMenuIndex++;

        DisplayCharacter();
    }

    public void PreviousCharacter()
    {
        if (_currentMenuIndex <= 0) _currentMenuIndex = 2;
        else _currentMenuIndex--;

        DisplayCharacter();
    }

    [ContextMenu("LogAnchorPosX")]
    public void LogAnchorPosX()
    {
        Debug.Log($"XPos -> {_charsPanel.anchoredPosition.x}");
    }

    void DisplayCharacter()
    {
        switch (_currentMenuIndex)
        {
            case 0:
                _charsPanel.DOAnchorPosX(_char1PosX, 0.5f).SetEase(Ease.OutCubic);
                break;
            case 1:
                _charsPanel.DOAnchorPosX(_char2PosX, 0.5f).SetEase(Ease.OutCubic);
                break;
            case 2:
                _charsPanel.DOAnchorPosX(_char3PosX, 0.5f).SetEase(Ease.OutCubic);
                break;
            default:
                break;
        }

        HideCharactersIfUsed();
    }

    void HideCharactersIfUsed()
    {
        _rangerPanel.CharUsed(GameDataManager.Instance.RangerUsed);
        _magePanel.CharUsed(GameDataManager.Instance.MageUsed);
        _scoutPanel.CharUsed(GameDataManager.Instance.ScoutUsed);

        _rangerEmbarkButton.enabled = !GameDataManager.Instance.RangerUsed;
        _mageEmbarkButton.enabled = !GameDataManager.Instance.MageUsed;
        _scoutEmbarkButton.enabled = !GameDataManager.Instance.ScoutUsed;
    }
}
