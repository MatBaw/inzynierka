using UnityEngine;
using UnityEngine.UI;

public class SceneEyeInteractionToggle : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] private GameObject iconRoot;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite eyeOpenSprite;
    [SerializeField] private Sprite eyeClosedSprite;

    [Header("World interaction to disable")]
    [SerializeField] private GazeDwellClick2D gazeClick2D;
    [SerializeField] private GazeEdgeButtons gazeEdgeButtons;

    private const string GlobalPrefKey = "EyeTrackingEnabled";
    private const string SceneInteractionPrefKey = "EyeInteractionEnabledInScene";

    public static bool IsGlobalEyeTrackingEnabled()
    {
        return PlayerPrefs.GetInt(GlobalPrefKey, 1) == 1;
    }

    public static bool IsSceneInteractionEnabled()
    {
        return PlayerPrefs.GetInt(SceneInteractionPrefKey, 1) == 1;
    }

    private void Start()
    {
        ApplyCurrentState();
        Debug.Log("[SceneEyeInteractionToggle] Start | gazeClick2D=" + gazeClick2D + " | gazeEdgeButtons=" + gazeEdgeButtons);
    }

    public void ToggleSceneInteraction()
    {
        Debug.Log("[SceneEyeInteractionToggle] ToggleSceneInteraction CALLED");

        if (!IsGlobalEyeTrackingEnabled())
        {
            Debug.Log("[SceneEyeInteractionToggle] Global eye tracking OFF");
            return;
        }

        bool next = !IsSceneInteractionEnabled();
        PlayerPrefs.SetInt(SceneInteractionPrefKey, next ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("[SceneEyeInteractionToggle] Scene interaction now = " + next);

        ApplyCurrentState();
    }

    public void ApplyCurrentState()
    {
        bool globalEnabled = IsGlobalEyeTrackingEnabled();

        if (!globalEnabled)
        {
            if (iconRoot != null)
                iconRoot.SetActive(false);

            SetSceneInteractionEnabled(false);
            return;
        }

        if (iconRoot != null)
            iconRoot.SetActive(true);

        bool sceneInteractionEnabled = IsSceneInteractionEnabled();

        SetSceneInteractionEnabled(sceneInteractionEnabled);
        RefreshIcon(sceneInteractionEnabled);
    }

    private void SetSceneInteractionEnabled(bool enabled)
    {
        if (gazeClick2D != null)
            gazeClick2D.enabled = enabled;

        if (gazeEdgeButtons != null)
            gazeEdgeButtons.enabled = enabled;

        Debug.Log("[SceneEyeInteractionToggle] SetSceneInteractionEnabled = " + enabled);
    }

    private void RefreshIcon(bool interactionEnabled)
    {
        if (iconImage == null)
            return;

        iconImage.sprite = interactionEnabled ? eyeOpenSprite : eyeClosedSprite;
    }
}