using UnityEngine;

public class FoodDisplay : MonoBehaviour
{
    [Header("프리팹 목록")]
    public GameObject[] foodPrefabs;

    [Header("사용할 카메라")]
    public Camera mainCamera;

    private GameObject currentFoodObj;
    private int currentIndex = 0;

    void Start()
    {
        ShowNextFood();

        // Null 체크: mainCamera
        if (mainCamera == null)
        {
            Debug.LogError("[FoodDisplay] mainCamera가 할당되지 않았습니다. 스크립트가 있는 오브젝트에서 Camera를 Drag & Drop 해주세요.");
        }

        // Null 체크: foodPrefabs
        if (foodPrefabs == null || foodPrefabs.Length == 0)
        {
            Debug.LogError("[FoodDisplay] foodPrefabs가 비어 있거나 null입니다. Inspector에서 프리팹들을 할당해주세요.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 우클릭 시
        {
            ShowNextFood();
        }
    }

    void ShowNextFood()
    {
        // 실행 전 예외 방지 체크
        if (mainCamera == null || foodPrefabs == null || foodPrefabs.Length == 0)
        {
            Debug.LogWarning("[FoodDisplay] ShowNextFood 실행 조건이 충족되지 않음 (mainCamera 또는 foodPrefabs 문제)");
            return;
        }

        // 기존 표시된 오브젝트 삭제
        if (currentFoodObj != null)
        {
            Destroy(currentFoodObj);
        }

        // 새 오브젝트 생성
        GameObject prefab = foodPrefabs[currentIndex];
        if (prefab == null)
        {
            Debug.LogWarning($"[FoodDisplay] foodPrefabs[{currentIndex}] 프리팹이 null입니다. Inspector에서 확인해주세요.");
            return;
        }

        currentFoodObj = Instantiate(prefab, mainCamera.transform);
        currentFoodObj.transform.localPosition = new Vector3(1.7f, -0.6f, 1.5f); // 카메라 기준 위치
        currentFoodObj.transform.localRotation = Quaternion.identity;
        currentFoodObj.transform.localScale = Vector3.one * 0.5f;

        // 다음 인덱스로 이동
        currentIndex = (currentIndex + 1) % foodPrefabs.Length;
    }
}
