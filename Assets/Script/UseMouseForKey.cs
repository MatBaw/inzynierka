using System.Collections;
using UnityEngine;

public class UseMouseForKey : MonoBehaviour
{
    [Header("Obiekty")]
    [SerializeField] private GameObject mouseVisual;
    [SerializeField] private SpriteRenderer mouseRenderer;

    [SerializeField] private GameObject keyObject;
    [SerializeField] private SpriteRenderer keyRenderer;
    [SerializeField] private Collider2D keyCollider;

    [Header("Punkty")]
    [SerializeField] private Transform mouseAppearPoint;
    [SerializeField] private Transform mouseBehindPoint;
    [SerializeField] private Transform keyOutPoint;
    [SerializeField] private Transform mouseExitPoint;

    [Header("Animacja")]
    [SerializeField] private float moveDuration = 0.45f;
    [SerializeField] private float waitBehindCabinet = 0.2f;
    [SerializeField] private float waitAfterDrop = 0.15f;

    [Header("Klucz")]
    [SerializeField] private int keySortingOrderAfterPull = 2;

    private bool busy = false;

    private void Awake()
    {
        if (mouseVisual != null && mouseRenderer == null)
            mouseRenderer = mouseVisual.GetComponent<SpriteRenderer>();

        if (keyObject != null && keyRenderer == null)
            keyRenderer = keyObject.GetComponent<SpriteRenderer>();

        if (keyObject != null && keyCollider == null)
            keyCollider = keyObject.GetComponent<Collider2D>();
    }

    private void Start()
    {
        ApplySavedState();
    }

    private void OnMouseDown()
    {
        TryUseMouse();
    }

    public void TryUseMouse()
    {
        if (busy)
            return;

        if (InventoryState.IsKeyCollected())
            return;

        if (InventoryState.IsKeyPulledOutByMouse())
            return;

        if (!InventoryState.IsKeyDroppedBehindCabinet())
        {
            Debug.Log("Klucz jeszcze nie spadł za szafkę.");
            return;
        }

        if (!InventoryState.HasMouse())
        {
            Debug.Log("Brak myszy.");
            return;
        }

        if (!InventoryState.IsSelected(InventoryState.Mouse))
        {
            Debug.Log("Mysz nie jest zaznaczona.");
            return;
        }

        if (mouseVisual == null || mouseAppearPoint == null || mouseBehindPoint == null || keyOutPoint == null || mouseExitPoint == null)
        {
            Debug.LogWarning("[UseMouseForKey] Brakuje punktów lub mouseVisual.");
            return;
        }

        StartCoroutine(MouseRoutine());
    }

    private IEnumerator MouseRoutine()
    {
        busy = true;

        InventoryState.SetMouse(false);
        InventoryState.SetSelectedItem(InventoryState.None);
        RefreshInventoryUI();

        mouseVisual.SetActive(true);
        mouseVisual.transform.position = mouseAppearPoint.position;
        SetFacingRight(true);

        yield return StartCoroutine(MoveTo(mouseBehindPoint.position));

        yield return new WaitForSeconds(waitBehindCabinet);

        SetFacingRight(false);

        keyObject.SetActive(true);
        if (keyCollider != null)
            keyCollider.enabled = false;

        yield return StartCoroutine(MoveMouseAndKeyTo(keyOutPoint.position));

        keyObject.transform.position = keyOutPoint.position;

        if (keyRenderer != null)
            keyRenderer.sortingOrder = keySortingOrderAfterPull;

        if (keyCollider != null)
            keyCollider.enabled = true;

        InventoryState.SetKeyPulledOutByMouse(true);

        yield return new WaitForSeconds(waitAfterDrop);

        yield return StartCoroutine(MoveTo(mouseExitPoint.position));

        mouseVisual.SetActive(false);

        busy = false;

        Debug.Log("[UseMouseForKey] Mysz wyciągnęła klucz i uciekła.");
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = mouseVisual.transform.position;
        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / moveDuration);
            float eased = Mathf.SmoothStep(0f, 1f, k);

            mouseVisual.transform.position = Vector3.Lerp(start, target, eased);
            yield return null;
        }

        mouseVisual.transform.position = target;
    }

    private IEnumerator MoveMouseAndKeyTo(Vector3 target)
    {
        Vector3 mouseStart = mouseVisual.transform.position;
        Vector3 keyStart = keyObject.transform.position;
        float t = 0f;

        while (t < moveDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / moveDuration);
            float eased = Mathf.SmoothStep(0f, 1f, k);

            Vector3 pos = Vector3.Lerp(mouseStart, target, eased);
            mouseVisual.transform.position = pos;
            keyObject.transform.position = pos;

            yield return null;
        }

        mouseVisual.transform.position = target;
        keyObject.transform.position = target;
    }

    private void SetFacingRight(bool right)
    {
        if (mouseRenderer == null)
            return;

        mouseRenderer.flipX = !right;
    }

    private void ApplySavedState()
    {
        if (mouseVisual != null)
            mouseVisual.SetActive(false);

        if (InventoryState.IsKeyCollected())
        {
            if (keyObject != null)
                keyObject.SetActive(false);
            return;
        }

        if (InventoryState.IsKeyPulledOutByMouse())
        {
            if (keyObject != null && keyOutPoint != null)
            {
                keyObject.SetActive(true);
                keyObject.transform.position = keyOutPoint.position;
            }

            if (keyRenderer != null)
                keyRenderer.sortingOrder = keySortingOrderAfterPull;

            if (keyCollider != null)
                keyCollider.enabled = true;
        }
    }

    private void RefreshInventoryUI()
    {
        InventorySlotItemUI[] all = FindObjectsOfType<InventorySlotItemUI>(true);
        foreach (InventorySlotItemUI slot in all)
            slot.Refresh();
    }
}