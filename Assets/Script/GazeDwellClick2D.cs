using UnityEngine;

public class GazeDwellClick2D : MonoBehaviour
{
    [Header("=== WYMAGANE REFERENCJE ===")]
    [SerializeField] RectTransform gazeDot;
    [SerializeField] Camera worldCam;
    [SerializeField] GazeCursorRingUI cursorUI;

    [Header("=== DWELL CZAS ===")]
    [SerializeField] float dwellSeconds = 2.5f;

    [Header("=== COOLDOWN ===")]
    [SerializeField] float cooldownAfterClick = 1.5f;
    [SerializeField] bool blockDwellAfterMouseClick = true;

    [Header("=== DEBUG ===")]
    [SerializeField] bool showDebugLogs = false;

    private GazeDwellTarget current;
    private float dwellTimer;
    private float cooldownTimer;

    public static GazeDwellClick2D Instance { get; private set; }

    private const string SceneInteractionPrefKey = "EyeInteractionEnabledInScene";

    void Awake()
    {
        Instance = this;
        if (worldCam == null) worldCam = Camera.main;
        if (cursorUI == null && gazeDot != null)
            cursorUI = gazeDot.GetComponent<GazeCursorRingUI>();

        if (cursorUI == null)
            Debug.LogError("[GazeDwellClick2D] BRAK cursorUI! Podepnij GazeCursorRingUI.", this);
    }

    void OnEnable()
    {
        current = null;
        dwellTimer = 0f;
        if (!UIIsTracking())
            cursorUI?.SetIdle();
    }

    bool UIIsTracking()
    {
        return GazeDwellClickUI.Instance != null && GazeDwellClickUI.Instance.IsTracking;
    }

    bool SceneInteractionEnabled()
    {
        return PlayerPrefs.GetInt(SceneInteractionPrefKey, 1) == 1;
    }

    void Update()
    {
        // jeśli interakcja sceny OFF -> nic na świecie nie klikamy
        if (!SceneInteractionEnabled())
        {
            if (current != null)
                ExitCurrentTarget();

            if (!UIIsTracking())
                cursorUI?.SetIdle();

            return;
        }

        // jeśli UI śledzi przycisk — oddaj mu kontrolę nad kursorem
        if (UIIsTracking()) return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (current != null) ExitCurrentTarget();
            cursorUI?.SetIdle();
            return;
        }

        if (blockDwellAfterMouseClick && Input.GetMouseButtonDown(0))
            TriggerCooldown();

        if (gazeDot == null || worldCam == null) return;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, gazeDot.position);
        float depth = -worldCam.transform.position.z;
        Vector3 worldPos = worldCam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, depth));

        RaycastHit2D hit = Physics2D.Raycast((Vector2)worldPos, Vector2.zero);

        GazeDwellTarget next = null;
        if (hit.collider != null)
        {
            next = hit.collider.GetComponent<GazeDwellTarget>()
                ?? hit.collider.GetComponentInParent<GazeDwellTarget>();
        }

        if (next != current)
        {
            ExitCurrentTarget();
            current = next;
            dwellTimer = 0f;
            if (current != null)
            {
                current.OnGazeEnter?.Invoke();
                if (showDebugLogs) Debug.Log($"[GazeDwell] Wejście: {current.name}");
            }
        }

        if (current == null)
        {
            cursorUI?.SetIdle();
            return;
        }

        dwellTimer += Time.deltaTime;
        float progress = dwellSeconds <= 0.001f ? 1f : (dwellTimer / dwellSeconds);
        cursorUI?.SetHoverProgress(progress);

        if (showDebugLogs && Time.frameCount % 60 == 0)
            Debug.Log($"[GazeDwell] {current.name} progress={progress:F2}");

        if (dwellTimer >= dwellSeconds)
        {
            if (showDebugLogs) Debug.Log($"[GazeDwell] KLIK: {current.name}");
            current.OnDwellClick?.Invoke();
            cursorUI?.SetIdle();
            ExitCurrentTarget();
            TriggerCooldown();
        }
    }

    void ExitCurrentTarget()
    {
        if (current != null)
        {
            current.OnGazeExit?.Invoke();
            current = null;
        }
        dwellTimer = 0f;
    }

    public void TriggerCooldown()
    {
        cooldownTimer = cooldownAfterClick;
        ExitCurrentTarget();
        if (!UIIsTracking())
            cursorUI?.SetIdle();
    }

    public void TriggerCooldown(float duration)
    {
        cooldownTimer = duration;
        ExitCurrentTarget();
        if (!UIIsTracking())
            cursorUI?.SetIdle();
    }
}


/*using UnityEngine;

public class GazeDwellClick2D : MonoBehaviour
{
    [Header("=== WYMAGANE REFERENCJE ===")]
    [SerializeField] RectTransform gazeDot;
    [SerializeField] Camera worldCam;
    [SerializeField] GazeCursorRingUI cursorUI;

    [Header("=== DWELL CZAS ===")]
    [SerializeField] float dwellSeconds = 2.5f;

    [Header("=== COOLDOWN ===")]
    [SerializeField] float cooldownAfterClick = 1.5f;
    [SerializeField] bool blockDwellAfterMouseClick = true;

    [Header("=== DEBUG ===")]
    [SerializeField] bool showDebugLogs = false;

    private GazeDwellTarget current;
    private float dwellTimer;
    private float cooldownTimer;

    public static GazeDwellClick2D Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        if (worldCam == null) worldCam = Camera.main;
        if (cursorUI == null && gazeDot != null)
            cursorUI = gazeDot.GetComponent<GazeCursorRingUI>();

        if (cursorUI == null)
            Debug.LogError("[GazeDwellClick2D] BRAK cursorUI! Podepnij GazeCursorRingUI.", this);
    }

    void OnEnable()
    {
        current = null;
        dwellTimer = 0f;
        // ✅ Resetuj idle tylko jeśli GazeDwellClickUI nie śledzi niczego
        if (!UIIsTracking())
            cursorUI?.SetIdle();
    }

    // ✅ Helper: czy GazeDwellClickUI aktualnie śledzi przycisk
    bool UIIsTracking()
    {
        return GazeDwellClickUI.Instance != null && GazeDwellClickUI.Instance.IsTracking;
    }

    void Update()
    {
        // ✅ Jeśli UI śledzi przycisk — oddaj mu kontrolę nad cursorem, nic nie rób
        if (UIIsTracking()) return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (current != null) ExitCurrentTarget();
            cursorUI?.SetIdle();
            return;
        }

        if (blockDwellAfterMouseClick && Input.GetMouseButtonDown(0))
            TriggerCooldown();

        if (gazeDot == null || worldCam == null) return;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, gazeDot.position);
        float depth = -worldCam.transform.position.z;
        Vector3 worldPos = worldCam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, depth));

        RaycastHit2D hit = Physics2D.Raycast((Vector2)worldPos, Vector2.zero);

        GazeDwellTarget next = null;
        if (hit.collider != null)
        {
            next = hit.collider.GetComponent<GazeDwellTarget>()
                ?? hit.collider.GetComponentInParent<GazeDwellTarget>();
        }

        if (next != current)
        {
            ExitCurrentTarget();
            current = next;
            dwellTimer = 0f;
            if (current != null)
            {
                current.OnGazeEnter?.Invoke();
                if (showDebugLogs) Debug.Log($"[GazeDwell] Wejście: {current.name}");
            }
        }

        if (current == null)
        {
            cursorUI?.SetIdle();
            return;
        }

        dwellTimer += Time.deltaTime;
        float progress = dwellSeconds <= 0.001f ? 1f : (dwellTimer / dwellSeconds);
        cursorUI?.SetHoverProgress(progress);

        if (showDebugLogs && Time.frameCount % 60 == 0)
            Debug.Log($"[GazeDwell] {current.name} progress={progress:F2}");

        if (dwellTimer >= dwellSeconds)
        {
            if (showDebugLogs) Debug.Log($"[GazeDwell] KLIK: {current.name}");
            current.OnDwellClick?.Invoke();
            cursorUI?.SetIdle();
            ExitCurrentTarget();
            TriggerCooldown();
        }
    }

    void ExitCurrentTarget()
    {
        if (current != null)
        {
            current.OnGazeExit?.Invoke();
            current = null;
        }
        dwellTimer = 0f;
    }

    public void TriggerCooldown()
    {
        cooldownTimer = cooldownAfterClick;
        ExitCurrentTarget();
        if (!UIIsTracking())
            cursorUI?.SetIdle();
    }

    public void TriggerCooldown(float duration)
    {
        cooldownTimer = duration;
        ExitCurrentTarget();
        if (!UIIsTracking())
            cursorUI?.SetIdle();
    }
}*/