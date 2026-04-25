using UnityEngine;

public class KeyInteract : MonoBehaviour
{
    [SerializeField] private DropKeyBehindCabinet dropKeyBehindCabinet;
    [SerializeField] private PickupLooseKey pickupLooseKey;

    public void Interact()
    {
        if (InventoryState.IsKeyCollected())
            return;

        if (InventoryState.IsKeyPulledOutByMouse())
        {
            if (pickupLooseKey != null)
                pickupLooseKey.PickUp();
        }
        else
        {
            if (dropKeyBehindCabinet != null)
                dropKeyBehindCabinet.TryDrop();
        }
    }

    private void OnMouseDown()
    {
        Interact();
    }
}