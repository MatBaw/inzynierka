using System.Collections;
using UnityEngine;

public class DropKeyBehindCabinet : MonoBehaviour
{
    [Header("Pozycja za szafką")]
    [SerializeField] private Transform hiddenPoint;

    [Header("Animacja")]
    [SerializeField] private float moveDuration = 0.5f;

    [Header("Render")]
    [SerializeField] private SpriteRenderer keyRenderer;
    [SerializeField] private int sortingOrderWhileHidden = -1;

    [Header("Kolizja")]
    [SerializeField] private Collider2D keyCollider;

    private bool busy = false;
    private Vector3 startPosition;
    private int startSortingOrder = 0;

    private void Awake()
    {
        startPosition = transform.position;

        if (keyRenderer == null)
            keyRenderer = GetComponent<SpriteRenderer>();

        if (keyCollider == null)
            keyCollider = GetComponent<Collider2D>();

        if (keyRenderer != null)
            startSortingOrder = keyRenderer.sortingOrder;
    }

    private void Start()
    {
        ApplySavedState();
    }

    private void OnMouseDown()
    {
        TryDrop();
    }

    public void TryDrop()
    {
        if (busy)
            return;

        if (InventoryState.IsKeyCollected())
            return;

        if (InventoryState.IsKeyDroppedBehindCabinet())
            return;

        if (hiddenPoint == null)
        {
            Debug.LogWarning("[DropKeyBehindCabinet] Brak hiddenPoint.");
            return;
        }

        StartCoroutine(DropRoutine());
    }

    private IEnumerator DropRoutine()
    {
        busy = true;

        Vector3 from = transform.position;
        Vector3 to = hiddenPoint.position;

        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / moveDuration);
            float eased = Mathf.SmoothStep(0f, 1f, normalized);

            transform.position = Vector3.Lerp(from, to, eased);
            yield return null;
        }

        transform.position = to;

        if (keyRenderer != null)
            keyRenderer.sortingOrder = sortingOrderWhileHidden;

        if (keyCollider != null)
            keyCollider.enabled = false;

        InventoryState.SetKeyDroppedBehindCabinet(true);
        busy = false;

        Debug.Log("[DropKeyBehindCabinet] Klucz spadł za szafkę.");
    }

    private void ApplySavedState()
    {
        if (InventoryState.IsKeyCollected())
        {
            gameObject.SetActive(false);
            return;
        }

        if (InventoryState.IsKeyPulledOutByMouse())
        {
            return;
        }

        if (InventoryState.IsKeyDroppedBehindCabinet())
        {
            if (hiddenPoint != null)
                transform.position = hiddenPoint.position;

            if (keyRenderer != null)
                keyRenderer.sortingOrder = sortingOrderWhileHidden;

            if (keyCollider != null)
                keyCollider.enabled = false;
        }
        else
        {
            transform.position = startPosition;

            if (keyRenderer != null)
                keyRenderer.sortingOrder = startSortingOrder;

            if (keyCollider != null)
                keyCollider.enabled = true;
        }
    }
}









/*using System.Collections;
using UnityEngine;

public class DropKeyBehindCabinet : MonoBehaviour
{
    [Header("Pozycja końcowa")]
    [SerializeField] private Transform targetPoint;

    [Header("Animacja")]
    [SerializeField] private float moveDuration = 0.6f;
    [SerializeField] private AnimationCurve moveCurve = null;

    [Header("Render")]
    [SerializeField] private SpriteRenderer keyRenderer;
    [SerializeField] private int orderInLayerAfterDrop = -1;

    [Header("Opcjonalnie")]
    [SerializeField] private Collider2D keyCollider;
    [SerializeField] private bool disableColliderAfterDrop = true;

    private bool dropped = false;
    private bool busy = false;
    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;

        if (keyRenderer == null)
            keyRenderer = GetComponent<SpriteRenderer>();

        if (keyCollider == null)
            keyCollider = GetComponent<Collider2D>();

        if (moveCurve == null || moveCurve.length == 0)
            moveCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }

    private void OnMouseDown()
    {
        TryDrop();
    }

    public void TryDrop()
    {
        if (busy || dropped)
            return;

        if (targetPoint == null)
        {
            Debug.LogWarning("[DropKeyBehindCabinet] Brak targetPoint.");
            return;
        }

        StartCoroutine(DropRoutine());
    }

    private IEnumerator DropRoutine()
    {
        busy = true;

        Vector3 from = transform.position;
        Vector3 to = targetPoint.position;

        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / moveDuration);
            float curved = moveCurve.Evaluate(normalized);

            transform.position = Vector3.Lerp(from, to, curved);
            yield return null;
        }

        transform.position = to;
        dropped = true;
        busy = false;

        if (keyRenderer != null)
            keyRenderer.sortingOrder = orderInLayerAfterDrop;

        if (disableColliderAfterDrop && keyCollider != null)
            keyCollider.enabled = false;

        Debug.Log("[DropKeyBehindCabinet] Klucz spadł za szafkę.");
    }
}*/