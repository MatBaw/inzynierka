using UnityEngine;

/// <summary>
/// Szafka z zoom-in po kliknięciu myszą LUB po dwell-gaze.
/// Automatycznie blokuje dwell-click na czas po zoomie (żeby wzrok nie odzoomował natychmiast).
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ZoomSzafka : MonoBehaviour
{
    [Header("Sprite")]
    public Sprite normalSprite;
    public Sprite zoomSprite;

    [Header("Zoom Root")]
    public GameObject zoomRoot;  // obiekt aktywowany po zoomie (np. wnętrze szafki)

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

    // Klik myszą
    void OnMouseDown()
    {
        if (zoomController == null) return;
        if (!zoomController.IsZoomed)
            ZoomIn();
    }

    /// <summary>
    /// Wejście w zoom — wywoływane przez klik myszą LUB przez GazeDwellTarget -> UnityEvent
    /// </summary>
    public void ZoomIn()
    {
        if (zoomController == null) return;
        if (zoomController.IsZoomed) return; // już zoomed, ignoruj

        // Zmień sprite
        if (sr != null && zoomSprite != null)
            sr.sprite = zoomSprite;

        // Pokaż wnętrze
        if (zoomRoot != null)
            zoomRoot.SetActive(true);

        // Wyłącz duży collider (kliknięcie w nic nie odzoomuje przez collider szafki)
        if (bigCollider != null)
            bigCollider.enabled = false;

        // Zrób zoom kamery
        zoomController.ZoomTo(transform);

        // --- WAŻNE: zablokuj dwell na chwilę żeby wzrok nie odzoomował natychmiast ---
        if (GazeDwellClick2D.Instance != null)
            GazeDwellClick2D.Instance.TriggerCooldown(gazeBlockDurationOnZoom);

        // Stary system (ClickOutsideToZoomOut) też zignoruje następny klik
        var clickOutside = FindFirstObjectByType<ClickOutsideToZoomOut>();
        if (clickOutside != null)
            clickOutside.IgnoreNextClick();
    }

    /// <summary>
    /// Reset do stanu normalnego — wywoływane przy wychodzeniu z zooma
    /// </summary>
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
