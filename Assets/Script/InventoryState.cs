using UnityEngine;

public static class InventoryState
{
    private const string PencilKey = "INV_PENCIL";

    public static bool HasPencil()
    {
        return PlayerPrefs.GetInt(PencilKey, 0) == 1;
    }

    public static void SetPencil(bool value)
    {
        PlayerPrefs.SetInt(PencilKey, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void ClearAll()
    {
        PlayerPrefs.DeleteKey(PencilKey);
        PlayerPrefs.Save();
    }
}