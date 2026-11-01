using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public GameObject[] obstaclePrefabs; // 3종류 프리팹
    public int obstacleCount = 20;       // 생성할 개수
    public Vector3 spawnAreaSize = new Vector3(50, 1, 50); // 배치 영역

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnObstacles();
    }

    void SpawnObstacles()
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                0f,
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2) + 10f
            );

            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject obstacle = Instantiate(obstaclePrefabs[randomIndex], randomPos, Quaternion.identity);

            obstacle.tag = "Obstacle";
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
