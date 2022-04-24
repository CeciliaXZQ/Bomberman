﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyService : IEnemyService
{
    private List<EnemyController> enemyControllers;
    private ILevelService levelService;

    private EnemyController enemyPrefab;

    private GameManager gameManager;

    public EnemyService(EnemyController enemyPrefab, GameManager gameManager)
    {
        this.gameManager = gameManager;
        enemyControllers = new List<EnemyController>();
        this.enemyPrefab = enemyPrefab;
        this.gameManager.restartGame += ResetEnemyList;
    }

    ~EnemyService()
    {
        this.gameManager.restartGame -= ResetEnemyList;
    }

    void ResetEnemyList()
    {
        for (int i = 0; i < enemyControllers.Count; i++)
        {
            Object.Destroy(enemyControllers[i].gameObject);
        }

        enemyControllers.Clear();
    }

    public void SetLevelService(ILevelService levelService)
    {
        this.levelService = levelService;
    }

    public void SpawnEnemy(Vector3 pos)
    {
        GameObject enemy = GameObject.Instantiate(enemyPrefab.gameObject, pos, Quaternion.identity);
        enemy.GetComponent<EnemyController>().SetServices(levelService, this);
        enemyControllers.Add(enemy.GetComponent<EnemyController>());
    }

    public void RemoveEnemy(EnemyController enemyController)
    {
        enemyControllers.Remove(enemyController);
        gameManager.UpdateScore();
        if (enemyControllers.Count <= 0)
        {
            //TODO: fire game won event
            gameManager.SetGameStatus(true);
            return;
        }
    }


}


