using System.Collections;
using UnityEngine;

public class DropKeyBehindCabinet : MonoBehaviour
{
    [Header("Pozycja za szafką")]
    [SerializeField] private Transform hiddenPoint;

    [Header("Animacja")]
    [SerializeField] private float moveDuration = 0.5f;

    [Header("Kolizja")]
    [SerializeField] private Collider2D keyCollider;

    private bool busy = false;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;

        if (keyCollider == null)
            keyCollider = GetComponent<Collider2D>();
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

            if (keyCollider != null)
                keyCollider.enabled = false;
        }
        else
        {
            transform.position = startPosition;

            if (keyCollider != null)
                keyCollider.enabled = true;
        }
    }
}

