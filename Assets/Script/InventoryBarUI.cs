using UnityEngine;

public class InventoryBarUI : MonoBehaviour
{
    [SerializeField] private GameObject pencilIcon;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        bool hasPencil = InventoryState.HasPencil();
        Debug.Log("Refresh UI, HasPencil = " + hasPencil);

        if (pencilIcon != null)
            pencilIcon.SetActive(hasPencil);
        else
            Debug.LogWarning("PencilIcon nie jest podpiety w InventoryBarUI.");
    }
}