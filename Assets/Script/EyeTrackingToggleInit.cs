using UnityEngine;
using UnityEngine.UI;

public class EyeTrackingToggleInit : MonoBehaviour
{
    [SerializeField] Toggle toggle;

    void Awake()
    {
        if (toggle == null) toggle = GetComponent<Toggle>();
        // Czytaj rzeczywisty stan z PlayerPrefs
        toggle.SetIsOnWithoutNotify(EyeTrackingSettings.IsEnabled());
    }
}