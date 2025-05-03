using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Camera mainCamera;

    private List<Vector3> points = new List<Vector3>();
    private HashSet<Collider2D> alreadyHit = new HashSet<Collider2D>();

    private void Update()
    {
        bool isDrawing = Input.GetMouseButton(0) || Input.touchCount > 0;

        if (isDrawing)
        {
            Vector3 inputPos = Input.touchCount > 0
                ? (Vector3)Input.GetTouch(0).position
                : Input.mousePosition;

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(inputPos);
            worldPos.z = 0f;

            if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], worldPos) > 0.1f)
            {
                AddPoint(worldPos);
                CheckSegmentCollision();
            }
        }
        else if (points.Count > 1)
        {
           ClearLine();
        }
    }

    void AddPoint(Vector3 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    void CheckSegmentCollision()
    {
        if (points.Count < 2) return;

        Vector2 start = points[points.Count - 2];
        Vector2 end = points[points.Count - 1];
        Vector2 dir = (end - start).normalized;
        float dist = Vector2.Distance(start, end);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(start, 0.05f, dir, dist);
        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Circle") && !alreadyHit.Contains(hit.collider))
            {
                alreadyHit.Add(hit.collider);
                Destroy(hit.collider.gameObject);
            }
        }
    }

    public void ClearLine()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
        alreadyHit.Clear();
    }
}
