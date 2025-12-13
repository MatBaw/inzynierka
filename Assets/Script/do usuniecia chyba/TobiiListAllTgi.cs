using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class TobiiListAllTgi : MonoBehaviour
{
    void Start()
    {
        // Szukamy typów z przestrzeni Tobii.GameIntegration.Net w Assembly-CSharp (Twoje .cs)
        var asm = Assembly.GetExecutingAssembly();

        var types = asm.GetTypes()
            .Where(t => t.Namespace != null && t.Namespace.StartsWith("Tobii.GameIntegration.Net"))
            .OrderBy(t => t.FullName)
            .ToList();

        Debug.Log("TGI types in Assembly-CSharp:\n" + string.Join("\n", types.Select(t => t.FullName)));

        string[] keys = { "Update", "Track", "Gaze", "Shutdown", "Init", "Connect", "Enable" };

        foreach (var t in types)
        {
            var ms = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                      .Where(m => m.DeclaringType == t)
                      .Where(m => keys.Any(k => m.Name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0))
                      .OrderBy(m => m.Name)
                      .ToList();

            if (ms.Count == 0) continue;

            Debug.Log($"--- {t.FullName} KEY METHODS ---\n" +
                      string.Join("\n", ms.Select(m => m.ToString())));
        }
    }
}
