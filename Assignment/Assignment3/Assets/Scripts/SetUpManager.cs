using UnityEngine;
using System.Collections.Generic;

public class SetUpManager : MonoBehaviour
{
    public GridManager gridManager;

    public GameObject[] obstaclePrefabs;
    public GameObject foxPrefab, dogPrefab, horsePrefab;

    public float obstacleProbability = 0.3f;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        Debug.Log("SetUpManager ½ÃÀÛµÊ");
        SetUpMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMap();
        }
    }

    void SetUpMap()
    {
        List<GridCell> allCells = gridManager.GetAllCells();
        List<GridCell> spawnableCells = new List<GridCell>(); 
        Debug.Log("¼¿ °³¼ö: " + allCells.Count);

        foreach (GridCell cell in allCells)
        {
            if (Random.value < obstacleProbability)
            {
                GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                GameObject obj = Instantiate(prefab, cell.transform.position, Quaternion.identity);
                spawnedObjects.Add(obj);
                cell.isBlocked = true;
            }
            else
            {
                cell.isBlocked = false;
                spawnableCells.Add(cell);
            }
        }

        SpawnAnimal(foxPrefab, spawnableCells);
        SpawnAnimal(dogPrefab, spawnableCells);
        SpawnAnimal(horsePrefab, spawnableCells);
    }


    void SpawnAnimal(GameObject prefab, List<GridCell> cells)
    {
        for (int i = 0; i < 100; i++) 
        {
            GridCell cell = cells[Random.Range(0, cells.Count)];

            if (!cell.isBlocked && gridManager.IsInsideBounds(cell.x, cell.z))
            {
                GameObject obj = Instantiate(prefab, cell.transform.position, Quaternion.identity);
                spawnedObjects.Add(obj);
                cell.isBlocked = true;
                return;
            }
        }

        Debug.LogWarning("ºó ¼¿À» Ã£Áö ¸øÇß½À´Ï´Ù!");
    }


    void ResetMap()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedObjects.Clear();

        SetUpMap();
    }
}
