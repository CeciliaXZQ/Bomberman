using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[,] gridArray;
    private List<Vector2> emptyGridList;
    private int edgeCount = 2;
    private int gridWidth = 15, gridHeight = 11, enemyCount;

    private FixedBlocks fixedBlockPref;
    private FloorBlocks floorBlockPref;
    private BreakableBlocks breakableBlockPref;
    private GameObject levelHolder;

    LevelService levelService;

    public LevelGenerator(FixedBlocks fixedBlockPrefab, BreakableBlocks breakableBlockPrefab, FloorBlocks floorBlockPrefab, LevelService levelService)
    {
        this.levelService = levelService;
        this.fixedBlockPref = fixedBlockPrefab;
        this.breakableBlockPref = breakableBlockPrefab;
        this.floorBlockPref = floorBlockPrefab;
        gridWidth = (int)GameManager.singleton.gridSize.x;
        gridHeight = (int)GameManager.singleton.gridSize.y;
    }

    ~LevelGenerator()
    {
        GameManager.singleton.restartGame -= RestartGame;
    }

    void RestartGame()
    {
        for (int i = 0; i < gridWidth + edgeCount; i++)
        {
            for (int j = 0; j < gridHeight + edgeCount; j++)
            {
                if (gridArray[i, j] != null)
                {
                    GameObject obj = gridArray[i, j];
                    Object.Destroy(obj);
                    gridArray[i, j] = null;
                }
            }
        }

        gridArray = null;

        GenerateLevel();
    }

    public void GenerateLevel()
    {
        emptyGridList = new List<Vector2>();
        gridArray = new GameObject[gridWidth + edgeCount, gridHeight + edgeCount];
        Debug.Log(gridArray.Length);
        if (levelHolder == null)
            levelHolder = new GameObject();
        levelHolder.transform.position = Vector3.zero;
        levelHolder.name = "LevelHolder";

        GenerateGrid();
        GenerateEdgeAndFloor();
        GenerateFixedBlock();
        GenerateBreakableBlock();
        emptyGridList.Clear();
    }

    void GenerateGrid()
    {
        for (int i = 0; i < gridWidth + edgeCount; i++)
        {
            for (int j = 0; j < gridHeight + edgeCount; j++)
            {
                Vector2 vector = new Vector2(i, j);
                emptyGridList.Add(vector);
                gridArray[i, j] = null;
            }
        }
    }

    void GenerateEdgeAndFloor()
    {
        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int y = 0; y < gridHeight + edgeCount; y++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int x = 0; x < gridWidth + edgeCount; x++)
            {
                //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
                if (x == 0 || x == gridHeight + edgeCount-1 || y == 0 || y == gridWidth + edgeCount-1)
                {
                    Edge(new Vector2(x, y));
                }
                else
                {
                    //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                    Floor(new Vector2(x, y));
                }

            }
        }
    }

    void Edge(Vector2 pos)
    {
        GameObject fixedBlock = Object.Instantiate(fixedBlockPref.gameObject, pos, Quaternion.identity);
        fixedBlock.transform.SetParent(levelHolder.transform);
        fixedBlock.name = "Edge[" + pos.x + "," + pos.y + "]";
        emptyGridList.Remove(pos);
        gridArray[(int)pos.x, (int)pos.y] = fixedBlock;
    }

    void Floor(Vector2 pos)
    {
        GameObject floorBlock = Object.Instantiate(floorBlockPref.gameObject, pos, Quaternion.identity);
        floorBlock.transform.SetParent(levelHolder.transform);
        floorBlock.name = "Floor[" + pos.x + "," + pos.y + "]";
        gridArray[(int)pos.x, (int)pos.y] = floorBlock;
    }


    void GenerateFixedBlock()
    {
        for (int i = edgeCount; i < gridWidth; i += 2)
        {
            for (int j = edgeCount; j < gridHeight; j += 2)
            {
                Vector2 vector = new Vector2(i, j);
                GameObject fixedBlock = Object.Instantiate(fixedBlockPref.gameObject, vector, Quaternion.identity);
                fixedBlock.transform.SetParent(levelHolder.transform);
                fixedBlock.name = "Fixed[" + vector.x + "," + vector.y + "]";
                emptyGridList.Remove(vector);
                gridArray[(int)vector.x, (int)vector.y] = fixedBlock;
            }
        }
    }

    void GenerateBreakableBlock()
    {
        int val = Random.Range(Mathf.CeilToInt(emptyGridList.Count / 6), Mathf.CeilToInt(emptyGridList.Count / 3));
        for (int i = 0; i < val; i++)
        {
            int k = Random.Range(0, emptyGridList.Count);
            Vector2 vector = emptyGridList[k];
            GameObject breakableBlock = Object.Instantiate(breakableBlockPref.gameObject, vector, Quaternion.identity);
            breakableBlock.transform.SetParent(levelHolder.transform);
            breakableBlock.name = "Breakable[" + vector.x + "," + vector.y + "]";
            breakableBlock.GetComponent<BreakableBlocks>().SetLevelService(levelService);
            emptyGridList.RemoveAt(k);
            gridArray[(int)vector.x, (int)vector.y] = breakableBlock;
        }
    }
}
