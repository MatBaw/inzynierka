using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSequence : MonoBehaviour
{
    [System.Serializable]
    public class SubtitleLine
    {
        public float startTime;
        public float endTime;

        [TextArea(2, 4)]
        public string text;
    }

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("UI")]
    public TMP_Text subtitleText;

    [Header("Subtitles")]
    public List<SubtitleLine> subtitles = new List<SubtitleLine>();

    [Header("Scene flow")]
    public string returnSceneName = "MenuScene";

    private bool finished = false;

    private void Start()
    {
        if (subtitleText != null)
            subtitleText.text = "";

        if (audioSource != null)
            audioSource.Play();
    }

    private void Update()
    {
        if (audioSource == null || subtitleText == null || finished)
            return;

        float t = audioSource.time;
        subtitleText.text = "";

        foreach (var line in subtitles)
        {
            if (t >= line.startTime && t <= line.endTime)
            {
                subtitleText.text = line.text;
                break;
            }
        }

        if (!audioSource.isPlaying && audioSource.time > 0.05f)
        {
            finished = true;
            SceneManager.LoadScene(returnSceneName);
        }
    }

    public void SkipIntro()
    {
        if (finished)
            return;

        finished = true;
        SceneManager.LoadScene(returnSceneName);
    }
}