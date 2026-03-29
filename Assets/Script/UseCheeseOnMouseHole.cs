using System.Collections;
using UnityEngine;

public class UseCheeseOnMouseHole : MonoBehaviour
{
    [Header("Obiekty sceny")]
    [SerializeField] private GameObject cheesePlacedVisual;
    [SerializeField] private GameObject mouseObject;

    [Header("Ruch myszy po wyjściu z dziury")]
    [SerializeField] private Vector3 mouseOffset = new Vector3(1.0f, 0f, 0f);
    [SerializeField] private float mouseMoveDuration = 0.5f;

    private Vector3 mouseStartPosition;
    private bool mouseStartCached = false;
    private bool isAnimating = false;

    private void Start()
    {
        if (mouseObject != null)
        {
            mouseStartPosition = mouseObject.transform.position;
            mouseStartCached = true;
        }

        ApplySavedState();
    }

    private void OnMouseDown()
    {
        TryUseCheese();
    }

    public void TryUseCheese()
    {
        if (InventoryState.IsMouseHoleSolved())
            return;

        if (isAnimating)
            return;

        if (!InventoryState.HasCheese())
        {
            Debug.Log("Brak sera");
            return;
        }

        if (!InventoryState.IsSelected(InventoryState.Cheese))
        {
            Debug.Log("Ser nie jest zaznaczony");
            return;
        }

        InventoryState.SetCheese(false);
        InventoryState.SetSelectedItem(InventoryState.None);
        InventoryState.SetMouseHoleSolved(true);

        if (cheesePlacedVisual != null)
            cheesePlacedVisual.SetActive(true);

        RefreshInventoryUI();

        if (mouseObject != null && mouseStartCached)
            StartCoroutine(AnimateMouseOut());
        else
            Debug.Log("Położono ser. Mysz wyszła z nory.");
    }

    private void ApplySavedState()
    {
        if (InventoryState.IsMouseHoleSolved())
            ShowSolvedStateInstant();
        else
            ShowUnsolvedState();
    }

    private void ShowSolvedStateInstant()
    {
        if (cheesePlacedVisual != null)
            cheesePlacedVisual.SetActive(true);

        if (mouseObject != null && mouseStartCached)
        {
            mouseObject.SetActive(true);
            mouseObject.transform.position = mouseStartPosition + mouseOffset;
        }
    }

    private void ShowUnsolvedState()
    {
        if (cheesePlacedVisual != null)
            cheesePlacedVisual.SetActive(false);

        if (mouseObject != null && mouseStartCached)
        {
            mouseObject.transform.position = mouseStartPosition;

            if (!InventoryState.HasMouse())
                mouseObject.SetActive(true);
        }
    }

    private IEnumerator AnimateMouseOut()
    {
        isAnimating = true;

        mouseObject.SetActive(true);

        Vector3 from = mouseStartPosition;
        Vector3 to = mouseStartPosition + mouseOffset;

        float t = 0f;

        while (t < mouseMoveDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / mouseMoveDuration);

            // płynniejszy ruch niż zwykły linear
            float eased = Mathf.SmoothStep(0f, 1f, normalized);

            mouseObject.transform.position = Vector3.Lerp(from, to, eased);
            yield return null;
        }

        mouseObject.transform.position = to;
        isAnimating = false;

        Debug.Log("Położono ser. Mysz wyszła z nory.");
    }

    private void RefreshInventoryUI()
    {
        InventorySlotItemUI[] all = FindObjectsOfType<InventorySlotItemUI>(true);
        foreach (InventorySlotItemUI slot in all)
            slot.Refresh();
    }
}