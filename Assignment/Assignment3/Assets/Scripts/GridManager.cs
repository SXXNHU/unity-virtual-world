using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab;
    public Transform ground;
    public int gridSize = 11;

    private GameObject[,] gridCells;
    private bool gridVisible = true;

    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGridVisibility();
        }
    }

    void GenerateGrid()
    {
        if (ground == null || gridCellPrefab == null)
        {
            //Debug.LogError("Ground³ª Prefab ¿¬°á ¾È µÊ");
            return;
        }

        float groundSize = ground.localScale.x * 10f;
        float spacing = groundSize / gridSize;

        Vector3 origin = ground.position - new Vector3(groundSize, 0, groundSize) / 2f + new Vector3(spacing / 2, 0, spacing / 2);

        gridCells = new GameObject[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 pos = origin + new Vector3(x * spacing, 0, z * spacing);
                GameObject cell = Instantiate(gridCellPrefab, pos, Quaternion.identity, transform);
                cell.name = $"Cell_{x}_{z}";

                GridCell cellData = cell.GetComponent<GridCell>();
                if (cellData != null)
                {
                    cellData.x = x;
                    cellData.z = z;
                    cellData.isBlocked = !IsInsideBounds(x, z);
                }

                gridCells[x, z] = cell;
            }
        }

    }

    void ToggleGridVisibility()
    {
        gridVisible = !gridVisible;

        foreach (GameObject cell in gridCells)
        {
            if (cell != null)
                cell.SetActive(gridVisible);
        }
    }

    public List<GridCell> GetAllCells()
    {
        List<GridCell> cells = new List<GridCell>();
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (gridCells[x, z] != null)
                {
                    GridCell cellData = gridCells[x, z].GetComponent<GridCell>();
                    if (cellData != null && IsInsideBounds(cellData.x, cellData.z))
                        cells.Add(cellData);
                }
            }
        }
        return cells;
    }


    public GridCell GetCell(int x, int z)
    {
        if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
        {
            return gridCells[x, z]?.GetComponent<GridCell>();
        }
        return null;
    }

    public GridCell FindNearestCell(Vector3 position)
    {
        GridCell nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GridCell cell in GetAllCells())
        {
            float dist = Vector3.Distance(position, cell.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = cell;
            }
        }
        return nearest;
    }
    public bool IsInsideBounds(int x, int z)
    {
        return x >= 0 && x < gridSize && z >= 0 && z < gridSize;
    }

}
