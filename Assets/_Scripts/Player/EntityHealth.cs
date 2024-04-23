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
        playerStatsData = statsContainer.PlayerStatsData;
    }

    void Start()
    {
        currentHealth = playerStatsData.MaxHealth;
        entityType = statsContainer.EntityType;
    }

    public void TakeDamage(int damage)
    {
        OnHealthChanged?.Invoke(currentHealth);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        // Check if enough stamina to heal
        if (playerStatsData.HealCost > entityStamina.CurrentActionPoints)
        {
            Debug.Log("Not enough stamina to heal");
            return;
        }

        OnHealthChanged?.Invoke(currentHealth);
        currentHealth += amount;

        // Check if health is already full
        if (currentHealth > playerStatsData.MaxHealth)
            currentHealth = playerStatsData.MaxHealth;

        // Subtract stamina
        entityStamina.SubtractAP(playerStatsData.HealCost);
    }

    public void Die()
    {
        Debug.Log("Entity died");
        OnDeath?.Invoke();
    }
}
