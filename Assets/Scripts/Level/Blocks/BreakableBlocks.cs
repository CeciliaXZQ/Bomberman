using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlocks : MonoBehaviour, IDamagable
{
    ILevelService levelService;
    GameObject hiddenDoor = null;

    public void SetLevelService(ILevelService levelService)
    {
        this.levelService = levelService;
    }

    public void SetDoorHiddenBehind(GameObject hiddenDoor)
    {
        this.hiddenDoor = hiddenDoor;
    }

    public void Damage()
    {
        levelService.EmptyGrid(transform.position);
        Destroy(gameObject);
        if (hiddenDoor != null)
        {
            hiddenDoor.SetActive(true);
        }
    }
}