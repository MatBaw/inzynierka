using UnityEngine;

public class PickupPencil : MonoBehaviour
{
    public void PickUp()
    {
        Debug.Log("PODNIESIONO OLOWEK");
        InventoryState.SetPencil(true);

        InventoryBarUI ui = FindFirstObjectByType<InventoryBarUI>();
        if (ui != null)
            ui.Refresh();
        else
            Debug.LogWarning("Nie znaleziono InventoryBarUI w scenie.");

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
}