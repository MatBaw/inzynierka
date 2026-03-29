using UnityEngine;

public class PickupLooseKey : MonoBehaviour
{
    private void Start()
    {
        if (InventoryState.IsKeyCollected())
            gameObject.SetActive(false);
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
        gameObject.SetActive(false);

        Debug.Log("[PickupLooseKey] Klucz został podniesiony.");
    }
}