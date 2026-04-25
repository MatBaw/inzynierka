using UnityEngine;

public class ClockHintPulse : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Pulse")]
    [SerializeField] private float minAlpha = 0.15f;
    [SerializeField] private float maxAlpha = 0.55f;
    [SerializeField] private float pulseSpeed = 2f;

    [Header("Scale pulse")]
    [SerializeField] private bool pulseScale = true;
    [SerializeField] private float minScale = 0.95f;
    [SerializeField] private float maxScale = 1.05f;

    [Header("Show only when solved but before first past travel")]
    [SerializeField] private bool hideAfterFirstPastTravel = true;

    private Vector3 baseScale;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        RefreshVisibility();
    }

    private void Update()
    {
        RefreshVisibility();
        if (!ShouldShowHint()) return;

        float ping = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = Mathf.Lerp(minAlpha, maxAlpha, ping);
            spriteRenderer.color = c;
        }

        if (pulseScale)
        {
            float s = Mathf.Lerp(minScale, maxScale, ping);
            transform.localScale = baseScale * s;
        }
    }

    private void RefreshVisibility()
    {
        bool show = ShouldShowHint();

        if (spriteRenderer != null)
            spriteRenderer.enabled = show;
    }

    private bool ShouldShowHint()
    {
        if (!ClockState.IsSolved)
            return false;

        if (!hideAfterFirstPastTravel)
            return true;

        return !InventoryState.HasVisitedPastOnce();
    }
}