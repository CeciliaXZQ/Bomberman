using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyService
{
    void SpawnEnemy(Vector3 pos);
    void SetLevelService(ILevelService levelService);
}
