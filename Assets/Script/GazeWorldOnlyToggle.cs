using UnityEngine;
using UnityEngine.UI;

public class GazeWorldOnlyToggle : MonoBehaviour
{
    [Header("Wyłączane / włączane")]
    [SerializeField] private GazeDwellClick2D gazeClick2D;
    [SerializeField] private GazeEdgeButtons gazeEdgeButtons;

    [Header("Ma zostać aktywne")]
    [SerializeField] private GazeDwellClickUI gazeClickUI;
    [SerializeField] private GameObject gazeDot;
    [SerializeField] private GazeDotController gazeDotController;
    [SerializeField] private GazeCursorRingUI gazeCursorRingUI;

    [Header("Ikona")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite eyeOpenSprite;
    [SerializeField] private Sprite eyeClosedSprite;

    private const string PrefKey = "GazeWorldEnabled";

    public static bool IsEnabled()
    {
        return PlayerPrefs.GetInt(PrefKey, 1) == 1;
    }

    private void Start()
    {
        Apply(IsEnabled());
    }

    public void SetEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(PrefKey, enabled ? 1 : 0);
        PlayerPrefs.Save();
        Apply(enabled);
    }

    public void Toggle()
    {
        SetEnabled(!IsEnabled());
    }

    private void Apply(bool enabled)
    {
        if (gazeClick2D != null) gazeClick2D.enabled = enabled;
        if (gazeEdgeButtons != null) gazeEdgeButtons.enabled = enabled;

        if (gazeClickUI != null) gazeClickUI.enabled = true;

        if (gazeDot != null) gazeDot.SetActive(true);
        if (gazeDotController != null) gazeDotController.enabled = true;
        if (gazeCursorRingUI != null) gazeCursorRingUI.enabled = true;

        if (!enabled && gazeCursorRingUI != null)
            gazeCursorRingUI.SetIdle();

        if (iconImage != null)
            iconImage.sprite = enabled ? eyeOpenSprite : eyeClosedSprite;

        Debug.Log("[GazeWorldOnlyToggle] world enabled = " + enabled);
    }
}