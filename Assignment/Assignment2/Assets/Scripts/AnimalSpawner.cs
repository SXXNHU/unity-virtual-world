using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    public GameObject hungerBarPrefab;
    public RectTransform miniMapRect;
    public Transform cameraTransform;

    void Start()
    {
        Debug.Log("Start 호출됨 - AnimalSpawner");
        InvokeRepeating("SpawnAnimals", 0f, 5f);  // 5초마다 반복
    }

    void SpawnAnimals()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject prefab = animalPrefabs[Random.Range(0, animalPrefabs.Length)];
            Vector3 pos = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f) + 10f);
            GameObject animalObj = Instantiate(prefab, pos, Quaternion.identity);
            Animal animal = animalObj.GetComponent<Animal>();

            if (animal == null)
            {
                Debug.LogError("Animal 스크립트가 프리팹에 없습니다");
                return;
            }

            // HungerBar 생성
            GameObject hungerBarObj = Instantiate(hungerBarPrefab);
            hungerBarObj.transform.SetParent(animalObj.transform);

            // 머리 위 위치 (월드 기준)
            Vector3 offset = new Vector3(0, 4f, 0);
            hungerBarObj.transform.position = animalObj.transform.position + offset;
            hungerBarObj.transform.localRotation = Quaternion.identity;

            HungerBar hungerBar = hungerBarObj.GetComponent<HungerBar>();
            if (hungerBar == null)
            {
                Debug.LogError("HungerBar 스크립트가 프리팹에 없습니다");
                return;
            }

            hungerBar.Initialize(animal, cameraTransform);
            animal.SetHungerBar(hungerBar);
        }
    }
}
