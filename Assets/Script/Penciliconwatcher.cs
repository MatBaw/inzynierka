using UnityEngine;

/// <summary>
/// Dodaj ten skrypt na obiekt PencilIcon.
/// Będzie logował KTO i SKĄD wywołuje SetActive(false).
/// Po znalezieniu winowajcy usuń ten skrypt.
/// </summary>
public class PencilIconWatcher : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log($"[PencilIconWatcher] ✅ WŁĄCZONY\n{new System.Diagnostics.StackTrace(true)}");
    }

    void OnDisable()
    {
        Debug.LogWarning($"[PencilIconWatcher] ❌ WYŁĄCZONY — winowajca:\n{new System.Diagnostics.StackTrace(true)}");
    }
}
