using UnityEngine;
using UnityEngine.UI;


public class GazeEdgeButtons : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] RectTransform gazeDot;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] CameraZoomController zoomController;

    [Header("Cursor UI (progress ring)")]
    [Tooltip("Podepnij GazeCursorRingUI z GazeDot")]
    [SerializeField] GazeCursorRingUI cursorUI;

    [Header("Edge zones")]
    [Range(0.03f, 0.20f)]
    [SerializeField] float edgeWidth01 = 0.08f;
    [SerializeField] float dwellSeconds = 0.6f;
    [SerializeField] float cooldownSeconds = 0.8f;

    private float dwellTimer;
    private float cooldownTimer;
    private int currentZone;

    private const string SceneInteractionPrefKey = "EyeInteractionEnabledInScene";

    void Awake()
    {
        if (zoomController == null)
            zoomController = FindFirstObjectByType<CameraZoomController>();

        if (cursorUI == null && gazeDot != null)
            cursorUI = gazeDot.GetComponent<GazeCursorRingUI>();
    }

    bool AnyOtherIsTracking()
    {
        if (GazeDwellClickUI.Instance != null && GazeDwellClickUI.Instance.IsTracking)
            return true;
        return false;
    }

    bool SceneInteractionEnabled()
    {
        return PlayerPrefs.GetInt(SceneInteractionPrefKey, 1) == 1;
    }

    void Update()
    {
        // jeśli interakcja sceny OFF -> edge buttons OFF
        if (!SceneInteractionEnabled())
        {
            ResetZone();
            return;
        }

        if (gazeDot == null || leftButton == null || rightButton == null) return;

        if (zoomController != null && zoomController.IsZoomed)
        {
            ResetZone();
            return;
        }

        if (AnyOtherIsTracking())
        {
            ResetZone();
            return;
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        Vector2 screen = RectTransformUtility.WorldToScreenPoint(null, gazeDot.position);
        float x01 = screen.x / Mathf.Max(Screen.width, 1);

        int zone = 0;
        if (x01 <= edgeWidth01) zone = -1;
        else if (x01 >= 1f - edgeWidth01) zone = 1;

        if (zone != currentZone)
        {
            currentZone = zone;
            dwellTimer = 0f;

            if (currentZone == 0)
                cursorUI?.SetIdle();
        }

        if (currentZone == 0) return;

        dwellTimer += Time.deltaTime;
        float progress = dwellSeconds <= 0.001f ? 1f : (dwellTimer / dwellSeconds);
        cursorUI?.SetHoverProgress(progress);

        if (dwellTimer >= dwellSeconds)
        {
            dwellTimer = 0f;
            cooldownTimer = cooldownSeconds;

            if (currentZone == -1) leftButton.onClick.Invoke();
            else rightButton.onClick.Invoke();

            cursorUI?.SetIdle();
            currentZone = 0;
        }
    }

    void ResetZone()
    {
        if (currentZone != 0)
        {
            currentZone = 0;
            dwellTimer = 0f;
            cursorUI?.SetIdle();
        }
    }
}

