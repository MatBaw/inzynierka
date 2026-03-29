using UnityEngine;

public class PickupMouse : MonoBehaviour
{
    private void Start()
    {
        if (InventoryState.HasMouse())
        {
            gameObject.SetActive(false);
            return;
        }

        // jeśli zagadka nie została rozwiązana, mysz ma być schowana / niedostępna
        if (!InventoryState.IsMouseHoleSolved())
            gameObject.SetActive(false);
    }

    public void PickUp()
    {
        if (InventoryState.HasMouse())
        {
            gameObject.SetActive(false);
            return;
        }

        if (!InventoryState.IsMouseHoleSolved())
        {
            Debug.Log("Mysz jeszcze nie wyszła z nory.");
            return;
        }

        InventoryState.AddItemToPast(InventoryState.Mouse);
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