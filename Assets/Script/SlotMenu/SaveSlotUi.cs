using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotUI : MonoBehaviour
{
    [Header("SLOT INDEX")]
    [SerializeField] private int slotIndex = 0;

    [Header("PRZYCISKI")]
    [SerializeField] private Button slotButton;
    [SerializeField] private Button resetButton;

    [Header("TEKSTY")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text dateText;

    [Header("WIDOCZNOŚĆ")]
    [SerializeField] private GameObject emptyLabel;
    [SerializeField] private GameObject savedLabel;

    [Header("SCENA DO ZAŁADOWANIA")]
    [SerializeField] private string defaultScene = "presentroom1";

    private void Start()
    {
        slotButton?.onClick.AddListener(OnSlotClicked);
        resetButton?.onClick.AddListener(OnResetClicked);
        Refresh();
    }

    public void HandleSlotDwellClick()
    {
        OnSlotClicked();
    }

    public void HandleResetDwellClick()
    {
        OnResetClicked();
    }

    public void Refresh()
    {
        bool exists = SaveSystem.SlotExists(slotIndex);
        var data = exists ? SaveSystem.Load(slotIndex) : null;

        if (emptyLabel != null) emptyLabel.SetActive(!exists);
        if (savedLabel != null) savedLabel.SetActive(exists);
        if (resetButton != null) resetButton.gameObject.SetActive(exists);

        if (statusText != null)
            statusText.text = $"Slot {slotIndex + 1}";

        if (dateText != null)
        {
            dateText.gameObject.SetActive(exists);
            dateText.text = data != null ? data.saveDate : "";
        }
    }

    private void OnSlotClicked()
    {
        Debug.Log("[SaveSlotUI] KLIK SLOTU " + slotIndex);

        SaveSystem.ActiveSlot = slotIndex;
        InventoryState.LoadFromSlot(slotIndex);

        string sceneToLoad = defaultScene;
        if (SaveSystem.SlotExists(slotIndex))
        {
            var data = SaveSystem.Load(slotIndex);
            if (data != null && !string.IsNullOrEmpty(data.currentScene))
                sceneToLoad = data.currentScene;
        }

        Debug.Log($"[SaveSlotUI] Slot {slotIndex} — ładuję scenę: {sceneToLoad}");
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }

    private void OnResetClicked()
    {
        SaveSystem.DeleteSlot(slotIndex);
        InventoryState.ClearAll();
        Refresh();
        Debug.Log($"[SaveSlotUI] Slot {slotIndex} zresetowany.");
    }
}