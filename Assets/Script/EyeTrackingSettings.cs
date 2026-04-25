using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EyeTrackingSettings : MonoBehaviour
{
    [Header("UI objects to show/hide")]
    public GameObject gazeDot;
    public GameObject sceneEyeToggleButton;

    [Header("Toggle button image")]
    public Image toggleButtonImage;
    public Sprite eyeTrackerOnSprite;
    public Sprite eyeTrackerOffSprite;

    [Header("Default behavior")]
    public bool forceDefaultOnAtStart = false;

    [Header("Scripts to enable/disable")]
    public GazeDwellClick2D gazeClick2D;
    public GazeDwellClickUI gazeClickUI;
    public GazeEdgeButtons gazeEdgeButtons;
    public GazeDotController gazeDotController;
    public GazeCursorRingUI gazeCursorRingUI;

    private bool isEyeTrackingEnabled = true;

    private void Awake()
    {
        Debug.Log("[EyeTrackingSettings] Awake START | Scene=" + SceneManager.GetActiveScene().name
            + " | Saved EyeTrackingEnabled=" + PlayerPrefs.GetInt("EyeTrackingEnabled", -999)
            + " | forceDefaultOnAtStart=" + forceDefaultOnAtStart
            + " | object=" + gameObject.name);

        if (forceDefaultOnAtStart)
        {
            Debug.LogWarning("[EyeTrackingSettings] FORCE DEFAULT ON fired on object: " + gameObject.name
                + " | Scene=" + SceneManager.GetActiveScene().name);

            PlayerPrefs.SetInt("EyeTrackingEnabled", 1);
            PlayerPrefs.SetInt("EyeInteractionEnabledInScene", 1);
            PlayerPrefs.Save();
        }

        isEyeTrackingEnabled = PlayerPrefs.GetInt("EyeTrackingEnabled", 1) == 1;

        Debug.Log("[EyeTrackingSettings] Awake AFTER READ | Scene=" + SceneManager.GetActiveScene().name
            + " | EyeTrackingEnabled=" + isEyeTrackingEnabled
            + " | object=" + gameObject.name);

        ApplyState();
    }

    public void Toggle()
    {
        isEyeTrackingEnabled = !isEyeTrackingEnabled;

        Debug.LogWarning("[EyeTrackingSettings] Toggle | Scene=" + SceneManager.GetActiveScene().name
            + " | NEW EyeTrackingEnabled=" + isEyeTrackingEnabled
            + " | object=" + gameObject.name);

        PlayerPrefs.SetInt("EyeTrackingEnabled", isEyeTrackingEnabled ? 1 : 0);
        PlayerPrefs.SetInt("EyeInteractionEnabledInScene", isEyeTrackingEnabled ? 1 : 0);
        PlayerPrefs.Save();

        ApplyState();
    }

    private void ApplyState()
    {
        if (gazeClick2D != null)
            gazeClick2D.enabled = isEyeTrackingEnabled;

        if (gazeClickUI != null)
            gazeClickUI.enabled = isEyeTrackingEnabled;

        if (gazeEdgeButtons != null)
            gazeEdgeButtons.enabled = isEyeTrackingEnabled;

        if (gazeDotController != null)
            gazeDotController.enabled = isEyeTrackingEnabled;

        if (gazeCursorRingUI != null)
            gazeCursorRingUI.enabled = isEyeTrackingEnabled;

        if (gazeDot != null)
            gazeDot.SetActive(isEyeTrackingEnabled);

        if (sceneEyeToggleButton != null)
            sceneEyeToggleButton.SetActive(isEyeTrackingEnabled);

        if (toggleButtonImage != null)
            toggleButtonImage.sprite = isEyeTrackingEnabled ? eyeTrackerOnSprite : eyeTrackerOffSprite;

        Debug.Log("[EyeTrackingSettings] ApplyState | Scene=" + SceneManager.GetActiveScene().name
            + " | EyeTrackingEnabled=" + isEyeTrackingEnabled
            + " | GazeDot=" + (gazeDot != null ? gazeDot.activeSelf.ToString() : "NULL")
            + " | SceneEyeToggleButton=" + (sceneEyeToggleButton != null ? sceneEyeToggleButton.activeSelf.ToString() : "NULL")
            + " | object=" + gameObject.name);
    }
}