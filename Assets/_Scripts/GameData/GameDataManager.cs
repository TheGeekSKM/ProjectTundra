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

    [SerializeField] EntityStatsContainer _ranger;
    public EntityStatsContainer Ranger => _ranger;

    [SerializeField] EntityStatsContainer _mage;
    public EntityStatsContainer Mage => _mage;

    [SerializeField] EntityStatsContainer _scout;
    public EntityStatsContainer Scout => _scout;

    [SerializeField] SceneController _sceneController;

    EntityStatsContainer _currentStats;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        _rangerUsed = false;
        _mageUsed = false;
        _scoutUsed = false;

        _sceneController.OnSceneStateChanged += OnSceneChanged;
    }

    void OnDisable()
    {
        _sceneController.OnSceneStateChanged -= OnSceneChanged;
    }

    // ADD TO THIS WHEN MORE SCENES ARE ADDED
    void OnSceneChanged(SceneState state)
    {
        switch (state)
        {
            case SceneState.MainMenu:
                break;
            case SceneState.CharacterSelect:
                break;
            case SceneState.GamePlay:
                SpawnPlayer();
                break;
            default:
                break;
        }
    }

    void SpawnPlayer()
    {
        // TODO: add logic here that will spawn the player when the gameplay scene is loaded
        // TODO: use _currentStats variable to determine which player to spawn
    }

    public void SelectRanger()
    {
        RangerUsed = true;
        _currentStats = Ranger;
    }

    public void SelectMage()
    {
        MageUsed = true;
        _currentStats = Mage;
    }

    public void SelectScout()
    {
        ScoutUsed = true;
        _currentStats = Scout;
    }
}
