using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class MainMenuController : MonoBehaviour
{
    [Header("=== PRZYCISKI ===")]
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    [Header("=== SCENA GRY ===")]
    [SerializeField] string gameSceneName = "SlotScene";

    [Header("=== FADE ===")]
    [SerializeField] Image fadePanel;
    [SerializeField] float fadeDuration = 1f;

    [Header("=== ANIMOWANE ELEMENTY (opcjonalne) ===")]
    [SerializeField] CanvasGroup menuGroup;

    void Start()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        StartCoroutine(FadeIn());
    }

    public void OnStartClicked()
    {
        StartCoroutine(LoadGameWithFade());
    }

    public void OnQuitClicked()
    {
        StartCoroutine(QuitWithFade());
    }

    IEnumerator LoadGameWithFade()
    {
        SetButtonsInteractable(false);

        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(gameSceneName);
    }

    IEnumerator QuitWithFade()
    {
        SetButtonsInteractable(false);
        yield return StartCoroutine(FadeOut());

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator FadeIn()
    {
        if (fadePanel == null) yield break;

        fadePanel.gameObject.SetActive(true);
        Color c = fadePanel.color;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            c.a = Mathf.Lerp(1f, 0f, t);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 0f;
        fadePanel.color = c;
        fadePanel.gameObject.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        if (fadePanel == null) yield break;

        fadePanel.gameObject.SetActive(true);
        Color c = fadePanel.color;
        c.a = 0f;
        fadePanel.color = c;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            c.a = Mathf.Lerp(0f, 1f, t);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 1f;
        fadePanel.color = c;
    }

    void SetButtonsInteractable(bool value)
    {
        if (startButton != null) startButton.interactable = value;
        if (quitButton != null) quitButton.interactable = value;
    }
}
