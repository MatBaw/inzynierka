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

        // ✅ Wczesne ostrzeżenie — wykryj brak podpięcia zanim cokolwiek pójdzie nie tak
        if (itemIcon == null)
            Debug.LogError($"[InventorySlotItemUI] '{gameObject.name}' w scenie '{gameObject.scene.name}': pole 'Item Icon' nie jest podpięte w Inspektorze!", this);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ✅ Odświeżaj tylko gdy załadowana scena to ta sama co nasz obiekt
        if (scene.name != gameObject.scene.name) return;

        Invoke(nameof(Refresh), 0.05f);
    }

    public void OnClicked()
    {
        if (!HasItem()) return;

        if (InventoryState.IsSelected(itemId))
            InventoryState.SetSelectedItem(InventoryState.None);
        else
            InventoryState.SetSelectedItem(itemId);

        RefreshAll();
    }

    public void Refresh()
    {
        // ✅ Nie rób nic jeśli itemIcon nie podpięty — unikamy NullReferenceException
        if (itemIcon == null)
        {
            Debug.LogError($"[InventorySlotItemUI] '{gameObject.name}': itemIcon = NULL — podepnij w Inspektorze!", this);
            return;
        }

        bool hasItem = HasItem();
        bool selected = InventoryState.IsSelected(itemId);

        Debug.Log($"[InventorySlotItemUI] {gameObject.name} | scene={gameObject.scene.name} | hasItem={hasItem} | selected={selected}");

        itemIcon.SetActive(hasItem);

        if (hasItem)
            itemIcon.transform.SetAsLastSibling();

        if (selectionHighlight != null)
            selectionHighlight.SetActive(hasItem && selected);
    }

    private bool HasItem()
    {
        if (itemId == "PENCIL")
            return InventoryState.HasPencil();

        return false;
    }

    private void RefreshAll()
    {
        InventorySlotItemUI[] all = FindObjectsByType<InventorySlotItemUI>(FindObjectsSortMode.None);
        foreach (var slot in all)
            slot.Refresh();
    }
}



/*using UnityEngine;
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
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ✅ POPRAWKA: Odświeżaj tylko gdy załadowana scena to ta sama co nasz obiekt
        // Zapobiega sytuacji gdzie MenuScene triggeruje Refresh() na slocie z presentroom
        if (scene.name != gameObject.scene.name)
        {
            Debug.Log($"[InventorySlotItemUI] OnSceneLoaded — ignoruję scenę '{scene.name}' (jesteśmy w '{gameObject.scene.name}')");
            return;
        }

        // Małe opóźnienie — pozwól Unity dokończyć ładowanie sceny
        Invoke(nameof(Refresh), 0.05f);
    }

    public void OnClicked()
    {
        if (!HasItem())
            return;

        if (InventoryState.IsSelected(itemId))
            InventoryState.SetSelectedItem(InventoryState.None);
        else
            InventoryState.SetSelectedItem(itemId);

        RefreshAll();
    }

    public void Refresh()
    {
        bool hasItem = HasItem();
        bool selected = InventoryState.IsSelected(itemId);

        Debug.Log($"[InventorySlotItemUI] {gameObject.name} | scene={gameObject.scene.name} | hasItem={hasItem} | selected={selected} | itemIcon={itemIcon?.name ?? "NULL"} | itemIcon.activeSelf przed={itemIcon?.activeSelf}");

        if (itemIcon != null)
        {
            itemIcon.SetActive(hasItem);

            if (hasItem)
                itemIcon.transform.SetAsLastSibling();

            Debug.Log($"[InventorySlotItemUI] itemIcon.SetActive({hasItem}) → activeSelf po={itemIcon.activeSelf}");
        }
        else
        {
            Debug.LogError($"[InventorySlotItemUI] {gameObject.name}: itemIcon NIE JEST PODPIĘTY w Inspektorze!");
        }

        if (selectionHighlight != null)
            selectionHighlight.SetActive(hasItem && selected);
    }

    private bool HasItem()
    {
        if (itemId == "PENCIL")
            return InventoryState.HasPencil();

        return false;
    }

    private void RefreshAll()
    {
        InventorySlotItemUI[] all = FindObjectsByType<InventorySlotItemUI>(FindObjectsSortMode.None);
        foreach (var slot in all)
            slot.Refresh();
    }
}*/




























/*using UnityEngine;
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
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // ✅ POPRAWKA: Odświeżaj też przy OnEnable (nie tylko Start)
        // bo przy powrocie do sceny Canvas może być reaktywowany
        Refresh();
    }

    private void Start()
    {
        Refresh();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Małe opóźnienie — pozwól Unity dokończyć ładowanie sceny zanim odświeżymy
        Invoke(nameof(Refresh), 0.05f);
    }

    public void OnClicked()
    {
        if (!HasItem())
            return;

        if (InventoryState.IsSelected(itemId))
            InventoryState.SetSelectedItem(InventoryState.None);
        else
            InventoryState.SetSelectedItem(itemId);

        RefreshAll();
    }

    public void Refresh()
    {
        bool hasItem = HasItem();
        bool selected = InventoryState.IsSelected(itemId);

        Debug.Log($"[InventorySlotItemUI] {gameObject.name} | scene={SceneManager.GetActiveScene().name} | hasItem={hasItem} | selected={selected} | itemIcon={itemIcon?.name ?? "NULL"} | itemIcon.activeSelf przed={itemIcon?.activeSelf}");

        if (itemIcon != null)
        {
            // ✅ POPRAWKA: Upewnij się że rodzic ikony też jest aktywny
            if (!itemIcon.transform.parent.gameObject.activeInHierarchy)
            {
                Debug.LogWarning($"[InventorySlotItemUI] Rodzic itemIcon jest wyłączony! ({itemIcon.transform.parent.name})");
            }

            itemIcon.SetActive(hasItem);

            // ✅ POPRAWKA: SetAsLastSibling żeby ikona była nad innymi elementami slotu
            if (hasItem)
                itemIcon.transform.SetAsLastSibling();

            Debug.Log($"[InventorySlotItemUI] itemIcon.SetActive({hasItem}) → activeSelf po={itemIcon.activeSelf}");
        }
        else
        {
            Debug.LogError($"[InventorySlotItemUI] {gameObject.name}: itemIcon NIE JEST PODPIĘTY w Inspectorze!");
        }

        if (selectionHighlight != null)
            selectionHighlight.SetActive(hasItem && selected);
    }

    private bool HasItem()
    {
        if (itemId == "PENCIL")
            return InventoryState.HasPencil();

        return false;
    }

    private void RefreshAll()
    {
        InventorySlotItemUI[] all = FindObjectsByType<InventorySlotItemUI>(FindObjectsSortMode.None);
        foreach (var slot in all)
            slot.Refresh();
    }
}*/




























/*using UnityEngine;
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
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Refresh();
    }

    public void OnClicked()
    {
        if (!HasItem())
            return;

        if (InventoryState.IsSelected(itemId))
            InventoryState.SetSelectedItem(InventoryState.None);
        else
            InventoryState.SetSelectedItem(itemId);

        RefreshAll();
    }

    public void Refresh()
    {
        bool hasItem = HasItem();
        bool selected = InventoryState.IsSelected(itemId);

        Debug.Log($"[InventorySlotItemUI] {gameObject.name} Refresh | hasItem={hasItem} | selected={selected}");

        if (itemIcon != null)
        {
            itemIcon.SetActive(hasItem);
            itemIcon.transform.SetAsLastSibling();
        }

        if (selectionHighlight != null)
            selectionHighlight.SetActive(hasItem && selected);
    }

    private bool HasItem()
    {
        if (itemId == "PENCIL")
            return InventoryState.HasPencil();

        return false;
    }

    private void RefreshAll()
    {
        InventorySlotItemUI[] all = FindObjectsByType<InventorySlotItemUI>(FindObjectsSortMode.None);
        foreach (var slot in all)
            slot.Refresh();
    }
}*/