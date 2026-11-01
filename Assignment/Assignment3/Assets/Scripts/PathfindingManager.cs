using UnityEngine;
using TMPro;

public enum PathfindingMode { BFS, Greedy, AStar }

public class PathfindingManager : MonoBehaviour
{
    public static PathfindingManager Instance;

    public PathfindingMode currentMode = PathfindingMode.BFS;
    public TextMeshProUGUI algorithmText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetMode(PathfindingMode.BFS);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetMode(PathfindingMode.Greedy);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetMode(PathfindingMode.AStar);
        }
    }

    public void SetMode(PathfindingMode mode)
    {
        currentMode = mode;
        if (algorithmText != null)
            algorithmText.text = $"Path Finding Algorithm: {mode}";
        Debug.Log($"[PathfindingManager] 알고리즘 전환됨 → {mode}");
    }

    public PathfindingMode GetCurrentMode()
    {
        return currentMode;
    }
}
