using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public static MiniMapCamera Instance;

    public Camera miniMapCam;               // 미니맵 전용 카메라
    public RectTransform miniMapRect;      // RawImage의 RectTransform

    void Awake()
    {
        Instance = this;
    }

    public static Vector3 WorldToMiniMapPosition(Vector3 worldPos)
    {
        if (Instance == null || Instance.miniMapCam == null || Instance.miniMapRect == null)
            return Vector3.zero;

        Vector3 viewportPos = Instance.miniMapCam.WorldToViewportPoint(worldPos);
        Vector2 size = Instance.miniMapRect.sizeDelta;

        return new Vector3(
            (viewportPos.x - 0.5f) * size.x,
            (viewportPos.y - 0.5f) * size.y,
            0f
        );
    }
}
