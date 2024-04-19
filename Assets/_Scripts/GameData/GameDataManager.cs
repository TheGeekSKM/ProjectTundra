using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    public System.Action OnGameDataChanged;
    [SerializeField] bool _rangerUsed;
    public bool RangerUsed
    {
        get { return _rangerUsed; }
        set
        {
            _rangerUsed = value;
            OnGameDataChanged?.Invoke();
        }
    }

    [SerializeField] bool _mageUsed;
    public bool MageUsed
    {
        get { return _mageUsed; }
        set
        {
            _mageUsed = value;
            OnGameDataChanged?.Invoke();
        }
    }
    [SerializeField] bool _scoutUsed;
    public bool ScoutUsed
    {
        get { return _scoutUsed; }
        set
        {
            _scoutUsed = value;
            OnGameDataChanged?.Invoke();
        }
    }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
}