using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSave : MonoBehaviour
{
    private static AutoSave _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Nie zapisuj scen menu / slotów / ekranów UI
        if (scene.name == "MenuScene" || scene.name == "SlotScene")
        {
            Debug.Log("[AutoSave] Pomijam zapis dla sceny UI: " + scene.name);
            return;
        }

        SaveSystem.SaveCurrent(scene.name);
        Debug.Log("[AutoSave] Zapisano przy załadowaniu sceny: " + scene.name);
    }
}








/*using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Automatyczny zapis przy zmianie sceny.
/// Dodaj na obiekt DontDestroyOnLoad (np. GameManager).
/// </summary>
public class AutoSave : MonoBehaviour
{
    private static AutoSave _instance;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Nie zapisuj przy wczytywaniu menu
        if (scene.name == "MenuScene") return;

        SaveSystem.SaveCurrent(scene.name);
        Debug.Log($"[AutoSave] Zapisano przy załadowaniu sceny: {scene.name}");
    }
}*/
