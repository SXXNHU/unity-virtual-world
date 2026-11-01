using UnityEngine;
using System.Collections.Generic;

public class GridHighlighter : MonoBehaviour
{
    public GridManager gridManager;
    public Transform player;
    public LayerMask gridCellLayer;

    void Update()
    {
        ResetAllColors();

        HighlightNearest(player.position, "green");

        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");
        foreach (GameObject animal in animals)
        {
            HighlightNearest(animal.transform.position, "blue");
        }

        List<GridCell> allCells = gridManager.GetAllCells();
        foreach (GridCell cell in allCells)
        {
            if (cell.isBlocked)
            {
                cell.SetColor("red");
                AddBlockingCollider(cell);
            }
        }
    }

    void HighlightNearest(Vector3 position, string colorName)
    {
        GridCell nearest = FindNearestCell(position);
        if (nearest != null && !nearest.isBlocked)
        {
            Debug.Log($"하이라이트: {nearest.name} → {colorName}");
            nearest.SetColor(colorName);
        }
    }

    GridCell FindNearestCell(Vector3 position)
    {
        GridCell nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GridCell cell in gridManager.GetAllCells())
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

    void ResetAllColors()
    {
        foreach (GridCell cell in gridManager.GetAllCells())
        {
            cell.ResetColor();
        }
    }

    void AddBlockingCollider(GridCell cell)
    {
        BoxCollider col = cell.GetComponent<BoxCollider>();
        if (col == null)
        {
            col = cell.gameObject.AddComponent<BoxCollider>();
            col.size = new Vector3(1, 2, 1);
            col.center = Vector3.up;
        }
    }
}
