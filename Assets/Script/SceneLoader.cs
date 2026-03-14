using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Cooldown")]
    [SerializeField] float cooldownSeconds = 0.8f;

    float nextAllowedTime = 0f;
    bool isLoading = false;

    bool CanLoad()
    {
        if (isLoading) return false;
        if (Time.unscaledTime < nextAllowedTime) return false;

        nextAllowedTime = Time.unscaledTime + cooldownSeconds;
        return true;
    }

    public void LoadSceneByName(string sceneName)
    {
        if (!CanLoad()) return;
        isLoading = true;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextBuildIndex()
    {
        if (!CanLoad()) return;
        isLoading = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousBuildIndex()
    {
        if (!CanLoad()) return;
        isLoading = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}