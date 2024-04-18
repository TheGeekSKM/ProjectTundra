using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharPanelManager : MonoBehaviour
{
    [SerializeField] Image _disabledPanel;
    [SerializeField] TextMeshProUGUI _titleText;

    private void Start()
    {
        _disabledPanel.color = new Color(255, 255, 255, 0);
    }

    public void CharUsed(bool used)
    {
        if (used)
        {
            _disabledPanel.color = new Color(255, 0, 0, 120);
            _titleText.text += "DISABLED";
        }
    }
}
