using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GazeDwellClickUI : MonoBehaviour
{
    public static GazeDwellClickUI Instance { get; private set; }

    [Header("=== WYMAGANE REFERENCJE ===")]
    [SerializeField] private RectTransform gazeDot;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GazeCursorRingUI cursorUI;

    [Header("=== DWELL CZAS ===")]
    [SerializeField] private float dwellSeconds = 1.2f;

    [Header("=== COOLDOWN ===")]
    [SerializeField] private float cooldownAfterClick = 0.5f;

    [Header("=== DEBUG ===")]
    [SerializeField] private bool showDebugLogs = false;

    private Button currentButton;
    private float dwellTimer;
    private float cooldownTimer;

    public bool IsTracking => currentButton != null;

    private PointerEventData pointerData;
    private readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

    private void Awake()
    {
        Instance = this;

        if (eventSystem == null)
            eventSystem = EventSystem.current;

        if (cursorUI == null && gazeDot != null)
            cursorUI = gazeDot.GetComponent<GazeCursorRingUI>();

        if (eventSystem != null)
            pointerData = new PointerEventData(eventSystem);

        if (eventSystem == null)
            Debug.LogWarning("[GazeDwellClickUI] Brak EventSystem.", this);

        if (cursorUI == null)
            Debug.LogWarning("[GazeDwellClickUI] Brak cursorUI.", this);
    }

    private void OnEnable()
    {
        currentButton = null;
        dwellTimer = 0f;
        cooldownTimer = 0f;
        cursorUI?.SetIdle();
    }

    private void OnDisable()
    {
        ExitCurrentButton();
        cursorUI?.SetIdle();

        if (Instance == this)
            Instance = null;
    }

    private void Update()
    {
        if (eventSystem == null)
            eventSystem = EventSystem.current;

        if (eventSystem == null || gazeDot == null)
            return;

        if (pointerData == null)
            pointerData = new PointerEventData(eventSystem);

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            ExitCurrentButton();
            return;
        }

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, gazeDot.position);

        pointerData.Reset();
        pointerData.position = screenPos;

        raycastResults.Clear();
        eventSystem.RaycastAll(pointerData, raycastResults);

        Button nextButton = FindFirstValidButton(raycastResults);

        if (nextButton != currentButton)
        {
            ExitCurrentButton();
            currentButton = nextButton;
            dwellTimer = 0f;

            if (currentButton != null && showDebugLogs)
                Debug.Log($"[GazeUI] Wejście: {currentButton.name}", currentButton);
        }

        // Nic UI nie śledzimy -> nie ruszamy kursora,
        // żeby 2D/world mogło nim sterować.
        if (currentButton == null)
            return;

        if (!currentButton.interactable || !currentButton.gameObject.activeInHierarchy)
        {
            ExitCurrentButton();
            return;
        }

        dwellTimer += Time.deltaTime;
        float progress = dwellSeconds <= 0.001f ? 1f : dwellTimer / dwellSeconds;

        cursorUI?.SetHoverProgress(progress);

        if (showDebugLogs && Time.frameCount % 60 == 0)
            Debug.Log($"[GazeUI] {currentButton.name} progress={progress:F2}", currentButton);

        if (dwellTimer >= dwellSeconds)
        {
            if (showDebugLogs)
                Debug.Log($"[GazeUI] KLIK: {currentButton.name}", currentButton);

            currentButton.onClick.Invoke();
            cursorUI?.SetIdle();

            ExitCurrentButton();
            cooldownTimer = cooldownAfterClick;
        }
    }

    private Button FindFirstValidButton(List<RaycastResult> results)
    {
        for (int i = 0; i < results.Count; i++)
        {
            GameObject go = results[i].gameObject;
            if (go == null) continue;

            Button btn = go.GetComponent<Button>() ?? go.GetComponentInParent<Button>();
            if (btn != null && btn.isActiveAndEnabled && btn.interactable)
                return btn;
        }

        return null;
    }

    private void ExitCurrentButton()
    {
        currentButton = null;
        dwellTimer = 0f;
    }
}