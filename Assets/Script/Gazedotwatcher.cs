using UnityEngine;

/// TYMCZASOWY skrypt diagnostyczny.
/// Dodaj na GazeDot w każdej scenie.
/// Sprawdź Player.log po uruchomieniu buildu.
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
        // To pokaże nam kto wyłącza GazeDot i skąd
        Debug.Log($"[GazeDotWatcher] OnDisable — kto to zrobił:\n{new System.Diagnostics.StackTrace()}");
    }
}