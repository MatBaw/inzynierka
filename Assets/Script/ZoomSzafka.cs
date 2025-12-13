using UnityEngine;

public class ZoomCabinet : MonoBehaviour
{
    public Sprite normalSprite;   // daleka perspektywa
    public Sprite zoomSprite;     // bliska perspektywa

    SpriteRenderer sr;
    CameraZoomController zoomController;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        zoomController = FindObjectOfType<CameraZoomController>();

        if (normalSprite == null && sr != null)
            normalSprite = sr.sprite;
    }

    void OnMouseDown()
    {
        if (zoomController == null || sr == null) return;

        // Zoom tylko jeśli jeszcze NIE jesteśmy zbliżeni
        if (!zoomController.IsZoomed)
        {
            sr.sprite = zoomSprite;
            zoomController.ZoomTo(transform);
        }
    }

    // wywołamy to z zewnątrz przy wyjściu z zooma
    public void ResetSprite()
    {
        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }
}
