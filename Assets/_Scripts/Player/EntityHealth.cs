using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] EntityStatsContainer statsContainer;
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
        playerStatsData = statsContainer.PlayerStatsData;
    }

    void Start()
    {
        currentHealth = playerStatsData.MaxHealth;
        entityType = statsContainer.EntityType;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Entity died");
        OnDeath?.Invoke();
    }
}
