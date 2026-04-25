using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFadeTransition : MonoBehaviour
{
    public static SceneFadeTransition Instance;

    [Header("Overlay")]
    [SerializeField] private Image overlayImage;

    [Header("Fade settings")]
    [SerializeField] private float fadeDuration = 0.35f;
    [SerializeField] private Color fadeColor = new Color(0f, 0f, 0f, 0f);
    [SerializeField] private float targetAlpha = 0.75f;

    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (overlayImage != null)
        {
            Color c = fadeColor;
            c.a = 0f;
            overlayImage.color = c;
        }
    }

    public void FadeToScene(string sceneName)
    {
        Debug.Log("FADE TO SCENE: " + sceneName);
        
        if (isTransitioning) return;
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isTransitioning = true;
        yield return Fade(0f, targetAlpha);
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(Fade(targetAlpha, 0f, true));
    }

    private IEnumerator Fade(float from, float to, bool finish = false)
    {
        if (overlayImage == null) yield break;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float n = Mathf.Clamp01(t / fadeDuration);

            Color c = fadeColor;
            c.a = Mathf.Lerp(from, to, n);
            overlayImage.color = c;

            yield return null;
        }

        Color finalColor = fadeColor;
        finalColor.a = to;
        overlayImage.color = finalColor;

        if (finish)
            isTransitioning = false;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}