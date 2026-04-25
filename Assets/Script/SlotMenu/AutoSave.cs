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
        if (scene.name == "MenuScene" || scene.name == "SlotScene")
        {
            Debug.Log("[AutoSave] Pomijam zapis dla sceny UI: " + scene.name);
            return;
        }

        SaveSystem.SaveCurrent(scene.name);
        Debug.Log("[AutoSave] Zapisano przy załadowaniu sceny: " + scene.name);
    }
}




