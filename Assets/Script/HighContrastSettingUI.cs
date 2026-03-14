using UnityEngine;
using UnityEngine.UI;

public class HighContrastSettingUI : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    private const string Key = "HighContrast";

    void Start()
    {
        bool saved = PlayerPrefs.GetInt(Key, 0) == 1; // domyślnie OFF
        toggle.isOn = saved;
        toggle.onValueChanged.AddListener(v =>
        {
            PlayerPrefs.SetInt(Key, v ? 1 : 0);
            PlayerPrefs.Save();
        });
    }
}