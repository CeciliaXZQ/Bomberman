using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public event Action<bool> gameStatus;
    public event Action updateScore;
    public event Action<int> updateHealth;
    public event Action restartGame;

    [Range(3, 10)]
    public int enemyCount;
    public int palyerHealth;
    public int enemyHealth;
    public Vector2 gridSize;
    public FixedBlocks fixedBlock;
    public BreakableBlocks breakableBlock;
    public FloorBlocks floorBlock;
    public Door doorPrefab;
    public PlayerController playerController;
    public EnemyController enemyPrefab;
    public Bomb bombPrefab;
    public SuperBomb superBombPrefab;
    public UIController uiController;

    ILevelService levelService;
    IPlayerService playerService;
    IEnemyService enemyService;

    private void Awake()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        uiController.SetGameManager(this);
        playerService = new PlayerManager(playerController, bombPrefab, superBombPrefab, this);
        enemyService = new EnemyService(enemyPrefab, this);
        levelService = new LevelService(fixedBlock, breakableBlock, floorBlock, doorPrefab, playerService, enemyService);
        enemyService.SetLevelService(levelService);

        levelService.GenerateLevel();
    }
    public void SetGameStatus(bool gameWon) => gameStatus?.Invoke(gameWon);

    public void UpdateScore() => updateScore?.Invoke();

    public void UpdateHealth(int health) => updateHealth?.Invoke(health);

    public void RestartGame()
    {
        restartGame?.Invoke();
    }

}
