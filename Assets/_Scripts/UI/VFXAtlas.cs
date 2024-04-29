using UnityEngine;

public class VFXAtlas : MonoBehaviour
{
    public static VFXAtlas Instance { get; private set; }
    [SerializeField] private GameObject _bloodSplatter;
    public GameObject BloodSplatter => _bloodSplatter;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void PlayVFX(VFXEvent vfxEvent, Transform _transform)
    {
        switch (vfxEvent)
        {
            case VFXEvent.BloodSplatter:
                Instantiate(_bloodSplatter, _transform.position, _transform.rotation);
                break;
        }
    }
}

public enum VFXEvent
{
    BloodSplatter
}
