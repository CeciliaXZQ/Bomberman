using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelService : ILevelService
{
    // Start is called before the first frame update
    private LevelGenerator levelGenerator;
    private IPlayerService playerService;

    public LevelService(FixedBlocks fixedBlockPrefab, BreakableBlocks breakableBlockPrefab, FloorBlocks floorBlockPrefab, Door doorPrefab, IPlayerService playerService, IEnemyService enemyService)
    {
        this.playerService = playerService;
        this.playerService.SetLevelService(this);
        levelGenerator = new LevelGenerator(fixedBlockPrefab, breakableBlockPrefab, floorBlockPrefab, doorPrefab, playerService, enemyService, this);
    }

    public void EmptyGrid(Vector2 position)
    {
        levelGenerator.gridArray[(int)position.x, (int)position.y] = null;
    }

    public void FillGrid(Vector2 position, GameObject gameObject)
    {
        if (levelGenerator.gridArray[(int)position.x, (int)position.y])
            levelGenerator.gridArray[(int)position.x, (int)position.y] = gameObject;
    }

    public void GenerateLevel()
    {
        levelGenerator.GenerateLevel();
    }

    public GameObject GetObjAtGrid(Vector2 position)
    {
        GameObject obj = null;

        if (levelGenerator.gridArray[(int)position.x, (int)position.y])
            obj = levelGenerator.gridArray[(int)position.x, (int)position.y];


        return obj;
    }
}

