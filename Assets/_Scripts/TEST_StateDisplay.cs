using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TEST_StateDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _stateText;
    void OnEnable()
    {
        CombatManager.Instance.OnTurnChanged += HandleTurnChanged;
    }

    void OnDisable()
    {
        CombatManager.Instance.OnTurnChanged -= HandleTurnChanged;
    }

    void HandleTurnChanged(CombatTurnState turnState)
    {
        _stateText.text = turnState.ToString();
    }
}
