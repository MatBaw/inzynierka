using UnityEngine;

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
