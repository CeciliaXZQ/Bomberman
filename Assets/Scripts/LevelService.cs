using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelService : ILevelService
{
    // Start is called before the first frame update
    private LevelGenerator levelGenerator;

    public LevelService(FixedBlocks fixedBlockPrefab, BreakableBlocks breakableBlockPrefab, FloorBlocks floorBlockPrefab)
    {
        levelGenerator = new LevelGenerator(fixedBlockPrefab, breakableBlockPrefab, floorBlockPrefab, this);
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

