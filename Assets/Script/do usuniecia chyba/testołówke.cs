using UnityEngine;

public class ResetPencilOnce : MonoBehaviour
{
    private void Start()
    {
        InventoryState.ClearAll();
    }
}