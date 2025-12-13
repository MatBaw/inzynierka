using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class TobiiWhichApi : MonoBehaviour
{
    void Start()
    {
        var results = new List<string>();

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types;
            try
            {
                types = asm.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types.Where(t => t != null).ToArray();
            }
            catch
            {
                continue;
            }

            foreach (var t in types)
            {
                if (t == null) continue;
                if (t.Name != "TobiiGameIntegrationApi") continue;

                var methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                               .OrderBy(m => m.Name)
                               .Select(m => m.ToString());

                results.Add("FOUND in asm: " + asm.GetName().Name + " :: " + t.FullName +
                            "\n" + string.Join("\n", methods));
            }
        }

        if (results.Count == 0)
        {
            Debug.LogError("Nie znalazłem typu TobiiGameIntegrationApi w żadnym assembly.");
            return;
        }

        Debug.Log(string.Join("\n\n", results));
    }
}
