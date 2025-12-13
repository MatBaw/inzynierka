using System.Reflection;
using UnityEngine;

public class TgiBuildSmokeTest : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
    void Start()
    {
        // TrackWindow mapuje gaze do okna gry :contentReference[oaicite:5]{index=5}
        var hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

        // Jeśli u Ciebie TrackWindow ma inną nazwę, podmień zgodnie z Ctrl+F w TobiiGameIntegrationApi.cs
        var ok = Tobii.GameIntegration.Net.TobiiGameIntegrationApi.TrackWindow(hwnd);
        Debug.Log($"TGI TrackWindow => {ok}, hwnd={hwnd}");
    }

    void Update()
    {
        // Update MUSI być co tick :contentReference[oaicite:6]{index=6}
        Tobii.GameIntegration.Net.TobiiGameIntegrationApi.Update();

        if (Tobii.GameIntegration.Net.TobiiGameIntegrationApi.TryGetLatestGazePoint(out var gp))
        {
            Debug.Log("GAZE: " + Dump(gp));
        }
    }

    void OnApplicationQuit()
    {
        Tobii.GameIntegration.Net.TobiiGameIntegrationApi.Shutdown(); // :contentReference[oaicite:7]{index=7}
    }

    private static string Dump<T>(T value)
    {
        var t = typeof(T);
        var fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
        var props  = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        string s = t.Name + " ";
        foreach (var f in fields) s += $"{f.Name}={f.GetValue(value)} ";
        foreach (var p in props) if (p.GetIndexParameters().Length == 0) s += $"{p.Name}={p.GetValue(value)} ";
        return s;
    }
#else
    void Start()
    {
        Debug.Log("TGI: ten test działa w Standalone Windows build (nie w Unity Editor).");
    }
#endif
}
