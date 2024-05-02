using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlsManager : MonoBehaviour
{
    [SerializeField] Button _attackButton;
    [SerializeField] Button _inventoryButton;
    [SerializeField] Button _moveButton;
    [SerializeField] Button _backButton;

    UIInputManager _uiInputManager;

    void Start()
    {
        // Get the UIInputManager instance
        _uiInputManager = UIInputManager.Instance;

        // Check if the UIInputManager is null
        if (_uiInputManager == null)
        {
            Debug.LogError("UIInputManager is null");
            return;
        }

        // Subscribe to the button click events
        _attackButton?.onClick.AddListener(() => _uiInputManager.Attack());
        
        _inventoryButton?.onClick.AddListener(() => ChestViewManager.Instance.OpenPlayerInventory());

        _moveButton?.onClick.AddListener(() => {
            CombatManager.Instance.AnimatePlayerControlsOutro();
            CombatManager.Instance.AnimatePlayerMovementControlsIntro();
        });

        _backButton?.onClick.AddListener(() => {
            CombatManager.Instance.AnimatePlayerMovementControlsOutro();
            CombatManager.Instance.AnimatePlayerControlsIntro();
        });
    }
    
}
