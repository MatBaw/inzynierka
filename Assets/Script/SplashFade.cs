using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeInTime = 1.5f;
    public float visibleTime = 2f;
    public float fadeOutTime = 1.5f;
    public string nextSceneName = "MainMenu";

    void Start()
    {
        StartCoroutine(PlaySplash());
    }

    IEnumerator PlaySplash()
    {
        // Fade In
        yield return StartCoroutine(Fade(0f, 1f, fadeInTime));

        // Czekanie
        yield return new WaitForSeconds(visibleTime);

        // Fade Out
        yield return StartCoroutine(Fade(1f, 0f, fadeOutTime));

        // Przejście do następnej sceny
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        canvasGroup.alpha = startAlpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}