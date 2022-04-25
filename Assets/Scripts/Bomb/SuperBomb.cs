using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBomb : MonoBehaviour
{
    ILevelService levelService;

    [SerializeField] private int explosionArea = 0;
    [SerializeField] private GameObject explosionObj;
    private int timer = 0;
    private int hitCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyController>() != null)
        {
            hitCount++;
            if (hitCount <= 1)
            {
                InvokeRepeating("Explode", 1f, 1f);
            }
        }
    }

    void Start()
    {
        GameManager.singleton.restartGame += RestartGame;
        hitCount = 0;
    }

    void Update()
    {
        if (timer >= 10)
        {
            CancelInvoke("Explode");
            levelService.EmptyGrid(transform.position);
            Destroy(gameObject);
        }
    }

    public void SetLevelService(ILevelService levelService)
    {
        this.levelService = levelService;
        this.levelService.FillGrid(transform.position, this.gameObject);
    }

    void Explode()
    {
        ExplodeCell(transform.position, 0, Vector3.up);
        ExplodeCell(transform.position, 0, Vector3.left);
        ExplodeCell(transform.position, 0, Vector3.right);
        ExplodeCell(transform.position, 0, Vector3.down);
        timer++;
    }
    void ExplodeCell(Vector3 position, int areaCovered, Vector3 explosionDirection)
    {
        int area = areaCovered;
        Vector2 targetPos = position + explosionDirection;
        GameObject obj = levelService.GetObjAtGrid(targetPos);
        area++;
        if (obj != null)
        {
            if (obj.GetComponent<FixedBlocks>() != null) return;
            else
            {
                Instantiate(explosionObj, targetPos, Quaternion.identity);
                levelService.EmptyGrid(position);

                if (area < explosionArea)
                {
                    ExplodeCell(transform.position + explosionDirection * area, area, explosionDirection);
                }
            }
        }
        else
        {
            Instantiate(explosionObj, targetPos, Quaternion.identity);
            if (area < explosionArea)
            {
                ExplodeCell(transform.position + explosionDirection * area, area, explosionDirection);
            }
        }

    }
    void OnDestroy()
    {
        CancelInvoke("Explode");
    }
    void RestartGame()
    {
        if (gameObject != null)
        {
            CancelInvoke("Explode");
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        GameManager.singleton.restartGame -= RestartGame;
    }
}
