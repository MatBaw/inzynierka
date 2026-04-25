using UnityEngine;
using UnityEngine.UI;

public class InventorySlotItemUI : MonoBehaviour
{
    [Header("Index slotu")]
    [SerializeField] private int slotIndex = 0;

    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject selectedHighlight;

    [Header("Ikony")]
    [SerializeField] private Sprite pencilSprite;
    [SerializeField] private Sprite cheeseSprite;
    [SerializeField] private Sprite mouseSprite;
    [SerializeField] private Sprite keySprite;

    private void OnEnable()
    {
        Refresh();
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        InventoryState.ClearSelectionIfItemNotInCurrentTime();

        string itemId = InventoryState.GetItemAtSlot(slotIndex);
        bool hasItem = !string.IsNullOrEmpty(itemId);

        if (iconImage != null)
        {
            if (hasItem)
            {
                iconImage.sprite = GetSpriteForItem(itemId);
                iconImage.enabled = iconImage.sprite != null;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }
        }

        if (selectedHighlight != null)
            selectedHighlight.SetActive(hasItem && InventoryState.IsSelected(itemId));
    }

    public void OnSlotClicked()
    {
        string itemId = InventoryState.GetItemAtSlot(slotIndex);

        if (string.IsNullOrEmpty(itemId))
            return;

        if (InventoryState.IsSelected(itemId))
            InventoryState.SetSelectedItem(InventoryState.None);
        else
            InventoryState.SetSelectedItem(itemId);

        RefreshAllSlots();
    }

    private Sprite GetSpriteForItem(string itemId)
    {
        if (itemId == InventoryState.Pencil)
            return pencilSprite;

        if (itemId == InventoryState.Cheese)
            return cheeseSprite;

        if (itemId == InventoryState.Mouse)
            return mouseSprite;

        if (itemId == InventoryState.Key)
            return keySprite;

        return null;
    }

    private void RefreshAllSlots()
    {
        InventorySlotItemUI[] all = FindObjectsOfType<InventorySlotItemUI>(true);

        foreach (InventorySlotItemUI slot in all)
            slot.Refresh();
    }
}


