using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHudManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _healthText;
    [SerializeField] TextMeshProUGUI _staminaText;
    EntityHealth _playerHealth;
    PlayerStatsData _playerStatsData;
    EntityStamina _playerStamina;

    private void Awake()
    {
        _playerHealth = Player.Instance.PlayerHealth;
        _playerStatsData = Player.Instance.PlayerStats.PlayerStatsData;
        _playerStamina = Player.Instance.PlayerStamina;
    }

    private void OnEnable()
    {
        _playerHealth.OnHealthChanged += HPChanged;
        _playerStamina.OnActionPointsChanged += APChanged;
    }

    private void OnDisable()
    {
        _playerHealth.OnHealthChanged -= HPChanged;
        _playerStamina.OnActionPointsChanged -= APChanged;
    }

    private void Start()
    {
        _healthText.text = $"{_playerHealth.CurrentHealth} / {_playerStatsData.MaxHealth}";
        _staminaText.text = $"{_playerStamina.CurrentActionPoints} / {_playerStatsData.TotalActionPoints}";
    }

    void HPChanged(int currentHealth)
    {
        _healthText.text = $"{currentHealth} / {_playerStatsData.MaxHealth}";
    }

    void APChanged(int currentStamina)
    {
        _staminaText.text = $"{currentStamina} / {_playerStatsData.TotalActionPoints}";
    }

}
