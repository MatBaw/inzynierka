using TMPro;
using UnityEngine;

public class EyeTrackingDebugText : MonoBehaviour
{
    [SerializeField] private TMP_Text debugText;

    private void Start()
    {
        if (debugText == null) return;

        int eyeTracking = PlayerPrefs.GetInt("EyeTrackingEnabled", -1);
        int sceneInteraction = PlayerPrefs.GetInt("EyeInteractionEnabledInScene", -1);

        debugText.text =
            "EyeTrackingEnabled = " + eyeTracking + "\n" +
            "EyeInteractionEnabledInScene = " + sceneInteraction;
    }
}