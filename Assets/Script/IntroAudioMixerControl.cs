using UnityEngine;
using UnityEngine.Audio;

public class IntroAudioMixerControl : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private string musicVolumeParameter = "MusicVolume";

    [SerializeField] private float introMusicDb = -80f;
    [SerializeField] private float normalMusicDb = 0f;

    private void Start()
    {
        if (mainMixer != null)
            mainMixer.SetFloat(musicVolumeParameter, introMusicDb);
    }

    private void OnDestroy()
    {
        if (mainMixer != null)
            mainMixer.SetFloat(musicVolumeParameter, normalMusicDb);
    }
}