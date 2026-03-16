using UnityEngine;
using UnityEditor;

public static class FindInventoryBarUIUsage
{
    [MenuItem("Tools/Find InventoryBarUI In Open Scene")]
    public static void FindUsage()
    {
        var all = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        int count = 0;
        foreach (var mb in all)
        {
            if (mb == null) continue;
            if (mb.GetType().Name == "InventoryBarUI")
            {
                Debug.Log("InventoryBarUI found on: " + GetPath(mb.gameObject), mb.gameObject);
                count++;
            }
        }

        Debug.Log("InventoryBarUI count = " + count);
    }

    private static string GetPath(GameObject obj)
    {
        string path = obj.name;
        Transform t = obj.transform.parent;
        while (t != null)
        {
            path = t.name + "/" + path;
            t = t.parent;
        }
        return path;
    }
}
