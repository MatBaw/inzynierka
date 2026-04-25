using UnityEngine;

public class GazeDotWatcher : MonoBehaviour
{
    void Awake()
    {
        Debug.Log($"[GazeDotWatcher] Awake — activeInHierarchy={gameObject.activeInHierarchy}");
    }

    void Start()
    {
        Debug.Log($"[GazeDotWatcher] Start — activeInHierarchy={gameObject.activeInHierarchy}");
    }

    void OnEnable()
    {
        Debug.Log($"[GazeDotWatcher] OnEnable");
    }

    void OnDisable()
    {
        Debug.Log($"[GazeDotWatcher] OnDisable — kto to zrobił:\n{new System.Diagnostics.StackTrace()}");
    }
}