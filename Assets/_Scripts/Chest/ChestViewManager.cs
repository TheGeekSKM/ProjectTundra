using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Mono.Cecil.Cil;

public class ChestViewManager : MonoBehaviour
{
    public static ChestViewManager Instance { get; private set; }
    [SerializeField] private ChestItemViewManager _chestItemViewManagerPrefab;
    [SerializeField] private PlayerItemViewManager _playerItemViewManagerPrefab;
    [SerializeField] TextMeshProUGUI _chestNameText;
    [SerializeField] private Transform _chestItemViewParent;
    [SerializeField] private ChestViewFSM _chestViewFSM;
    [SerializeField] private RectTransform _chestView;
    private float _chestViewInitialXPosition;
    bool _isChestOpen = false;
    public bool IsChestOpen => _isChestOpen;
    bool _isPlayerInventoryOpen = false;

    // this is the inventory of the person who is opening the chest
    private ItemContainer _chestOpeningInventory;

    // this is the inventory of the chest
    private ItemContainer _chestInventory;

    #region Singleton
    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }
    #endregion

    public void OpenChest(ItemContainer chestInventory, ItemContainer playerInventory)
    {
        // play the loot sound
        AudioManager.Instance.PlayAudio2D(EAudioEvent.Loot);

        Debug.Log($"Opening chest {chestInventory.ContainerName}");

        // set the chest inventory and the player inventory
        _chestInventory = chestInventory;
        _chestOpeningInventory = playerInventory;

        // changes the state
        _chestViewFSM.ChangeState(_chestViewFSM.OpenState);
    }

    public void OpenPlayerInventory()
    {
        _chestInventory = Player.Instance.PlayerStats.PlayerStatsData.ItemContainer;
        _chestOpeningInventory = Player.Instance.PlayerStats.PlayerStatsData.ItemContainer;
        _isPlayerInventoryOpen = true;

        _chestViewFSM.ChangeState(_chestViewFSM.OpenState);
    }

    public void CloseChest()
    {
        // changes the state
        _chestViewFSM.ChangeState(Instance._chestViewFSM.ClosedState);
        _isPlayerInventoryOpen = false;
    }

    public void SetChestView()
    {
        // set the chest name
        _chestNameText.text = _chestInventory.ContainerName;
        
        // set the chest items
        foreach (var item in _chestInventory.GetItems())
        {
            if (_isPlayerInventoryOpen)
            {
                var chestItem = Instantiate(_playerItemViewManagerPrefab, _chestItemViewParent);
                chestItem.SetItemView(item);
            }
            else
            {
                var chestItem = Instantiate(_chestItemViewManagerPrefab, _chestItemViewParent);
                chestItem.SetItemView(item);
            }
        }
    }

    public void ClearChestView()
    {
        // clear the chest name and the chest items
        _chestNameText.text = "";
        foreach (Transform child in _chestItemViewParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdatePlayerInventory(BaseItemData item)
    {
        // update the player inventory
        if (item.ItemBroken) _chestOpeningInventory.RemoveItem(item);
        // CloseChest();
    }

    // only does the opening animation, SHOULD NOT set the item view
    public void AnimateChestViewOpen()
    {
        _chestViewInitialXPosition = _chestView.anchoredPosition.x;
        _chestView.DOAnchorPosX(0, 0.5f);
        _isChestOpen = true;
    }

    // does the closing animation
    public void AnimateChestViewClose()
    {
        // animate the chest view to the initial position and clear the chest view
        _chestView.DOAnchorPosX(_chestViewInitialXPosition, 0.5f).OnComplete(() =>
        {
            ClearChestView();
            _isChestOpen = false;
        });
    }

    public void DisplayItemInfo(BaseItemData item)
    {
        // display the item info
        Debug.Log(item.ItemName);
    }

    public void AddItemToPlayerInventory(BaseItemData item)
    {
        // add the item to the player inventory
        _chestInventory.RemoveItem(item);
        _chestOpeningInventory.AddItem(item);
    }


}
