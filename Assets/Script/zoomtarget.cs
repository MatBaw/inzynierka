using UnityEngine;
using UnityEngine.Events;

public class ZoomTarget : MonoBehaviour
{
    CameraZoomController zoomController;

    [Header("Per-object zoom settings")]
    [SerializeField] Transform zoomPoint;
    [SerializeField] float customZoomSize = 3f;

    [Header("Events")]
    public UnityEvent onZoom;   // wywoła się po zoom

    void Start()
    {
        zoomController = FindFirstObjectByType<CameraZoomController>();
        if (zoomPoint == null) zoomPoint = transform;
    }

    void OnMouseDown()
    {
        ZoomNow();
    }

    public void ZoomNow()
    {
        if (zoomController != null && zoomPoint != null)
        {
            zoomController.ZoomTo(zoomPoint, customZoomSize);
            onZoom?.Invoke();
        }
    }
}