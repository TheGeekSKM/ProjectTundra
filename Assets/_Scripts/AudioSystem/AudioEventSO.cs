using UnityEngine;

[CreateAssetMenu(fileName = "AudioEvent", menuName = "Audio/AudioEvent")]
public class AudioEventSO : ScriptableObject
{
    public EAudioEvent audioEvent;
    public AudioClip audioClip;
}

public enum EAudioEvent
{
    ButtonClick,
    PlayerHurt,
    PlayerAttack,
    CombatBGM,
    MainMenuBGM,
    ScrollBGM,
    NonCombatBGM
}
