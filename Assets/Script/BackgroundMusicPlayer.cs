using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    private static BackgroundMusicPlayer instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}