using UnityEngine;

public class EntityLoot : MonoBehaviour
{
    [SerializeField] EntityStatsContainer _entityStatsContainer;
    [SerializeField] EntityHealth _entityHealth;
    [SerializeField] EntityInventoryManager _entityInventoryManager;
    [SerializeField] ChestManager _lootChestPrefab;
    public System.Action OnLootDropped;

    void Awake()
    {
        if (_entityStatsContainer == null) _entityStatsContainer = GetComponent<EntityStatsContainer>();
        if (_entityHealth == null) _entityHealth = GetComponent<EntityHealth>();
        if (_entityInventoryManager == null) _entityInventoryManager = GetComponent<EntityInventoryManager>();
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

        var loot = Instantiate(_lootChestPrefab, transform.position, Quaternion.identity, gameObject.transform.parent);
        // Set the loot chest to use the entity's item container and death sprite
        loot.Init(_entityInventoryManager.EntityInventory, false, true, _entityStatsContainer.PlayerStatsData.DeathSprite);

        OnLootDropped?.Invoke();        
    }
}
