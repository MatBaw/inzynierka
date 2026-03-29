using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotItemUI : MonoBehaviour, IPointerClickHandler
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
                iconImage.enabled = true;
            }
            else
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }
        }

        if (selectedHighlight != null)
            selectedHighlight.SetActive(hasItem && InventoryState.IsSelected(itemId));

        Debug.Log("[InventorySlotItemUI] slot=" + slotIndex + " item=" + itemId + " hasItem=" + hasItem);
    }

    public void OnPointerClick(PointerEventData eventData)
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

        return null;
    }

    private void RefreshAllSlots()
    {
        InventorySlotItemUI[] all = FindObjectsOfType<InventorySlotItemUI>(true);

        foreach (InventorySlotItemUI slot in all)
            slot.Refresh();
    }
}
/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventorySlotItemUI : MonoBehaviour
{
    [SerializeField] private string itemId = "PENCIL";
    [SerializeField] private GameObject itemIcon;
    [SerializeField] private GameObject selectionHighlight;
    [SerializeField] private Button button;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnClicked);
        if (itemIcon == null)
            Debug.LogError($"[InventorySlotItemUI] '{gameObject.name}': Item Icon nie podpięty!", this);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // ✅ Tylko jedna coroutine która czeka końca klatki — po wszystkich Awake/Start/OnEnable
        StartCoroutine(RefreshEndOfFrame());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != gameObject.scene.name) return;
        StartCoroutine(RefreshEndOfFrame());
    }

    // ✅ Czeka do końca klatki — wszystkie skrypty zdążą wykonać Awake/Start
    // i InventoryState.Init() zdąży załadować dane z pliku
    private IEnumerator RefreshEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        Refresh();
    }

private bool clickLocked = false;

public void OnClicked()
{
    if (clickLocked) return;
    StartCoroutine(ClickLockRoutine());

    if (!HasItem())
        return;

    if (InventoryState.IsSelected(itemId))
        InventoryState.SetSelectedItem(InventoryState.None);
    else
        InventoryState.SetSelectedItem(itemId);

    RefreshAll();
}

private System.Collections.IEnumerator ClickLockRoutine()
{
    clickLocked = true;
    yield return new WaitForSeconds(0.5f);
    clickLocked = false;
}

    public void Refresh()
    {
        if (itemIcon == null)
        {
            Debug.LogError($"[InventorySlotItemUI] '{gameObject.name}': itemIcon = NULL!", this);
            return;
        }

        bool hasItem = HasItem();
        bool selected = InventoryState.IsSelected(itemId);

        Debug.Log($"[InventorySlotItemUI] Refresh() | obiekt='{gameObject.name}' | scena='{gameObject.scene.name}' | instanceID={itemIcon.GetInstanceID()} | hasItem={hasItem} | SetActive({hasItem})");

        if (selectionHighlight != null)
            selectionHighlight.SetActive(false);

        itemIcon.SetActive(hasItem);

        if (hasItem)
        {
            itemIcon.transform.SetAsLastSibling();

            var img = itemIcon.GetComponent<Image>();
            if (img != null)
            {
                img.enabled = true;
                var c = img.color;
                c.a = 1f;
                img.color = c;
            }
        }

        if (selectionHighlight != null)
            selectionHighlight.SetActive(hasItem && selected);
    }

    private bool HasItem()
    {
        if (itemId == "PENCIL") return InventoryState.HasPencil();
        return false;
    }

    private void RefreshAll()
    {
        InventorySlotItemUI[] all = FindObjectsByType<InventorySlotItemUI>(FindObjectsSortMode.None);
        foreach (var slot in all)
            slot.Refresh();
    }
}*/





