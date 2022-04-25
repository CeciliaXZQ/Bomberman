using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    void Awake()
    {
        GameManager.singleton.restartGame += RestartGame;
    }
    void RestartGame()
    {
        Destroy(gameObject);
    }
    //private void OnDisable()
    //{
    //    GameManager.singleton.restartGame -= RestartGame;
    //}

    private void OnDestroy()
    {
        GameManager.singleton.restartGame -= RestartGame;
    }

}
