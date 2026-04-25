using UnityEngine;
using UnityEngine.UI;

public class EyeTrackingToggle : MonoBehaviour
{
    [Header("KOMPONENTY DO WYŁĄCZANIA")]
    [SerializeField] GazeDwellClick2D dwellClick2D;
    [SerializeField] GazeDwellClickUI dwellClickUI;
    [SerializeField] GazeEdgeButtons gazeEdgeButtons;

    [Header("UI IKONKI")]
    [SerializeField] Sprite eyeOpenSprite;
    [SerializeField] Sprite eyeClosedSprite;
    [SerializeField] Image buttonImage;

    [Header("KOLORY PRZYCISKU")]
    [SerializeField] Color activeColor   = new Color(0.3f, 0.8f, 1f, 0.9f);
    [SerializeField] Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);

    [Header("CURSOR UI")]
    [SerializeField] GazeCursorRingUI cursorUI;

    [Header("=== DWELL CZAS TOGGLE ===")]
    [SerializeField] float toggleDwellSeconds = 1.5f;

    private bool isEyeTrackingActive = true;
    private float dwellTimer = 0f;
    private bool isGazingAtButton = false;

    public static EyeTrackingToggle Instance { get; private set; }
    public bool IsActive => isEyeTrackingActive;

    void Awake()
    {
        Instance = this;
        if (dwellClick2D == null)    dwellClick2D    = FindFirstObjectByType<GazeDwellClick2D>();
        if (dwellClickUI == null)    dwellClickUI    = FindFirstObjectByType<GazeDwellClickUI>();
        if (gazeEdgeButtons == null) gazeEdgeButtons = FindFirstObjectByType<GazeEdgeButtons>();
        if (cursorUI == null)        cursorUI        = FindFirstObjectByType<GazeCursorRingUI>();
        if (buttonImage == null)     buttonImage     = GetComponent<Image>();
    }

    void Start()
    {
        isEyeTrackingActive = true;
        UpdateButtonVisuals();
        Debug.Log("[EyeToggle] Start — tracking WŁĄCZONY");
    }

    public void Toggle()
    {
        isEyeTrackingActive = !isEyeTrackingActive;

        // Włącz/wyłącz komponenty
        if (dwellClick2D != null)    dwellClick2D.enabled    = isEyeTrackingActive;
        if (dwellClickUI != null)    dwellClickUI.enabled    = isEyeTrackingActive;
        if (gazeEdgeButtons != null) gazeEdgeButtons.enabled = isEyeTrackingActive;

        UpdateButtonVisuals();

        dwellTimer = 0f;
        isGazingAtButton = false;

        if (!isEyeTrackingActive)
            cursorUI?.SetIdle();

        Debug.Log($"[EyeToggle] Tracking: {(isEyeTrackingActive ? "WŁĄCZONY" : "WYŁĄCZONY")}");
    }

    void Update()
    {
        if (!isEyeTrackingActive)
            HandleManualDwellWhenDisabled();
    }

    void HandleManualDwellWhenDisabled()
    {
        if (cursorUI == null) return;
        var gazeDot = FindGazeDot();
        if (gazeDot == null) return;

        bool overButton = IsGazeDotOverButton(gazeDot);

        if (overButton != isGazingAtButton)
        {
            isGazingAtButton = overButton;
            dwellTimer = 0f;
            if (!isGazingAtButton) cursorUI.SetIdle();
        }

        if (!isGazingAtButton) return;

        dwellTimer += Time.deltaTime;
        float progress = toggleDwellSeconds <= 0f ? 1f : dwellTimer / toggleDwellSeconds;
        cursorUI.SetHoverProgress(progress);

        if (dwellTimer >= toggleDwellSeconds)
            Toggle();
    }

    void UpdateButtonVisuals()
    {
        if (buttonImage == null) return;
        buttonImage.color = isEyeTrackingActive ? activeColor : inactiveColor;
        if (isEyeTrackingActive && eyeOpenSprite != null)
            buttonImage.sprite = eyeOpenSprite;
        else if (!isEyeTrackingActive && eyeClosedSprite != null)
            buttonImage.sprite = eyeClosedSprite;
    }

    bool IsGazeDotOverButton(RectTransform gazeDot)
    {
        var rt = GetComponent<RectTransform>();
        if (rt == null) return false;
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(null, gazeDot.position);
        return RectTransformUtility.RectangleContainsScreenPoint(rt, screen);
    }

    RectTransform FindGazeDot()
    {
        var go = GameObject.Find("GazeDot");
        return go != null ? go.GetComponent<RectTransform>() : null;
    }
}