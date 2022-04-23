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

    ILevelService levelService;

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
        levelService = new LevelService(fixedBlock, breakableBlock, floorBlock);

        levelService.GenerateLevel();
    }
}
