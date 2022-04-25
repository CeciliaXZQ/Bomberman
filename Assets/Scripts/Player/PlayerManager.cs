using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : IPlayerService
{
    private PlayerLibrary playerLibrary;
    private PlayerController playerController;
    private Bomb bombPrefab;
    private SuperBomb superBombPrefab;
    private ILevelService levelService;
    private GameManager gameManager;

    public PlayerManager(PlayerController playerController, Bomb bombPrefab, SuperBomb superBombPrefab, GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.playerController = playerController;
        this.bombPrefab = bombPrefab;
        this.superBombPrefab = superBombPrefab;
        gameManager.restartGame += RestartGame;
    }

    ~PlayerManager()
    {
        gameManager.restartGame -= RestartGame;
    }

    public void SpawnPlayer(Vector2 spawnPos)
    {
        playerLibrary = new PlayerLibrary(playerController, bombPrefab.gameObject, superBombPrefab.gameObject, spawnPos, this, levelService);
    }
    public int GetHealth()
    {
        return playerController.health;
    }
    public void PlayerKilled()
    {
        gameManager.SetGameStatus(false);
        playerLibrary = null;
    }
    public void PlayerDamaged(int health)
    {
        gameManager.UpdateHealth(health);
    }

    void RestartGame()
    {
        if (playerLibrary != null)
        {
            playerLibrary.PlayerDestroy();
            playerLibrary = null;
        }
    }

    public void PlayerSurvived()
    {
        Debug.Log("WIN!!");
        gameManager.SetGameStatus(true);
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

