using UnityEngine;
using System.Collections.Generic;
using TMPro;
using static FeederController;

public enum AnimalType { Dog, Horse, Fox }

public class AnimalAI : MonoBehaviour
{
    [Header("설정")]
    public AnimalType type;
    public GridManager gridManager;
    public Transform player;
    public float moveSpeed = 2f;
    public float rotateSpeed = 5f;

    private List<GridCell> currentPath = new();
    private int pathIndex = 0;
    private GridCell lastPlayerCell;
    private GridCell lastAnimalCell;
    private FeederController feeder;

    public static PathfindingMode currentMode = PathfindingMode.BFS;

    [Header("UI")]
    public TextMeshProUGUI algorithmText;

    float HorizontalDistance(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;
        return Vector3.Distance(a, b);
    }

    void Awake()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
                Debug.Log($"[AnimalAI] player 자동 연결됨: {player.name}");
            }
        }

        if (gridManager == null)
        {
            gridManager = FindObjectOfType<GridManager>();
        }

        if (algorithmText == null)
        {
            algorithmText = GameObject.Find("AlgorithmText")?.GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        feeder = player.GetComponent<FeederController>();
    }

    void Update()
    {
        if (!ShouldFollowPlayer())
            return;

        UpdatePathIfNeeded();
        FollowPath();
    }

    bool ShouldFollowPlayer()
    {
        if (feeder == null) return false;

        FoodType currentFoodType = feeder.GetCurrentFoodType();
        return (currentFoodType == FoodType.Bone && type == AnimalType.Dog) ||
               (currentFoodType == FoodType.Carrot && type == AnimalType.Horse) ||
               (currentFoodType == FoodType.Apple && type == AnimalType.Fox);
    }

    void UpdatePathIfNeeded()
    {
        GridCell playerCell = gridManager.FindNearestCell(player.position);
        GridCell animalCell = gridManager.FindNearestCell(transform.position);
        if (playerCell == null || animalCell == null) return;

        if (playerCell != lastPlayerCell || animalCell != lastAnimalCell)
        {
            lastPlayerCell = playerCell;
            lastAnimalCell = animalCell;

            PathfindingMode mode = PathfindingManager.Instance.GetCurrentMode();

            if (mode == PathfindingMode.BFS)
            {
                currentPath = FindPathBFS(animalCell, playerCell);
            }
            else if (mode == PathfindingMode.Greedy)
            {
                currentPath = FindPathGreedy(animalCell, playerCell);
            }
            else if (mode == PathfindingMode.AStar)
            {
                currentPath = FindPathAStar(animalCell, playerCell);
            }

            pathIndex = 0;
            HighlightPath(currentPath);
        }
    }


    void FollowPath()
    {
        if (currentPath == null || currentPath.Count == 0) return;

        if (pathIndex >= currentPath.Count)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);
            if (distToPlayer < 1.5f)
            {
                Vector3 dir = player.position - transform.position;
                if (dir != Vector3.zero)
                {
                    Quaternion rot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);
                }
                return;
            }
        }

        if (pathIndex < currentPath.Count)
        {
            GridCell target = currentPath[pathIndex];
            Vector3 targetPos = target.transform.position;
            Vector3 dir = (targetPos - transform.position).normalized;

            if (dir != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotateSpeed * Time.deltaTime);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                pathIndex++;
            }
        }
    }

    List<GridCell> FindPathBFS(GridCell start, GridCell goal)
    {
        Dictionary<GridCell, GridCell> cameFrom = new();
        Queue<GridCell> queue = new();
        queue.Enqueue(start);
        cameFrom[start] = null;

        while (queue.Count > 0)
        {
            GridCell current = queue.Dequeue();
            if (current == goal) break;

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    cameFrom[neighbor] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        return ReconstructPath(cameFrom, goal);
    }

    List<GridCell> FindPathGreedy(GridCell start, GridCell goal)
    {
        PriorityQueue<GridCell> frontier = new();
        Dictionary<GridCell, GridCell> cameFrom = new();
        frontier.Enqueue(start, 0);
        cameFrom[start] = null;

        while (frontier.Count > 0)
        {
            GridCell current = frontier.Dequeue();
            if (current == goal) break;

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    float priority = Vector2Int.Distance(
                        new Vector2Int(neighbor.x, neighbor.z),
                        new Vector2Int(goal.x, goal.z));
                    frontier.Enqueue(neighbor, priority);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return ReconstructPath(cameFrom, goal);
    }

    List<GridCell> FindPathAStar(GridCell start, GridCell goal)
    {
        PriorityQueue<GridCell> frontier = new();
        Dictionary<GridCell, GridCell> cameFrom = new();
        Dictionary<GridCell, float> costSoFar = new();

        frontier.Enqueue(start, 0);
        cameFrom[start] = null;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            GridCell current = frontier.Dequeue();
            if (current == goal) break;

            foreach (var neighbor in GetNeighbors(current))
            {
                float newCost = costSoFar[current] + Vector2Int.Distance(
                    new Vector2Int(current.x, current.z),
                    new Vector2Int(neighbor.x, neighbor.z)
                );

                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;

                    float priority = newCost + Vector2Int.Distance(
                        new Vector2Int(neighbor.x, neighbor.z),
                        new Vector2Int(goal.x, goal.z)
                    );

                    frontier.Enqueue(neighbor, priority);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return ReconstructPath(cameFrom, goal);
    }


    List<GridCell> ReconstructPath(Dictionary<GridCell, GridCell> cameFrom, GridCell goal)
    {
        List<GridCell> path = new();
        GridCell c = goal;
        while (c != null && cameFrom.ContainsKey(c))
        {
            path.Add(c);
            c = cameFrom[c];
        }
        path.Reverse();
        return path;
    }

    List<GridCell> GetNeighbors(GridCell cell)
    {
        List<GridCell> neighbors = new();
        AddIfValid(cell.x + 1, cell.z, neighbors);
        AddIfValid(cell.x - 1, cell.z, neighbors);
        AddIfValid(cell.x, cell.z + 1, neighbors);
        AddIfValid(cell.x, cell.z - 1, neighbors);
        return neighbors;
    }

    void AddIfValid(int x, int z, List<GridCell> list)
    {
        GridCell neighbor = gridManager.GetCell(x, z);
        if (neighbor != null && !neighbor.isBlocked)
        {
            list.Add(neighbor);
        }
    }

    void HighlightPath(List<GridCell> path)
    {
        foreach (var cell in path)
        {
            cell.SetColor("yellow");
        }
    }
}
