using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ZoomSzafka : MonoBehaviour
{
    [Header("Sprite")]
    public Sprite normalSprite;
    public Sprite zoomSprite;

    [Header("Zoom Root")]
    public GameObject zoomRoot;

    [Header("Cooldown po zoomie")]
    [Tooltip("Jak długo wzrok nie może odzoomować po wejściu w zoom")]
    [SerializeField] float gazeBlockDurationOnZoom = 2.0f;

    private SpriteRenderer sr;
    private BoxCollider2D bigCollider;
    private CameraZoomController zoomController;

    void Awake()
    {
        sr           = GetComponent<SpriteRenderer>();
        bigCollider  = GetComponent<BoxCollider2D>();
        zoomController = FindFirstObjectByType<CameraZoomController>();

        if (normalSprite == null && sr != null)
            normalSprite = sr.sprite;

        if (zoomRoot != null)
            zoomRoot.SetActive(false);
    }

    void OnMouseDown()
    {
        if (zoomController == null) return;
        if (!zoomController.IsZoomed)
            ZoomIn();
    }

    public void ZoomIn()
    {
        if (zoomController == null) return;
        if (zoomController.IsZoomed) return;
        // Zmień sprite
        if (sr != null && zoomSprite != null)
            sr.sprite = zoomSprite;

        if (zoomRoot != null)
            zoomRoot.SetActive(true);

        if (bigCollider != null)
            bigCollider.enabled = false;

        zoomController.ZoomTo(transform);

        if (GazeDwellClick2D.Instance != null)
            GazeDwellClick2D.Instance.TriggerCooldown(gazeBlockDurationOnZoom);

        var clickOutside = FindFirstObjectByType<ClickOutsideToZoomOut>();
        if (clickOutside != null)
            clickOutside.IgnoreNextClick();
    }
    
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
