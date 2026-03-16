using UnityEngine;
using System.Diagnostics;

public static class InventoryState
{
    private const string PencilKey      = "INV_PENCIL";
    private const string SelectedKey    = "INV_SELECTED_ITEM";

    public const string None   = "";
    public const string Pencil = "PENCIL";

    // ─── cache ────────────────────────────────────────────────────────────────
    // Reload Domain resetuje zmienne statyczne, dlatego używamy nullable bool.
    // null = "nie wczytano jeszcze w tej sesji domeny" → wczytaj z PlayerPrefs.
    // Po wczytaniu trzymamy wartość w pamięci → żaden kolejny odczyt z dysku.
    private static bool? _hasPencil = null;

    // ─── RuntimeInitializeOnLoadMethod ───────────────────────────────────────
    // Ta metoda jest wywoływana przez Unity zawsze po Domain Reload,
    // przed Awake/Start jakiegokolwiek obiektu. Działa w edytorze i w buildzie.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetCache()
    {
        // Wymuś ponowny odczyt z PlayerPrefs po każdym Domain Reload
        _hasPencil = null;
        UnityEngine.Debug.Log("[InventoryState] ResetCache — cache wyczyszczony (Domain Reload)");
    }

    // ─── API ──────────────────────────────────────────────────────────────────
    public static bool HasPencil()
    {
        // Jeśli cache pusty (po Domain Reload lub pierwszym uruchomieniu) — wczytaj z dysku
        if (_hasPencil == null)
        {
            _hasPencil = PlayerPrefs.GetInt(PencilKey, 0) == 1;
            UnityEngine.Debug.Log($"[InventoryState] HasPencil — wczytano z PlayerPrefs: {_hasPencil}");
        }
        return _hasPencil.Value;
    }

    public static void SetPencil(bool value)
    {
        _hasPencil = value;                              // cache
        PlayerPrefs.SetInt(PencilKey, value ? 1 : 0);  // dysk
        PlayerPrefs.Save();
        UnityEngine.Debug.Log("[InventoryState] SetPencil = " + value + "\nCaller:\n" + GetStackTrace());
    }

    public static string GetSelectedItem()
    {
        return PlayerPrefs.GetString(SelectedKey, None);
    }

    public static void SetSelectedItem(string itemId)
    {
        PlayerPrefs.SetString(SelectedKey, itemId);
        PlayerPrefs.Save();
        UnityEngine.Debug.Log("[InventoryState] SetSelectedItem = " + itemId);
    }

    public static bool IsSelected(string itemId)
    {
        return GetSelectedItem() == itemId;
    }

    public static void ClearAll()
    {
        _hasPencil = false;
        PlayerPrefs.DeleteKey(PencilKey);
        PlayerPrefs.DeleteKey(SelectedKey);
        PlayerPrefs.Save();
        UnityEngine.Debug.Log("[InventoryState] ClearAll()\nCaller:\n" + GetStackTrace());
    }

    private static string GetStackTrace()
    {
        return new StackTrace(1, true).ToString();
    }
}








/*using UnityEngine;
using System.Diagnostics;

public static class InventoryState
{
    private const string PencilKey = "INV_PENCIL";
    private const string SelectedItemKey = "INV_SELECTED_ITEM";

    public const string None = "";
    public const string Pencil = "PENCIL";

    // ✅ POPRAWKA: Trzymamy stan w statycznej zmiennej w pamięci
    // PlayerPrefs jest zawodny przy szybkich zmianach scen — zmienna statyczna
    // żyje przez cały czas działania aplikacji i jest zawsze aktualna.
    private static bool _hasPencil;
    private static bool _initialized = false;

    // Wywoływane automatycznie przy pierwszym użyciu
    private static void EnsureInitialized()
    {
        if (_initialized) return;
        _hasPencil = PlayerPrefs.GetInt(PencilKey, 0) == 1;
        _initialized = true;
        UnityEngine.Debug.Log($"[InventoryState] Inicjalizacja — hasPencil={_hasPencil} (z PlayerPrefs)");
    }

    public static bool HasPencil()
    {
        EnsureInitialized();
        return _hasPencil;
    }

    public static void SetPencil(bool value)
    {
        EnsureInitialized();
        _hasPencil = value;  // ✅ Najpierw zapisz w pamięci

        PlayerPrefs.SetInt(PencilKey, value ? 1 : 0);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("[InventoryState] SetPencil = " + value + "\nCaller:\n" + GetStackTrace());
    }

    public static string GetSelectedItem()
    {
        return PlayerPrefs.GetString(SelectedItemKey, None);
    }

    public static void SetSelectedItem(string itemId)
    {
        PlayerPrefs.SetString(SelectedItemKey, itemId);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("[InventoryState] Selected item = " + itemId);
    }

    public static bool IsSelected(string itemId)
    {
        return GetSelectedItem() == itemId;
    }

    public static void ClearAll()
    {
        _hasPencil = false;
        _initialized = true;

        PlayerPrefs.DeleteKey(PencilKey);
        PlayerPrefs.DeleteKey(SelectedItemKey);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("[InventoryState] ClearAll()\nCaller:\n" + GetStackTrace());
    }

    private static string GetStackTrace()
    {
        return new StackTrace(1, true).ToString();
    }
}*/

























/*using UnityEngine;
using System.Diagnostics;

public static class InventoryState
{
    private const string PencilKey = "INV_PENCIL";
    private const string SelectedItemKey = "INV_SELECTED_ITEM";

    public const string None = "";
    public const string Pencil = "PENCIL";

    public static bool HasPencil()
    {
        return PlayerPrefs.GetInt(PencilKey, 0) == 1;
    }

    public static void SetPencil(bool value)
    {
        PlayerPrefs.SetInt(PencilKey, value ? 1 : 0);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("[InventoryState] SetPencil = " + value + "\nCaller:\n" + GetStackTrace());
    }

    public static string GetSelectedItem()
    {
        return PlayerPrefs.GetString(SelectedItemKey, None);
    }

    public static void SetSelectedItem(string itemId)
    {
        PlayerPrefs.SetString(SelectedItemKey, itemId);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("[InventoryState] Selected item = " + itemId);
    }

    public static bool IsSelected(string itemId)
    {
        return GetSelectedItem() == itemId;
    }

    public static void ClearAll()
    {
        PlayerPrefs.DeleteKey(PencilKey);
        PlayerPrefs.DeleteKey(SelectedItemKey);
        PlayerPrefs.Save();

        UnityEngine.Debug.Log("[InventoryState] ClearAll()\nCaller:\n" + GetStackTrace());
    }

    private static string GetStackTrace()
    {
        return new StackTrace(1, true).ToString();
    }
}*/
