using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLibrary
{
    private PlayerController playerController;
    private PlayerManager playerManager;
    private GameObject bombPrefab;
    private GameObject superBombPrefab;
    private ILevelService levelService;

    public PlayerController GetPlayerController { get { return playerController; } }

    private GameObject lastBomb = null;
    private GameObject lastSuperBomb = null;

    public PlayerLibrary(PlayerController playerPref, GameObject bombPrefab, GameObject superBombPrefab
                            , Vector2 pos, PlayerManager playerManager
        , ILevelService levelService)
    {
        this.levelService = levelService;
        this.playerManager = playerManager;
        this.bombPrefab = bombPrefab;
        this.superBombPrefab = superBombPrefab;
        GameObject player = Object.Instantiate(playerPref.gameObject, pos, Quaternion.identity);
        playerController = player.GetComponent<PlayerController>();
        playerController.GetLibrary(this);
    }

    public void SpawnBomb()
    {
        Vector2 spawnPOs = playerController.transform.position;
        spawnPOs.x = Mathf.Round(spawnPOs.x);
        spawnPOs.y = Mathf.Round(spawnPOs.y);
        if (lastBomb == null)
        {
            lastBomb = Object.Instantiate(bombPrefab, spawnPOs, Quaternion.identity);
            lastBomb.GetComponent<Bomb>().SetLevelService(levelService);
        }
    }

    public void SpawnSuperBomb()
    {
        Vector2 spawnPOs = playerController.transform.position;
        spawnPOs.x = Mathf.Round(spawnPOs.x);
        spawnPOs.y = Mathf.Round(spawnPOs.y);
        if (lastSuperBomb == null)
        {
            lastSuperBomb = Object.Instantiate(superBombPrefab, spawnPOs, Quaternion.identity);
            lastSuperBomb.GetComponent<SuperBomb>().SetLevelService(levelService);
        }
    }

    public void PlayerKilled()
    {
        Object.Destroy(playerController.gameObject);
        playerManager.PlayerKilled();
    }
    public void PlayerDamaged()
    {
        playerController.health -= 10;
        playerManager.PlayerDamaged(playerController.health);
        if (playerController.health <= 0)
        {
            PlayerKilled();
        }
    }

    public void PlayerDestroy()
    {
        Object.Destroy(playerController.gameObject);

    }

    public void PlayerSurvived()
    {
        playerManager.PlayerSurvived();
    }
}
