using System.Collections;
using UnityEngine;

public class RevealPaperWithPencil : MonoBehaviour
{
    [Header("Obrazy")]
    [SerializeField] private GameObject blurredBase;
    [SerializeField] private GameObject revealStep1;
    [SerializeField] private GameObject revealStep2;
    [SerializeField] private GameObject revealStep3;

    [Header("Animacja")]
    [SerializeField] private float stepDuration = 0.35f;

    [Header("Opcjonalnie")]
    [SerializeField] private bool deselectPencilAfterUse = true;

    private bool revealed = false;
    private bool busy = false;

    public void TryReveal()
    {
        if (busy || revealed)
            return;

        if (!InventoryState.HasPencil())
            return;

        if (!InventoryState.IsSelected(InventoryState.Pencil))
            return;

        StartCoroutine(RevealRoutine());
    }

    private IEnumerator RevealRoutine()
    {
        busy = true;

        if (blurredBase != null) blurredBase.SetActive(true);
        if (revealStep1 != null) revealStep1.SetActive(false);
        if (revealStep2 != null) revealStep2.SetActive(false);
        if (revealStep3 != null) revealStep3.SetActive(false);

        yield return new WaitForSeconds(stepDuration);

        if (revealStep1 != null) revealStep1.SetActive(true);
        yield return new WaitForSeconds(stepDuration);

        if (revealStep2 != null) revealStep2.SetActive(true);
        yield return new WaitForSeconds(stepDuration);

        if (revealStep3 != null) revealStep3.SetActive(true);

        revealed = true;
        busy = false;

        if (deselectPencilAfterUse)
        {
            InventoryState.SetSelectedItem(InventoryState.None);

            InventorySlotItemUI[] all = FindObjectsByType<InventorySlotItemUI>(FindObjectsSortMode.None);
            foreach (var slot in all)
                slot.Refresh();
        }
    }
    private void OnMouseDown()
{
    TryReveal();
}
}