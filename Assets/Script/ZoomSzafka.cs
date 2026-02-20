using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ZoomSzafka : MonoBehaviour
{
    public Sprite normalSprite;      // widok z daleka
    public Sprite zoomSprite;        // widok zbliżony
    public GameObject zoomRoot;      // np. szafkamala_inna_perspektywa_0 (Drawers w środku)

    private SpriteRenderer sr;
    private BoxCollider2D bigCollider;
    private CameraZoomController zoomController;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        bigCollider = GetComponent<BoxCollider2D>();
        zoomController = FindFirstObjectByType<CameraZoomController>();

        if (normalSprite == null && sr != null)
            normalSprite = sr.sprite;

        if (zoomRoot != null)
            zoomRoot.SetActive(false);   // wnętrze wyłączone na starcie
    }

    void OnMouseDown()
    {
        if (zoomController == null) return;

        // TYLKO wejście w zoom, jeśli jeszcze nie jesteśmy zzoomowani
        if (!zoomController.IsZoomed)
        {
            ZoomIn();
        }
    }

    public void ZoomIn()
    {
        if (zoomController == null) return;

        if (sr != null && zoomSprite != null)
            sr.sprite = zoomSprite;

        if (zoomRoot != null)
            zoomRoot.SetActive(true);

        if (bigCollider != null)
            bigCollider.enabled = false;  // duży collider nas już nie obchodzi

        zoomController.ZoomTo(transform);
    }

    // wywoływane przy wychodzeniu z zooma
    public void ResetSprite()
    {
        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;

        if (zoomRoot != null)
            zoomRoot.SetActive(false);

        if (bigCollider != null)
            bigCollider.enabled = true;
    }
}
