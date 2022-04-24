using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : IPlayerService
{
    private PlayerController playerController;
    private PlayerLibrary playerLibrary;
    private Bomb bombPrefab;
    private ILevelService levelService;
    private GameManager gameManager;

    public PlayerManager(PlayerController playerController, Bomb bombPrefab, GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.playerController = playerController;
        this.bombPrefab = bombPrefab;
        gameManager.restartGame += RestartGame;
    }

    ~PlayerManager()
    {
        gameManager.restartGame -= RestartGame;
    }

    public void SpawnPlayer(Vector2 spawnPos)
    {
        Debug.Log(spawnPos);
        playerLibrary = new PlayerLibrary(playerController, bombPrefab.gameObject,spawnPos, this,
        levelService);
    }

    public void PlayerKilled()
    {
        //TODO: fire game lost event
        gameManager.SetGameStatus(false);
        playerController = null;
    }

    void RestartGame()
    {
        if (playerLibrary != null)
        {
            playerLibrary.PlayerDestroy();
            playerLibrary = null;
        }
    }

    public GameObject GetPlayer()
    {
        return playerLibrary.GetPlayerController.gameObject;
    }

    public void SetLevelService(ILevelService levelService)
    {
        this.levelService = levelService;
    }
}

