using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Główny skrypt menu startowego.
/// 
/// SETUP:
/// 1. Utwórz nową scenę "MenuScene" (File → New Scene)
/// 2. Dodaj ten skrypt na pusty GameObject "MenuManager"
/// 3. Podepnij referencje w Inspectorze
/// 4. W Build Settings dodaj MenuScene (index 0) i SampleScene (index 1)
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("=== PRZYCISKI ===")]
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    [Header("=== SCENA GRY ===")]
    [SerializeField] string gameSceneName = "SlotScene";

    [Header("=== FADE ===")]
    [SerializeField] Image fadePanel;       // czarny panel do fade-in/out
    [SerializeField] float fadeDuration = 1f;

    [Header("=== ANIMOWANE ELEMENTY (opcjonalne) ===")]
    [SerializeField] CanvasGroup menuGroup; // cała grupa menu do fade-in

    void Start()
    {
        // Podepnij przyciski
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);

        // Fade in przy starcie menu
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
        // Zablokuj przyciski podczas przejścia
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
