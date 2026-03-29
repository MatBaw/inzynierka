using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Kontroler pojedynczego slotu zapisu w menu głównym.
/// 
/// SETUP w Inspektorze:
/// - slotIndex: 0, 1 lub 2
/// - slotButton: przycisk całego slotu (klik = graj/nowa gra)
/// - resetButton: przycisk X po prawej stronie
/// - statusText: "Slot 1", "Slot 2" itp. lub data zapisu
/// - dateText: data i godzina zapisu (ukryty gdy pusty)
/// - emptyLabel: GameObject z napisem "Puste" (widoczny gdy brak zapisu)
/// - savedLabel: GameObject z ikoną/tekstem (widoczny gdy jest zapis)
/// </summary>
public class SaveSlotUI : MonoBehaviour
{
    [Header("=== SLOT INDEX ===")]
    [SerializeField] private int slotIndex = 0;

    [Header("=== PRZYCISKI ===")]
    [SerializeField] private Button slotButton;
    [SerializeField] private Button resetButton;

    [Header("=== TEKSTY ===")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text dateText;

    [Header("=== WIDOCZNOŚĆ ===")]
    [SerializeField] private GameObject emptyLabel;   // widoczny gdy slot pusty
    [SerializeField] private GameObject savedLabel;   // widoczny gdy slot zajęty

    [Header("=== SCENA DO ZAŁADOWANIA ===")]
    [SerializeField] private string defaultScene = "presentroom1";

    private void Start()
    {
        slotButton?.onClick.AddListener(OnSlotClicked);
        resetButton?.onClick.AddListener(OnResetClicked);
        Refresh();
    }

    public void Refresh()
    {
        bool exists = SaveSystem.SlotExists(slotIndex);
        var data    = exists ? SaveSystem.Load(slotIndex) : null;

        // Pokaż/ukryj etykiety
        if (emptyLabel != null) emptyLabel.SetActive(!exists);
        if (savedLabel != null) savedLabel.SetActive(exists);
        if (resetButton != null) resetButton.gameObject.SetActive(exists);

        // Teksty
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

        // Wczytaj stan ekwipunku z tego slotu
        InventoryState.LoadFromSlot(slotIndex);

        // Określ do której sceny wejść
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
