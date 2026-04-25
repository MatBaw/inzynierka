using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class RoomCycleLoader : MonoBehaviour
{
    [Header("Scene name format: presentroom1..4 and pastroom1..4")]
    [SerializeField] int minIndex = 1;
    [SerializeField] int maxIndex = 4;

    [Header("Cooldown (shared across scenes)")]
    [SerializeField] float cooldownSeconds = 3f;

    static float globalNextAllowedTime = 0f;
    static bool globalIsLoading = false;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        globalIsLoading = false;
    }

    bool CanLoad()
    {
        if (globalIsLoading) return false;
        if (Time.unscaledTime < globalNextAllowedTime) return false;

        globalNextAllowedTime = Time.unscaledTime + cooldownSeconds;
        globalIsLoading = true;
        return true;
    }

    public void Next()
    {
        if (!CanLoad()) return;

        if (!TryParseCurrent(out string prefix, out int number))
        {
            globalIsLoading = false;
            return;
        }

        int next = number + 1;
        if (next > maxIndex) next = minIndex;

        SceneManager.LoadScene(prefix + next);
    }

    public void Prev()
    {
        if (!CanLoad()) return;

        if (!TryParseCurrent(out string prefix, out int number))
        {
            globalIsLoading = false;
            return;
        }

        int prev = number - 1;
        if (prev < minIndex) prev = maxIndex;

        SceneManager.LoadScene(prefix + prev);
    }

    bool TryParseCurrent(out string prefix, out int number)
    {
        prefix = null;
        number = 0;

        string name = SceneManager.GetActiveScene().name;

        var m = Regex.Match(name, @"^(presentroom|pastroom)(\d+)$");
        if (!m.Success) return false;

        prefix = m.Groups[1].Value;
        number = int.Parse(m.Groups[2].Value);
        return true;
    }
}