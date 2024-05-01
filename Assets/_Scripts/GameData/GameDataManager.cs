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

    [SerializeField] GameObject _ranger;
    public GameObject Ranger => _ranger;

    [SerializeField] GameObject _mage;
    public GameObject Mage => _mage;

    [SerializeField] GameObject _scout;
    public GameObject Scout => _scout;
    [SerializeField] Vector2Int _spawnPosition;

    [SerializeField] SceneController _sceneController;

    [SerializeField] int _playerKillCount;
    public int PlayerKillCount
    {
        get => _playerKillCount;
        set => _playerKillCount = value;
    }
    // enemy difficulty increases every 3 kills
    public int EnemyDifficulty => Mathf.FloorToInt(_playerKillCount / 3);

    GameObject _currentStats;

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
                break;
            default:
                break;
        }
    }

    void SpawnPlayer()
    {
        if (Player.Instance)
        {
            Player.Instance.transform.position = new Vector3(_spawnPosition.x+0.5f, _spawnPosition.y+0.5f, 0);

            var playerStatsContainer = Player.Instance.GetComponent<EntityStatsContainer>();
            var selectedStatsContainer = _currentStats.GetComponent<EntityStatsContainer>();

            playerStatsContainer.SetPlayerStatsData(selectedStatsContainer.PlayerStatsData);

            Player.Instance.Initialize();
        }
    }

    public void SelectRanger()
    {
        RangerUsed = true;
        _currentStats = Ranger;
        SpawnPlayer();
    }

    public void SelectMage()
    {
        MageUsed = true;
        _currentStats = Mage;
        SpawnPlayer();
    }

    public void SelectScout()
    {
        ScoutUsed = true;
        _currentStats = Scout;
        SpawnPlayer();
    }
}
