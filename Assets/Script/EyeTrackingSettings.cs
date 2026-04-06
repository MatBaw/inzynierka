using UnityEngine;
using UnityEngine.UI;

public class EyeTrackingSettings : MonoBehaviour
{
    [Header("UI objects to show/hide")]
    [SerializeField] private GameObject gazeDot;

    [Header("Toggle button image")]
    [SerializeField] private Image toggleButtonImage;
    [SerializeField] private Sprite eyeTrackerOnSprite;
    [SerializeField] private Sprite eyeTrackerOffSprite;

    [Header("Default behavior")]
    [SerializeField] private bool forceDefaultOnAtStart = true;

    [Header("Scripts to enable/disable")]
    [SerializeField] private GazeDwellClick2D gazeClick2D;
    [SerializeField] private GazeDwellClickUI gazeClickUI;
    [SerializeField] private GazeEdgeButtons gazeEdgeButtons;
    [SerializeField] private GazeDotController gazeDotController;
    [SerializeField] private GazeCursorRingUI gazeCursorRingUI;

    private const string PrefKey = "EyeTrackingEnabled";
    private const string SceneInteractionPrefKey = "EyeInteractionEnabledInScene";

    public static bool IsEnabled()
    {
        return PlayerPrefs.GetInt(PrefKey, 1) == 1;
    }

    private void Awake()
    {
        if (forceDefaultOnAtStart)
        {
            PlayerPrefs.SetInt(PrefKey, 1);
            PlayerPrefs.SetInt(SceneInteractionPrefKey, 1);
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
        Apply(IsEnabled());
    }

    public void Toggle()
    {
        SetEnabled(!IsEnabled());
    }

    public void SetEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(PrefKey, enabled ? 1 : 0);

        // Gdy globalnie włączamy eye tracking, domyślnie włączamy też interakcję sceny
        if (enabled)
            PlayerPrefs.SetInt(SceneInteractionPrefKey, 1);

        PlayerPrefs.Save();

        Apply(enabled);

        Debug.Log("[EyeTrackingSettings] enabled = " + enabled);
    }

    private void Apply(bool enabled)
    {
        if (gazeDot != null)
            gazeDot.SetActive(enabled);

        if (gazeClick2D != null)
            gazeClick2D.enabled = enabled;

        if (gazeClickUI != null)
            gazeClickUI.enabled = enabled;

        if (gazeEdgeButtons != null)
            gazeEdgeButtons.enabled = enabled;

        if (gazeDotController != null)
            gazeDotController.enabled = enabled;

        if (gazeCursorRingUI != null)
            gazeCursorRingUI.enabled = enabled;

        RefreshButtonVisual(enabled);
    }

    private void RefreshButtonVisual(bool enabled)
    {
        if (toggleButtonImage == null)
            return;

        toggleButtonImage.sprite = enabled ? eyeTrackerOnSprite : eyeTrackerOffSprite;
    }
}








/*using UnityEngine;

public class EyeTrackingSettings : MonoBehaviour
{
    [Header("UI objects to show/hide")]
    [SerializeField] GameObject gazeDot;
    [SerializeField] GameObject eyeToggleButton;

    [Header("Scripts to enable/disable")]
    [SerializeField] GazeDwellClick2D gazeClick2D;
    [SerializeField] GazeDwellClickUI gazeClickUI;
    [SerializeField] GazeEdgeButtons gazeEdgeButtons;
    [SerializeField] GazeDotController gazeDotController;
    [SerializeField] GazeCursorRingUI gazeCursorRingUI;

    const string PrefKey = "EyeTrackingEnabled";

    public static bool IsEnabled()
    {
        return PlayerPrefs.GetInt(PrefKey, 1) == 1;
    }

   void Start()
{
    bool hasKey = PlayerPrefs.HasKey(PrefKey);
    int val = PlayerPrefs.GetInt(PrefKey, 1);
    Debug.Log($"[EyeSettings] Start — hasKey={hasKey}, val={val}, IsEnabled={IsEnabled()}");
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

    void Apply(bool enabled)
    {
        if (gazeDot != null)          gazeDot.SetActive(enabled);
        if (eyeToggleButton != null)  eyeToggleButton.SetActive(true);
        if (gazeClick2D != null)      gazeClick2D.enabled      = enabled;
        if (gazeClickUI != null)      gazeClickUI.enabled      = enabled;
        if (gazeEdgeButtons != null)  gazeEdgeButtons.enabled  = enabled;
        if (gazeDotController != null) gazeDotController.enabled = enabled;
        if (gazeCursorRingUI != null)  gazeCursorRingUI.enabled  = enabled;
    }
}*/
