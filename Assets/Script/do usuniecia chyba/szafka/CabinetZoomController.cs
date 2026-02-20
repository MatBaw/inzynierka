using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class CabinetZoomController : MonoBehaviour
{
    public GameObject zoomViewRoot;              // ZoomView
    public CameraZoomController zoomController;  // możesz zostawić puste -> znajdzie

    private BoxCollider2D bigCollider;
    private bool isZoomed = false;

    void Awake()
    {
        bigCollider = GetComponent<BoxCollider2D>();

        if (zoomController == null)
            zoomController = FindFirstObjectByType<CameraZoomController>();

        if (zoomViewRoot != null)
            zoomViewRoot.SetActive(false);   // wnętrze wyłączone na start
    }

    void OnMouseDown()
    {
        if (zoomController == null) return;

        // ⬇⬇⬇ WAŻNE: TYLKO ZOOM IN, ŻADNEGO ZOOM OUT TUTAJ
        if (!isZoomed && !zoomController.IsZoomed)
        {
            ZoomIn();
        }
        // jeśli już jesteśmy zzoomowani na szafkę -> klik ignorujemy
    }

    public void ZoomIn()
    {
        if (isZoomed) return;
        isZoomed = true;

        if (zoomViewRoot != null)
            zoomViewRoot.SetActive(true);     // włącz inny widok + szuflady

        if (bigCollider != null)
            bigCollider.enabled = false;      // duży collider wyłączony w zoomie

        if (zoomController != null)
            zoomController.ZoomTo(transform);
    }

    public void ZoomOut()
    {
        if (!isZoomed) return;
        isZoomed = false;

        if (zoomViewRoot != null)
            zoomViewRoot.SetActive(false);    // wyłącz widok zoom

        if (bigCollider != null)
            bigCollider.enabled = true;       // znów łapie klik do zooma

        if (zoomController != null)
            zoomController.ZoomOut();
    }
}
