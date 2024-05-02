using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChecker : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player)
        {
            var sceneFSM = SceneController.Instance.SceneFSM;
            sceneFSM.ChangeState(sceneFSM.WinMenuState);
        }
    }
}
