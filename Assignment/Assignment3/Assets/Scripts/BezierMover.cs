using UnityEngine;
using System.Collections.Generic;

public class BezierMover : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    public GameObject bezierLinePrefab;
    public GameObject controlPointPrefab;

    [Header("Settings")]
    public float moveDuration = 3f;

    private List<Vector3> controlPoints = new List<Vector3>();
    private List<GameObject> visualPoints = new List<GameObject>();
    private float t = 0f;
    private bool isMoving = false;

    private LineRenderer lineRenderer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isMoving)
            {
                CancelMovement();
            }
            else
            {
                StartBezierMovement();
            }
        }

        if (isMoving)
        {
            t += Time.deltaTime / moveDuration;

            if (t >= 1f)
            {
                FinishMovement();
                return;
            }

            Vector3 pos = CalculateBezier(t);
            transform.position = pos;

            // 키 입력 감지 → 강제 중단
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                CancelMovement();
            }
        }
    }

    void StartBezierMovement()
    {
        ClearVisuals();
        controlPoints.Clear();

        // 첫 포인트는 플레이어 현재 위치
        controlPoints.Add(transform.position);

        // 나머지 3개는 랜덤한 GridCell
        List<GridCell> cells = gridManager.GetAllCells();
        for (int i = 1; i < 4; i++)
        {
            int idx = Random.Range(0, cells.Count);
            controlPoints.Add(cells[idx].transform.position);
        }

        // 컨트롤 포인트 시각화
        foreach (var point in controlPoints)
        {
            GameObject cp = Instantiate(controlPointPrefab, point + Vector3.up * 0.2f, Quaternion.identity);
            visualPoints.Add(cp);
        }

        DrawBezierLine();

        // 라인 시각화
        GameObject lineObj = Instantiate(bezierLinePrefab);
        lineRenderer = lineObj.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 30;

        for (int i = 0; i < 30; i++)
        {
            float tt = i / 29f;
            lineRenderer.SetPosition(i, CalculateBezier(tt));
        }

        visualPoints.Add(lineObj);
        t = 0f;
        isMoving = true;
    }

    Vector3 CalculateBezier(float t)
    {
        Vector3 p0 = controlPoints[0];
        Vector3 p1 = controlPoints[1];
        Vector3 p2 = controlPoints[2];
        Vector3 p3 = controlPoints[3];

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * p0;
        point += 3 * uu * t * p1;
        point += 3 * u * tt * p2;
        point += ttt * p3;

        return point;
    }
    void DrawBezierLine()
    {
        GameObject go = Instantiate(bezierLinePrefab);
        LineRenderer line = go.GetComponent<LineRenderer>();
        line.positionCount = 30;

        for (int i = 0; i < 30; i++)
        {
            float t = i / 29f;
            line.SetPosition(i, CalculateBezier(t));
        }

        visualPoints.Add(go);
    }

    void CancelMovement()
    {
        isMoving = false;
        ClearVisuals();
    }

    void FinishMovement()
    {
        isMoving = false;
        ClearVisuals();
    }

    void ClearVisuals()
    {
        foreach (GameObject go in visualPoints)
        {
            Destroy(go);
        }
        visualPoints.Clear();
    }

}
