using UnityEngine;
using UnityEngine.UI;

public class GazeCursorRingUI : MonoBehaviour
{
    [Header("=== WYMAGANE REFERENCJE ===")]
    [SerializeField] private Image ringBg;
    [SerializeField] private Image ringFill;

    [Header("=== ROZMIARY ===")]
    [SerializeField] private float idleSize = 16f;
    [SerializeField] private float hoverSize = 48f;

    [Header("=== KOLORY ===")]
    [SerializeField] private Color idleColor      = new Color(1f, 1f, 1f, 0.85f);
    [SerializeField] private Color hoverBgColor   = new Color(0.9f, 0.1f, 0.1f, 0.75f);
    [SerializeField] private Color fillColor      = new Color(0.2f, 1f, 0.3f, 0.95f);

    [Header("=== ANIMACJA ===")]
    [SerializeField] private float sizeTransitionSpeed = 8f;
    [SerializeField] private bool animateFillClockwise = true;

    [Header("=== DEBUG ===")]
    [SerializeField] private bool debugLog = false;

    private RectTransform rt;
    private float targetSize;
    private float currentSize;

    public bool IsHovering { get; private set; }

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (rt == null) rt = gameObject.AddComponent<RectTransform>();
        ApplyIdleImmediate();
    }

    void Update()
    {
        currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * sizeTransitionSpeed);
        if (rt != null)
            rt.sizeDelta = new Vector2(currentSize, currentSize);
    }

    public void SetIdle()
    {
        if (debugLog)
            Debug.Log($"[CursorUI] SetIdle() wywołane z: {new System.Diagnostics.StackTrace().ToString().Split('\n')[1].Trim()}");

        IsHovering = false;
        targetSize = idleSize;

        if (ringBg != null) ringBg.color = idleColor;
        if (ringFill != null)
        {
            ringFill.enabled = false;
            ringFill.fillAmount = 0f;
        }
    }

    public void SetHoverProgress(float progress01)
    {
        if (debugLog)
            Debug.Log($"[CursorUI] SetHoverProgress({progress01:F2}) wywołane z: {new System.Diagnostics.StackTrace().ToString().Split('\n')[1].Trim()}");

        IsHovering = true;
        targetSize = hoverSize;

        if (ringBg != null) ringBg.color = hoverBgColor;
        if (ringFill != null)
        {
            ringFill.enabled = true;
            ringFill.color = fillColor;
            ringFill.fillAmount = Mathf.Clamp01(progress01);
        }
    }

    public void FlashClick()
    {
        SetIdle();
    }

    void ApplyIdleImmediate()
    {
        currentSize = idleSize;
        targetSize = idleSize;
        if (rt != null) rt.sizeDelta = new Vector2(idleSize, idleSize);
        if (ringBg != null) ringBg.color = idleColor;
        if (ringFill != null)
        {
            ringFill.enabled = false;
            ringFill.fillAmount = 0f;
        }
    }

#if UNITY_EDITOR
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T)) SetHoverProgress(0.25f);
        if (Input.GetKeyDown(KeyCode.Y)) SetHoverProgress(0.75f);
        if (Input.GetKeyDown(KeyCode.U)) SetHoverProgress(1f);
        if (Input.GetKeyDown(KeyCode.I)) SetIdle();
    }
#endif
}