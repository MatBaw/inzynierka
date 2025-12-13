using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class TobiiFindEntry : MonoBehaviour
{
    void Start()
    {
        var asm = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "Tobii.GameIntegration.Net");

        Debug.Log("ASM: " + (asm == null ? "NOT FOUND" : asm.FullName));
        if (asm == null) return;

        // 1) Znajdź KAŻDĄ metodę (public i non-public), której nazwa zawiera "GetApi"
        var hits = asm.GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                .Where(m => m.Name.IndexOf("GetApi", StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(m => (type: t, method: m)))
            .ToList();

        if (hits.Count == 0)
        {
            Debug.LogWarning("Nie znalazłem żadnej metody zawierającej 'GetApi'. Szukam 'Api' w nazwach metod…");
        }
        else
        {
            Debug.Log("=== GetApi hits ===");
            foreach (var h in hits)
                Debug.Log($"{h.type.FullName} :: {h.method}");
        }

        // 2) Dodatkowo: wypisz publiczne metody klas, które wyglądają na główne moduły
        var keyTypes = asm.GetTypes()
            .Where(t => t.FullName != null && (
                        t.FullName.Contains("TobiiGameIntegrationApi") ||
                        t.FullName.Contains("Tracker") ||
                        t.FullName.Contains("StreamsProvider") ||
                        t.FullName.Contains("Streams")))
            .OrderBy(t => t.FullName)
            .ToList();

        Debug.Log("=== Key types ===\n" + string.Join("\n", keyTypes.Select(t => t.FullName)));

        foreach (var t in keyTypes)
        {
            var pub = t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                       .Where(m => m.DeclaringType == t)
                       .ToArray();

            if (pub.Length == 0) continue;
            Debug.Log($"--- {t.FullName} PUBLIC METHODS ---\n" + string.Join("\n", pub.Select(m => m.ToString())));
        }
    }
}
