using UnityEngine;

public class GazeDwellClick2D : MonoBehaviour
{
    [SerializeField] RectTransform gazeDot;
    [SerializeField] Camera worldCam;
    [SerializeField] GazeCursorRingUI cursorUI;

    [Header("DWELL CZAS")]
    [SerializeField] float dwellSeconds = 2.5f;

    [Header("COOLDOWN")]
    [SerializeField] float cooldownAfterClick = 1.5f;
    [SerializeField] bool blockDwellAfterMouseClick = true;

    [Header("DEBUG")]
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

    float GetCurrentTargetDwellSeconds()
    {
        if (current == null)
            return dwellSeconds;

        GazeDwellOverride overrideComp = current.GetComponent<GazeDwellOverride>();
        if (overrideComp == null)
            overrideComp = current.GetComponentInParent<GazeDwellOverride>();

        if (overrideComp != null)
            return overrideComp.dwellSeconds;

        return dwellSeconds;
    }

    void Update()
    {
        if (!SceneInteractionEnabled())
        {
            if (current != null)
                ExitCurrentTarget();

            if (!UIIsTracking())
                cursorUI?.SetIdle();

            return;
        }

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

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, gazeDot.position); //pozycja obiektu DOT w UI i zmiana na współrzędne ekranu 
        float depth = -worldCam.transform.position.z; 
        Vector3 worldPos = worldCam.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, depth));

        RaycastHit2D hit = Physics2D.Raycast((Vector2)worldPos, Vector2.zero); //sprawdzenie czy gracz patrzy na hitbox

        GazeDwellTarget next = null;
        if (hit.collider != null) //gdy gracz patrzy na collider to szuka komponentu gaze dweel target i w jego rodzicu
        {
            next = hit.collider.GetComponent<GazeDwellTarget>()
                ?? hit.collider.GetComponentInParent<GazeDwellTarget>();
        }

        if (next != current) //jeśli zmienimy wzrok na następny obiekt to target się resetuje 
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

        float currentDwellSeconds = GetCurrentTargetDwellSeconds();

        dwellTimer += Time.deltaTime;
        float progress = currentDwellSeconds <= 0.001f ? 1f : (dwellTimer / currentDwellSeconds);
        cursorUI?.SetHoverProgress(progress);

        if (showDebugLogs && Time.frameCount % 60 == 0)
            Debug.Log($"[GazeDwell] {current.name} progress={progress:F2}");

        if (dwellTimer >= currentDwellSeconds) //gdy ustawiony czas minie zrób akcje
        {
            if (showDebugLogs) Debug.Log($"[GazeDwell] KLIK: {current.name}");
            current.OnDwellClick?.Invoke(); // uruchoomienie akcji
            cursorUI?.SetIdle(); //zsresetowanie kursora wzroku
            ExitCurrentTarget(); //reset
            TriggerCooldown(); //zabezpieczenie przed ponownym uruchomieniem
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