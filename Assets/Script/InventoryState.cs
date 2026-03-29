using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InventoryState
{
    public const string None = "";
    public const string Pencil = "PENCIL";
    public const string Cheese = "CHEESE";
    public const string Mouse = "MOUSE";

    private static List<string> presentItems = new List<string>();
    private static List<string> pastItems = new List<string>();

    private static string selectedItem = None;

    private static bool paperRevealed = false;
    private static bool mouseHoleSolved = false;
    private static bool pastCabinetOpened = false;

    private static bool keyDroppedBehindCabinet = false;
    private static bool keyPulledOutByMouse = false;
    private static bool keyCollected = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        presentItems = new List<string>();
        pastItems = new List<string>();
        selectedItem = None;

        paperRevealed = false;
        mouseHoleSolved = false;
        pastCabinetOpened = false;

        keyDroppedBehindCabinet = false;
        keyPulledOutByMouse = false;
        keyCollected = false;

        Debug.Log("[InventoryState] Init");
    }

    public static bool IsPresentScene()
    {
        string scene = SceneManager.GetActiveScene().name.ToLower();
        return scene.Contains("present");
    }

    public static bool IsPastScene()
    {
        string scene = SceneManager.GetActiveScene().name.ToLower();
        return scene.Contains("past");
    }

    public static List<string> GetCurrentInventory()
    {
        return IsPresentScene() ? presentItems : pastItems;
    }

    public static string GetItemAtSlot(int slotIndex)
    {
        List<string> current = GetCurrentInventory();

        if (slotIndex < 0 || slotIndex >= current.Count)
            return None;

        return current[slotIndex];
    }

    public static int GetCurrentInventoryCount()
    {
        return GetCurrentInventory().Count;
    }

    public static bool HasItem(string itemId)
    {
        return presentItems.Contains(itemId) || pastItems.Contains(itemId);
    }

    public static bool HasItemInCurrentTime(string itemId)
    {
        return GetCurrentInventory().Contains(itemId);
    }

    public static void AddItemToPresent(string itemId)
    {
        if (!presentItems.Contains(itemId))
        {
            presentItems.Add(itemId);
            Debug.Log("[InventoryState] Added to PRESENT: " + itemId);
        }
    }

    public static void AddItemToPast(string itemId)
    {
        if (!pastItems.Contains(itemId))
        {
            pastItems.Add(itemId);
            Debug.Log("[InventoryState] Added to PAST: " + itemId);
        }
    }

    public static void RemoveItem(string itemId)
    {
        presentItems.Remove(itemId);
        pastItems.Remove(itemId);

        if (selectedItem == itemId)
            selectedItem = None;

        Debug.Log("[InventoryState] Removed item: " + itemId);
    }

    public static string GetSelectedItem()
    {
        return selectedItem;
    }

    public static void SetSelectedItem(string itemId)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            selectedItem = None;
            Debug.Log("[InventoryState] SetSelectedItem = NONE");
            return;
        }

        if (!HasItem(itemId))
        {
            Debug.Log("[InventoryState] Nie można zaznaczyć itemu, bo go nie ma: " + itemId);
            return;
        }

        selectedItem = itemId;
        Debug.Log("[InventoryState] SetSelectedItem = " + itemId);
    }

    public static bool IsSelected(string itemId)
    {
        return selectedItem == itemId;
    }

    public static void ClearSelectionIfItemNotInCurrentTime()
    {
        if (!string.IsNullOrEmpty(selectedItem) && !HasItemInCurrentTime(selectedItem))
        {
            Debug.Log("[InventoryState] ClearSelectionIfItemNotInCurrentTime -> " + selectedItem);
            selectedItem = None;
        }
    }

    public static bool IsPaperRevealed()
    {
        return paperRevealed;
    }

    public static void SetPaperRevealed(bool value)
    {
        paperRevealed = value;
        Debug.Log("[InventoryState] SetPaperRevealed = " + value);
    }

    public static bool IsMouseHoleSolved()
    {
        return mouseHoleSolved;
    }

    public static void SetMouseHoleSolved(bool value)
    {
        mouseHoleSolved = value;
        Debug.Log("[InventoryState] SetMouseHoleSolved = " + value);
    }

    public static bool IsPastCabinetOpened()
    {
        return pastCabinetOpened;
    }

    public static void SetPastCabinetOpened(bool value)
    {
        pastCabinetOpened = value;
        Debug.Log("[InventoryState] SetPastCabinetOpened = " + value);
    }

    public static bool IsKeyDroppedBehindCabinet()
    {
        return keyDroppedBehindCabinet;
    }

    public static void SetKeyDroppedBehindCabinet(bool value)
    {
        keyDroppedBehindCabinet = value;
        Debug.Log("[InventoryState] SetKeyDroppedBehindCabinet = " + value);
    }

    public static bool IsKeyPulledOutByMouse()
    {
        return keyPulledOutByMouse;
    }

    public static void SetKeyPulledOutByMouse(bool value)
    {
        keyPulledOutByMouse = value;
        Debug.Log("[InventoryState] SetKeyPulledOutByMouse = " + value);
    }

    public static bool IsKeyCollected()
    {
        return keyCollected;
    }

    public static void SetKeyCollected(bool value)
    {
        keyCollected = value;
        Debug.Log("[InventoryState] SetKeyCollected = " + value);
    }

    // =========================
    // KOMPATYBILNOŚĆ ZE STARYM KODEM
    // =========================

    public static bool HasPencil()
    {
        return presentItems.Contains(Pencil);
    }

    public static void SetPencil(bool value)
    {
        if (value)
        {
            if (!presentItems.Contains(Pencil))
                presentItems.Add(Pencil);
        }
        else
        {
            presentItems.Remove(Pencil);
            if (selectedItem == Pencil)
                selectedItem = None;
        }

        Debug.Log("[InventoryState] SetPencil = " + value);
    }

    public static bool HasCheese()
    {
        return pastItems.Contains(Cheese);
    }

    public static void SetCheese(bool value)
    {
        if (value)
        {
            if (!pastItems.Contains(Cheese))
                pastItems.Add(Cheese);
        }
        else
        {
            pastItems.Remove(Cheese);
            if (selectedItem == Cheese)
                selectedItem = None;
        }

        Debug.Log("[InventoryState] SetCheese = " + value);
    }

    public static bool HasMouse()
    {
        return pastItems.Contains(Mouse);
    }

    public static void SetMouse(bool value)
    {
        if (value)
        {
            if (!pastItems.Contains(Mouse))
                pastItems.Add(Mouse);
        }
        else
        {
            pastItems.Remove(Mouse);
            if (selectedItem == Mouse)
                selectedItem = None;
        }

        Debug.Log("[InventoryState] SetMouse = " + value);
    }

    public static void LoadFromSlot(int slot)
    {
        ClearAll();

        SaveSystem.SaveData data = SaveSystem.Load(slot);
        if (data == null)
        {
            Debug.Log("[InventoryState] LoadFromSlot -> brak danych w slocie " + slot);
            return;
        }

        SetPencil(data.hasPencil);
        SetCheese(data.hasCheese);
        SetMouse(data.hasMouse);

        SetPaperRevealed(data.paperRevealed);
        SetMouseHoleSolved(data.mouseHoleSolved);
        SetPastCabinetOpened(data.pastCabinetOpened);

        SetKeyDroppedBehindCabinet(data.keyDroppedBehindCabinet);
        SetKeyPulledOutByMouse(data.keyPulledOutByMouse);
        SetKeyCollected(data.keyCollected);

        Debug.Log(
            "[InventoryState] LoadFromSlot(" + slot + ")" +
            " | hasPencil=" + data.hasPencil +
            " | hasCheese=" + data.hasCheese +
            " | hasMouse=" + data.hasMouse +
            " | paperRevealed=" + data.paperRevealed +
            " | mouseHoleSolved=" + data.mouseHoleSolved +
            " | pastCabinetOpened=" + data.pastCabinetOpened +
            " | keyDroppedBehindCabinet=" + data.keyDroppedBehindCabinet +
            " | keyPulledOutByMouse=" + data.keyPulledOutByMouse +
            " | keyCollected=" + data.keyCollected
        );
    }

    public static void ClearAll()
    {
        presentItems.Clear();
        pastItems.Clear();
        selectedItem = None;

        paperRevealed = false;
        mouseHoleSolved = false;
        pastCabinetOpened = false;

        keyDroppedBehindCabinet = false;
        keyPulledOutByMouse = false;
        keyCollected = false;

        Debug.Log("[InventoryState] ClearAll()");
    }
}


























/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InventoryState
{
    public const string None = "";
    public const string Pencil = "PENCIL";
    public const string Cheese = "CHEESE";
    public const string Mouse = "MOUSE";

    private static List<string> presentItems = new List<string>();
    private static List<string> pastItems = new List<string>();

    private static string selectedItem = None;

    private static bool paperRevealed = false;

    private static bool mouseHoleSolved = false;

    private static bool pastCabinetOpened = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        presentItems = new List<string>();
        pastItems = new List<string>();
        selectedItem = None;
        paperRevealed = false;
        mouseHoleSolved = false;
        pastCabinetOpened = false;
        Debug.Log("[InventoryState] Init");
    }

    public static bool IsPresentScene()
    {
        string scene = SceneManager.GetActiveScene().name.ToLower();
        return scene.Contains("present");
    }

    public static bool IsPastScene()
    {
        string scene = SceneManager.GetActiveScene().name.ToLower();
        return scene.Contains("past");
    }

    public static bool IsPastCabinetOpened()
    {
        return pastCabinetOpened;
    }

public static void SetPastCabinetOpened(bool value)
{
    pastCabinetOpened = value;
    Debug.Log("[InventoryState] SetPastCabinetOpened = " + value);
}

    public static List<string> GetCurrentInventory()
    {
        return IsPresentScene() ? presentItems : pastItems;
    }

    public static string GetItemAtSlot(int slotIndex)
    {
        List<string> current = GetCurrentInventory();

        if (slotIndex < 0 || slotIndex >= current.Count)
            return None;

        return current[slotIndex];
    }

    public static int GetCurrentInventoryCount()
    {
        return GetCurrentInventory().Count;
    }

    public static bool HasItem(string itemId)
    {
        return presentItems.Contains(itemId) || pastItems.Contains(itemId);
    }

    public static bool HasItemInCurrentTime(string itemId)
    {
        return GetCurrentInventory().Contains(itemId);
    }

    public static void AddItemToPresent(string itemId)
    {
        if (!presentItems.Contains(itemId))
        {
            presentItems.Add(itemId);
            Debug.Log("[InventoryState] Added to PRESENT: " + itemId);
        }
    }

    public static void AddItemToPast(string itemId)
    {
        if (!pastItems.Contains(itemId))
        {
            pastItems.Add(itemId);
            Debug.Log("[InventoryState] Added to PAST: " + itemId);
        }
    }

    public static void RemoveItem(string itemId)
    {
        presentItems.Remove(itemId);
        pastItems.Remove(itemId);

        if (selectedItem == itemId)
            selectedItem = None;

        Debug.Log("[InventoryState] Removed item: " + itemId);
    }

    public static string GetSelectedItem()
    {
        return selectedItem;
    }

    public static void SetSelectedItem(string itemId)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            selectedItem = None;
            Debug.Log("[InventoryState] SetSelectedItem = NONE");
            return;
        }

        if (!HasItem(itemId))
        {
            Debug.Log("[InventoryState] Nie można zaznaczyć itemu, bo go nie ma: " + itemId);
            return;
        }

        selectedItem = itemId;
        Debug.Log("[InventoryState] SetSelectedItem = " + itemId);
    }

    public static bool IsSelected(string itemId)
    {
        return selectedItem == itemId;
    }

    public static void ClearSelectionIfItemNotInCurrentTime()
    {
        if (!string.IsNullOrEmpty(selectedItem) && !HasItemInCurrentTime(selectedItem))
        {
            Debug.Log("[InventoryState] ClearSelectionIfItemNotInCurrentTime -> " + selectedItem);
            selectedItem = None;
        }
    }

    public static bool IsPaperRevealed()
    {
        return paperRevealed;
    }

    public static void SetPaperRevealed(bool value)
    {
        paperRevealed = value;
        Debug.Log("[InventoryState] SetPaperRevealed = " + value);
    }

    public static bool HasPencil()
    {
        return presentItems.Contains(Pencil);
    }

    public static void SetPencil(bool value)
    {
        if (value)
        {
            if (!presentItems.Contains(Pencil))
                presentItems.Add(Pencil);
        }
        else
        {
            presentItems.Remove(Pencil);
            if (selectedItem == Pencil)
                selectedItem = None;
        }

        Debug.Log("[InventoryState] SetPencil = " + value);
    }

    public static bool HasCheese()
    {
        return pastItems.Contains(Cheese);
    }

    public static void SetCheese(bool value)
    {
        if (value)
        {
            if (!pastItems.Contains(Cheese))
                pastItems.Add(Cheese);
        }
        else
        {
            pastItems.Remove(Cheese);
            if (selectedItem == Cheese)
                selectedItem = None;
        }

        Debug.Log("[InventoryState] SetCheese = " + value);
    }

    public static bool HasMouse()
    {
        return pastItems.Contains(Mouse);
    }

    public static void SetMouse(bool value)
    {
        if (value)
        {
            if (!pastItems.Contains(Mouse))
                pastItems.Add(Mouse);
        }
        else
        {
            pastItems.Remove(Mouse);
            if (selectedItem == Mouse)
                selectedItem = None;
        }

        Debug.Log("[InventoryState] SetMouse = " + value);
    }

    public static bool IsMouseHoleSolved()
{
    return mouseHoleSolved;
}

public static void SetMouseHoleSolved(bool value)
{
    mouseHoleSolved = value;
    Debug.Log("[InventoryState] SetMouseHoleSolved = " + value);
}

public static void LoadFromSlot(int slot)
{
    ClearAll();

    SaveSystem.SaveData data = SaveSystem.Load(slot);
    if (data == null)
    {
        Debug.Log("[InventoryState] LoadFromSlot -> brak danych w slocie " + slot);
        return;
    }

    SetPencil(data.hasPencil);
    SetCheese(data.hasCheese);
    SetMouse(data.hasMouse);
    SetPaperRevealed(data.paperRevealed);
    SetMouseHoleSolved(data.mouseHoleSolved);
    SetPastCabinetOpened(data.pastCabinetOpened);

    Debug.Log(
        "[InventoryState] LoadFromSlot(" + slot + ")" +
        " | hasPencil=" + data.hasPencil +
        " | hasCheese=" + data.hasCheese +
        " | hasMouse=" + data.hasMouse +
        " | paperRevealed=" + data.paperRevealed
    );
}

    public static void ClearAll()
    {
        presentItems.Clear();
        pastItems.Clear();
        selectedItem = None;
        paperRevealed = false;
        mouseHoleSolved = false;
        pastCabinetOpened = false;
        Debug.Log("[InventoryState] ClearAll()");
    }
}*/


