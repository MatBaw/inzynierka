using UnityEngine;

public class PickupPencil : MonoBehaviour
{
    private void Start()
    {
        // Jeśli ołówek już podniesiony (z poprzedniej sesji/sceny) — ukryj obiekt w świecie
        if (InventoryState.HasPencil())
        {
            Debug.Log("[PickupPencil] Ołówek już podniesiony — ukrywam obiekt w scenie.");
            gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        PickUp();
    }

    public void PickUp()
    {
        Debug.Log("[PickupPencil] PODNIESIONO OŁÓWEK");
        InventoryState.SetPencil(true);

        // Odśwież wszystkie sloty ekwipunku
        InventorySlotItemUI[] all = FindObjectsByType<InventorySlotItemUI>(FindObjectsSortMode.None);
        Debug.Log($"[PickupPencil] Znaleziono {all.Length} slotów do odświeżenia.");
        foreach (var slot in all)
            slot.Refresh();

        gameObject.SetActive(false);
    }

    // ✅ Możesz wywołać tę metodę też z GazeDwellTarget (OnDwellClick)
    public void PickUpGaze()
    {
        PickUp();
    }
}

/*using UnityEngine;

public class PickupPencil : MonoBehaviour
{
    public void PickUp()
    {
        Debug.Log("[PickupPencil] PODNIESIONO OLOWEK");
        InventoryState.SetPencil(true);

        InventorySlotItemUI[] all = FindObjectsByType<InventorySlotItemUI>(FindObjectsSortMode.None);
        foreach (var slot in all)
            slot.Refresh();

        gameObject.SetActive(false);
    }

    private void Start()
    {
        if (InventoryState.HasPencil())
            gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        PickUp();
    }
}*/