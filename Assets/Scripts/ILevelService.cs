using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelService
{
    void GenerateLevel();
    GameObject GetObjAtGrid(Vector2 position);
    void FillGrid(Vector2 position, GameObject gameObject);
    void EmptyGrid(Vector2 position);
}
