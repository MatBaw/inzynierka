using UnityEngine;

public class ZoomTarget : MonoBehaviour
{
    CameraZoomController zoomController;

    void Start()
    {
        zoomController = FindObjectOfType<CameraZoomController>();
    }

    void OnMouseDown()
    {
        ZoomNow(); // klik myszą dalej działa
    }

    // TO JEST DO WZROKU (UnityEvent):
    public void ZoomNow()
    {
        if (zoomController != null)
        {
            zoomController.ZoomTo(transform);
        }
    }
}
