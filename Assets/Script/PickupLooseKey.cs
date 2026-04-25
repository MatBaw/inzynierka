using UnityEngine;

public class PickupLooseKey : MonoBehaviour
{
    private void Start()
    {
        ApplySavedState();
    }

    private void OnMouseDown()
    {
        PickUp();
    }

    public void PickUp()
    {
        if (!InventoryState.IsKeyPulledOutByMouse())
        {
            Debug.Log("Klucz nie został jeszcze wyciągnięty.");
            return;
        }

        if (InventoryState.IsKeyCollected())
        {
            gameObject.SetActive(false);
            return;
        }

        InventoryState.SetKeyCollected(true);
        InventoryState.SetSelectedItem(InventoryState.None);

        RefreshInventoryUI();
        gameObject.SetActive(false);

        Debug.Log("[PickupLooseKey] Klucz został podniesiony.");
    }

    private void ApplySavedState()
    {
        if (InventoryState.IsKeyCollected())
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
    }

    private void RefreshInventoryUI()
    {
        InventorySlotItemUI[] all = FindObjectsOfType<InventorySlotItemUI>(true);
        foreach (InventorySlotItemUI slot in all)
            slot.Refresh();
    }
}