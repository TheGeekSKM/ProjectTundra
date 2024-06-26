using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] EntityStatsContainer statsContainer;
    [SerializeField] EntityStamina entityStamina;
    [SerializeField] int currentHealth;
    public int CurrentHealth => currentHealth;
    EntityType entityType;
    public EntityType EntityType => entityType;
    PlayerStatsData playerStatsData;

    public System.Action<int> OnHealthChanged;
    public System.Action OnDeath;

    void Awake()
    {
        if (statsContainer == null) statsContainer = GetComponent<EntityStatsContainer>();
        if (entityStamina == null) entityStamina = GetComponent<EntityStamina>();
    }

    public void Initialize()
    {
        playerStatsData = statsContainer.PlayerStatsData;
        currentHealth = playerStatsData.MaxHealth;
        entityType = statsContainer.PlayerStatsData.EntityType;
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        HandleSound();

		currentHealth -= damage;
		OnHealthChanged?.Invoke(currentHealth);
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        DamagePopupManager.Instance.DisplayDamage(damage, transform.position);
        VFXAtlas.Instance.PlayVFX(VFXEvent.BloodSplatter, attacker ? attacker : transform);
    }

    void HandleSound()
    {
        switch (entityType)
        {
            case EntityType.Player:
                AudioManager.Instance.PlayAudio3D(EAudioEvent.PlayerHurt, transform.position);
                break;
            case EntityType.Enemy:
                AudioManager.Instance.PlayAudio3D(EAudioEvent.EnemyHurt, transform.position);
                break;
        }
    }

    public void Heal()
    {
        #region Old Code
        // // Check if enough stamina to heal
        // if (playerStatsData.HealCost > entityStamina.CurrentActionPoints)
        // {
        //     Debug.Log("Not enough stamina to heal");
        //     return;
        // }

        // OnHealthChanged?.Invoke(currentHealth);
        // currentHealth += amount;

        // // Check if health is already full
        // if (currentHealth > playerStatsData.MaxHealth)
        //     currentHealth = playerStatsData.MaxHealth;

        // // Subtract stamina
        // entityStamina.SubtractAP(playerStatsData.HealCost);
        #endregion

        
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} died");
        OnDeath?.Invoke();
    }
}
