using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItemViewManager : MonoBehaviour
{
    [Header("Item View Components")]
    [SerializeField] Image _itemImage;
    [SerializeField] TextMeshProUGUI _itemNameText;
    [SerializeField] Button _itemUseButton;
    [SerializeField] Button _dropButton;
    BaseItemData _itemData;

    void Start()
    {
        SetButtonEvents();
    }

    void SetButtonEvents()
    {
        var chestViewInstance = ChestViewManager.Instance;

        _itemUseButton.onClick.AddListener(() =>
        {
            if (_itemData == null) return;
            if (_itemData.ItemBroken) return;

            // use the item
            var apCost = _itemData.Use();
            if (Player.Instance.PlayerStamina.CurrentActionPoints < apCost) return;
            Player.Instance.PlayerStamina.SubtractAP(apCost);

            // update the player inventory
            chestViewInstance.UpdatePlayerInventory(_itemData);
        });

        _dropButton.onClick.AddListener(() =>
        {
            if (_itemData == null) return;

            // drop the item
            Player.Instance.PlayerStats.PlayerStatsData.ItemContainer.RemoveItem(_itemData);
            Destroy(gameObject);
        });
    }

    public void SetItemView(BaseItemData itemData)
    {
        _itemData = itemData;
        _itemImage.sprite = itemData.ItemSprite;
        _itemNameText.text = itemData.ItemName;
    }


}
