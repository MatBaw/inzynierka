using UnityEngine;
using UnityEngine.UI;
using Graphic = UnityEngine.UI.Graphic;

public class SceneEyeInteractionToggle : MonoBehaviour
{
    [Header("Icon")]
    public Image iconImage;
    public Sprite eyeOpenSprite;
    public Sprite eyeClosedSprite;

    [Header("World interaction")]
    public GazeDwellClick2D gazeClick2D;
    public GazeEdgeButtons gazeEdgeButtons;

    [Header("UI graphics to block from gaze")]
    public Graphic[] uiRaycastToDisable;

    private bool interactionEnabled = true;

    private void OnEnable()
    {
        interactionEnabled = PlayerPrefs.GetInt("EyeInteractionEnabledInScene", 1) == 1;
        ApplyState();
    }

    public void ToggleSceneInteraction()
    {
        interactionEnabled = !interactionEnabled;
        PlayerPrefs.SetInt("EyeInteractionEnabledInScene", interactionEnabled ? 1 : 0);
        PlayerPrefs.Save();

        ApplyState();
    }

    private void ApplyState()
    {
        if (gazeClick2D != null)
            gazeClick2D.enabled = interactionEnabled;

        if (gazeEdgeButtons != null)
            gazeEdgeButtons.enabled = interactionEnabled;

        if (uiRaycastToDisable != null)
        {
            foreach (var g in uiRaycastToDisable)
            {
                if (g != null)
                    g.raycastTarget = interactionEnabled;
            }
        }

        if (iconImage != null)
            iconImage.sprite = interactionEnabled ? eyeOpenSprite : eyeClosedSprite;
    }
}