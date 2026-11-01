using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public Transform player;           // 플레이어 Transform
    public Transform miniMapCamera;    // 미니맵 카메라 Transform

    private bool rotateMiniMap = true; // true: Mini-map 1, false: Mini-map 2

    void Update()
    {
        // R키 누르면 모드 토글
        if (Input.GetKeyDown(KeyCode.R))
        {
            rotateMiniMap = !rotateMiniMap;
        }

        if (rotateMiniMap) // Mini-map 1
        {
            miniMapCamera.rotation = Quaternion.Euler(90, player.eulerAngles.y, 0);
        }
        else // Mini-map 2 (고정 미니맵)
        {
            miniMapCamera.rotation = Quaternion.Euler(90, 0, 0);
        }
    }
}
