using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    ILevelService levelService;

    [SerializeField] private int explosionArea = 0;
    [SerializeField] private GameObject explosionObj;

    void Start()
    {
        Invoke("Explode", 3);
        GameManager.singleton.restartGame += RestartGame;
    }
    public void SetLevelService(ILevelService levelService)
    {
        this.levelService = levelService;
        this.levelService.FillGrid(transform.position, this.gameObject);
    }
    void Explode()
    {
        ExplodeCell(transform.position, 10, Vector3.zero);
        ExplodeCell(transform.position, 0, Vector3.up);
        ExplodeCell(transform.position, 0, Vector3.left);
        ExplodeCell(transform.position, 0, Vector3.right);
        ExplodeCell(transform.position, 0, Vector3.down);

        levelService.EmptyGrid(transform.position);
        Destroy(gameObject);
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
                    ExplodeCell(transform.position + explosionDirection*area, area, explosionDirection);
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

    void RestartGame()
    {
        Destroy(gameObject);
    }
    private void OnDisable()
    {
        GameManager.singleton.restartGame -= RestartGame;
    }
}
