using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class TobiiProbe : MonoBehaviour
{
    void Start()
    {
        // 1) Czy assembly w ogóle jest załadowane?
        var asm = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "Tobii.GameIntegration.Net");

        Debug.Log("ASM: " + (asm == null ? "NOT FOUND" : asm.FullName));
        if (asm == null) return;

        // 2) Wypisz typy i publiczne statyczne metody (entry points)
        var types = asm.GetTypes()
            .Where(t => t.FullName != null &&
                        (t.FullName.Contains("Tobii") ||
                         t.FullName.Contains("GameIntegration") ||
                         t.FullName.Contains("Api")))
            .OrderBy(t => t.FullName)
            .ToList();

        foreach (var t in types)
        {
            var ms = t.GetMethods(BindingFlags.Public | BindingFlags.Static)
                      .Where(m => m.DeclaringType == t)
                      .ToArray();

            if (ms.Length == 0) continue;

            Debug.Log($"TYPE: {t.FullName}\n" +
                      string.Join("\n", ms.Select(m => m.ToString())));
        }
    }
}
