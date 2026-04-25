using UnityEngine;
using UnityEngine.Events;

public class ZoomTarget : MonoBehaviour
{
    CameraZoomController zoomController;

    [Header("Per-object zoom settings")]
    [SerializeField] Transform zoomPoint;
    [SerializeField] float customZoomSize = 3f;

    [Header("Mouse")]
    [SerializeField] bool enableMouseClick = false;

    [Header("Events")]
    public UnityEvent onZoom;

    void Start()
    {
        zoomController = FindFirstObjectByType<CameraZoomController>();
        if (zoomPoint == null) zoomPoint = transform;
    }

    void OnMouseDown()
    {
        if (!enableMouseClick) return;
        ZoomNow();
    }

public void ZoomNow()
{
    if (zoomController != null && zoomPoint != null)
    {
        zoomController.ZoomTo(zoomPoint, transform, customZoomSize);
        onZoom?.Invoke();
    }
}
}