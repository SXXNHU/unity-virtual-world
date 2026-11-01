using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x, z;
    public bool isBlocked = false;

    private Renderer rend;
    private Color baseColor;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.material = new Material(rend.material); // 인스턴스화
        baseColor = rend.material.color;
    }

    public void SetColor(string colorName)
    {
        if (rend == null) return;

        Color targetColor = baseColor;

        switch (colorName.ToLower())
        {
            case "red":
                targetColor = new Color(1f, 0f, 0f, 0.4f); // 반투명 빨강
                break;
            case "green":
                targetColor = new Color(0f, 1f, 0f, 0.4f); // 반투명 초록
                break;
            case "blue":
                targetColor = new Color(0f, 0f, 1f, 0.4f); // 반투명 파랑
                break;
            case "yellow":
                targetColor = new Color(1f, 1f, 0f, 0.4f); // 반투명 노랑 (경로)
                break;
            case "white":
                targetColor = new Color(1f, 1f, 1f, 0.3f); // 반투명 흰색 (visited)
                break;
            default:
                Debug.LogWarning($"[GridCell:{gameObject.name}] 알 수 없는 색상 '{colorName}' 입력됨. 기본 색상으로 유지.");
                break;
        }

        rend.material.color = targetColor;
    }


    public void ResetColor()
    {
        if (rend != null)
            rend.material.color = baseColor;
    }
}
