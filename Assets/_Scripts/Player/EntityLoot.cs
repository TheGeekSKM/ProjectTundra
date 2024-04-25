using UnityEngine;

public class EntityLoot : MonoBehaviour
{
    [SerializeField] EntityStatsContainer _entityStatsContainer;
    [SerializeField] EntityHealth _entityHealth;
    [SerializeField] ChestManager _lootChestPrefab;
    public System.Action OnLootDropped;

    void Awake()
    {
        if (_entityStatsContainer == null) _entityStatsContainer = GetComponent<EntityStatsContainer>();
        if (_entityHealth == null) _entityHealth = GetComponent<EntityHealth>();
    }

    void OnEnable() => _entityHealth.OnDeath += DropLoot;
    void OnDisable() => _entityHealth.OnDeath -= DropLoot;

    void DropLoot()
    {
        Debug.Log($"Loot dropped for {gameObject.name}");
        if (_entityStatsContainer == null) return;
        if (_lootChestPrefab == null) 
        {
            Debug.LogError($"No loot chest prefab found on {name}");
            return;
        }

        var loot = Instantiate(_lootChestPrefab, transform.position, Quaternion.identity);
        // Set the loot chest to use the entity's item container and death sprite
        loot.Init(_entityStatsContainer.PlayerStatsData.ItemContainer, false, true, _entityStatsContainer.PlayerStatsData.DeathSprite);

        OnLootDropped?.Invoke();        
    }
}
