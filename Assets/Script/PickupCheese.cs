using UnityEngine;

public class PickupCheese : MonoBehaviour
{
    private void Start()
    {
        bool shouldHide = InventoryState.HasCheese() || InventoryState.IsMouseHoleSolved();
        gameObject.SetActive(!shouldHide);
    }

    private void OnMouseDown()
    {
        PickUp();
    }

    public void PickUp()
    {
        if (InventoryState.HasCheese() || InventoryState.IsMouseHoleSolved())
        {
            gameObject.SetActive(false);
            return;
        }

        Debug.Log("[PickupCheese] PickUp");

        InventoryState.AddItemToPast(InventoryState.Cheese);
        RefreshInventoryUI();
        gameObject.SetActive(false);
    }

    private void RefreshInventoryUI()
    {
        InventorySlotItemUI[] all = FindObjectsOfType<InventorySlotItemUI>(true);
        foreach (InventorySlotItemUI slot in all)
            slot.Refresh();
    }
}