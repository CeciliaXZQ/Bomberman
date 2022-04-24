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
    public event Action restartGame;

    [Range(3, 10)]
    public int enemyCount;
    public Vector2 gridSize;
    public FixedBlocks fixedBlock;
    public BreakableBlocks breakableBlock;
    public FloorBlocks floorBlock;
    public PlayerController playerController;
    public EnemyController enemyPrefab;
    public Bomb bombPrefab;

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

    // Start is called before the first frame update
    void Start()
    {
        playerService = new PlayerManager(playerController, bombPrefab, this);
        enemyService = new EnemyService(enemyPrefab, this);
        levelService = new LevelService(fixedBlock, breakableBlock, floorBlock, playerService, enemyService);
        enemyService.SetLevelService(levelService);

        levelService.GenerateLevel();
    }
    public void SetGameStatus(bool gameWon) => gameStatus?.Invoke(gameWon);

    public void UpdateScore() => updateScore?.Invoke();

    public void RestartGame()
    {
        restartGame?.Invoke();
    }

}
