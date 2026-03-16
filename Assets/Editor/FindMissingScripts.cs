using UnityEngine;
using UnityEditor;

public class FindMissingScripts
{
    [MenuItem("Tools/Find Missing Scripts In Open Scene")]
    public static void FindInOpenScene()
    {
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        int count = 0;

        foreach (GameObject go in allObjects)
        {
            Component[] components = go.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.Log($"Missing Script on: {GetGameObjectPath(go)}", go);
                    count++;
                }
            }
        }

        Debug.Log($"Skan zakończony. Znaleziono Missing Scripts: {count}");
    }

    private static string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform current = obj.transform.parent;

        while (current != null)
        {
            path = current.name + "/" + path;
            current = current.parent;
        }

        return path;
    }
}
