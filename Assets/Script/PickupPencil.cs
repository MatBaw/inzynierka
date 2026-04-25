using UnityEngine;

public class PickupPencil : MonoBehaviour
{
    private void Start()
    {
        if (InventoryState.HasPencil())
            gameObject.SetActive(false);
    }

    public void PickUp()
    {
        if (InventoryState.HasPencil())
        {
            gameObject.SetActive(false);
            return;
        }

        InventoryState.AddItemToPresent(InventoryState.Pencil);
        RefreshInventoryUI();
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        PickUp();
    }

    private void RefreshInventoryUI()
    {
        InventorySlotItemUI[] all = FindObjectsOfType<InventorySlotItemUI>(true);
        foreach (InventorySlotItemUI slot in all)
            slot.Refresh();
    }
}