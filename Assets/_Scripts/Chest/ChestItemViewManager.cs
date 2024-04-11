using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestItemViewManager : MonoBehaviour
{    
    [Header("Item View Components")]
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Button itemInfoButton;
    [SerializeField] Button itemTakeButton;
    BaseItemData _itemData;

    void Start()
    {
        SetButtonEvents();
    }


    
    /// <summary>
    /// Sets the item view with the provided item data.
    /// </summary>
    /// <param name="itemData">The item data to set the view with.</param>
    public void SetItemView(BaseItemData itemData)
    {
        _itemData = itemData;
        itemImage.sprite = itemData.ItemSprite;
        itemNameText.text = itemData.ItemName;
    }

    public void SetButtonEvents()
    {
        // get the instance of the chest view manager
        var chestViewInstance = ChestViewManager.Instance;

        // if the info button is clicked, display the item info
        itemInfoButton.onClick.AddListener(() =>
        {
            chestViewInstance.DisplayItemInfo(_itemData);
        });

        // if the take button is clicked,
        itemTakeButton.onClick.AddListener(() =>
        {
            // first, let the ChestViewManager add the item to the player inventory
            chestViewInstance.AddItemToPlayerInventory(_itemData);

            // then, remove the item from the displayed chest inventory
            Destroy(gameObject);
        });
    }
}