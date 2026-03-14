using UnityEngine;
using UnityEngine.Rendering;

public class HighContrastApplier : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    private const string Key = "HighContrast";

    void Start()
    {
        bool on = PlayerPrefs.GetInt(Key, 0) == 1;
        if (globalVolume != null) globalVolume.enabled = on;
    }
}