using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseSceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalText;
    [SerializeField] Button respawnButton;
    [TextArea(2, 5)]
    [SerializeField] string loseTextWithRespawn = "You have fallen,\nyet your soul has not.";
    [TextArea(2, 5)]
    [SerializeField] string loseTextWithoutRespawn = "You have fallen,\nand your soul has been lost.";

    void Start()
    {
        if (!finalText || !respawnButton) return;

        if (GameDataManager.Instance.RangerUsed &&
            GameDataManager.Instance.MageUsed &&
            GameDataManager.Instance.ScoutUsed)
            {
                finalText.text = loseTextWithoutRespawn;
                respawnButton.interactable = false;
            }
        else
        {
            finalText.text = loseTextWithRespawn;
            respawnButton.interactable = true;
        }
    }

    public void Reborn()
    {
        var sceneFSM = SceneController.Instance.SceneFSM;
        sceneFSM.ChangeState(sceneFSM.CharacterSelectState);
    }

    public void MainMenu()
    {
        var sceneFSM = SceneController.Instance.SceneFSM;
        sceneFSM.ChangeState(sceneFSM.MainMenuState);
    }
}
