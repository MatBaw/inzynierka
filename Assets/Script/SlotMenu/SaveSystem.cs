using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public string currentScene;

        public bool hasPencil;
        public bool hasCheese;
        public bool hasMouse;

        public bool paperRevealed;
        public bool mouseHoleSolved;
        public bool pastCabinetOpened;

        public bool keyDroppedBehindCabinet;
        public bool keyPulledOutByMouse;
        public bool keyCollected;

        public string saveDate;
        public string saveTime;
    }

    private static string SavePath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, "save_slot_" + slot + ".json");
    }

    public static int ActiveSlot
    {
        get { return PlayerPrefs.GetInt("active_save_slot", 0); }
        set { PlayerPrefs.SetInt("active_save_slot", value); }
    }

    public static void SaveCurrent(string sceneName)
    {
        string now = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

        SaveData data = new SaveData
        {
            currentScene = sceneName,

            hasPencil = InventoryState.HasPencil(),
            hasCheese = InventoryState.HasCheese(),
            hasMouse = InventoryState.HasMouse(),

            paperRevealed = InventoryState.IsPaperRevealed(),
            mouseHoleSolved = InventoryState.IsMouseHoleSolved(),
            pastCabinetOpened = InventoryState.IsPastCabinetOpened(),

            keyDroppedBehindCabinet = InventoryState.IsKeyDroppedBehindCabinet(),
            keyPulledOutByMouse = InventoryState.IsKeyPulledOutByMouse(),
            keyCollected = InventoryState.IsKeyCollected(),

            saveDate = now,
            saveTime = now
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath(ActiveSlot), json);

        Debug.Log("[SaveSystem] Zapisano slot " + ActiveSlot + ": " + json);
    }

    public static SaveData Load(int slot)
    {
        string path = SavePath(slot);
        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (string.IsNullOrEmpty(data.saveDate) && !string.IsNullOrEmpty(data.saveTime))
            data.saveDate = data.saveTime;

        if (string.IsNullOrEmpty(data.saveTime) && !string.IsNullOrEmpty(data.saveDate))
            data.saveTime = data.saveDate;

        return data;
    }

    public static bool SlotExists(int slot)
    {
        return File.Exists(SavePath(slot));
    }

    public static void Delete(int slot)
    {
        string path = SavePath(slot);
        if (File.Exists(path))
            File.Delete(path);
    }

    public static void DeleteSlot(int slot)
    {
        Delete(slot);
    }
}


















/*using System;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public string currentScene;

        public bool hasPencil;
        public bool hasCheese;
        public bool hasMouse;

        public bool paperRevealed;

        public string saveDate;
        public string saveTime;
        public bool mouseHoleSolved;

        public bool pastCabinetOpened;
    }

    private static string SavePath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, "save_slot_" + slot + ".json");
    }

    public static int ActiveSlot
    {
        get { return PlayerPrefs.GetInt("active_save_slot", 0); }
        set { PlayerPrefs.SetInt("active_save_slot", value); }
    }

    public static void SaveCurrent(string sceneName)
    {
        string now = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

        SaveData data = new SaveData
        {
            currentScene = sceneName,

            hasPencil = InventoryState.HasPencil(),
            hasCheese = InventoryState.HasCheese(),
            hasMouse = InventoryState.HasMouse(),

            paperRevealed = InventoryState.IsPaperRevealed(),

            mouseHoleSolved = InventoryState.IsMouseHoleSolved(),

            pastCabinetOpened = InventoryState.IsPastCabinetOpened(),

            saveDate = now,
            saveTime = now
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath(ActiveSlot), json);

        Debug.Log("[SaveSystem] Zapisano slot " + ActiveSlot + ": " + json);
    }

    public static SaveData Load(int slot)
    {
        string path = SavePath(slot);
        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (string.IsNullOrEmpty(data.saveDate) && !string.IsNullOrEmpty(data.saveTime))
            data.saveDate = data.saveTime;

        if (string.IsNullOrEmpty(data.saveTime) && !string.IsNullOrEmpty(data.saveDate))
            data.saveTime = data.saveDate;

        return data;
    }

    public static bool SlotExists(int slot)
    {
        return File.Exists(SavePath(slot));
    }

    public static void Delete(int slot)
    {
        string path = SavePath(slot);
        if (File.Exists(path))
            File.Delete(path);
    }

    public static void DeleteSlot(int slot)
    {
        Delete(slot);
    }
}*/
