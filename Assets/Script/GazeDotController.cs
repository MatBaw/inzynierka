using System;
using System.Reflection;
using UnityEngine;
using Tobii.GameIntegration.Net;

public class GazeDotController : MonoBehaviour
{
    [SerializeField] RectTransform dot;
    [SerializeField] Canvas canvas;

    [Header("Smoothing (anti-jitter)")]
    [SerializeField] bool smoothingEnabled = true;
    [Tooltip("Więcej = stabilniej, ale większy lag. Polecam 0.06–0.12")]
    [SerializeField] float halfLife = 0.08f;
    [Tooltip("Ignoruj mikrodrgania mniejsze niż tyle pikseli")]
    [SerializeField] float deadzonePx = 6f;

    private Vector2 _smoothedScreen;
    private bool _hasSmoothed;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
    void Start()
    {
        if (dot == null) dot = GetComponent<RectTransform>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();

        var hwnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
        bool ok = TobiiGameIntegrationApi.TrackWindow(hwnd);
        Debug.Log($"TGI TrackWindow => {ok}, hwnd={hwnd}");
    }

    void Update()
    {
        TobiiGameIntegrationApi.Update();

        var points = TobiiGameIntegrationApi.GetGazePoints();
        if (points == null || points.Count == 0) return;

        var gp = points[points.Count - 1];

        if (!TryGetNormalizedXY(gp, out float nx, out float ny))
            return;

        float x01 = Mathf.Clamp01((nx + 1f) * 0.5f);
        float y01 = Mathf.Clamp01((ny + 1f) * 0.5f);

        Vector2 screen = new Vector2(x01 * Screen.width, y01 * Screen.height);

        if (smoothingEnabled)
            screen = SmoothScreen(screen, Time.deltaTime);

        var canvasRect = canvas.transform as RectTransform;
        var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screen, cam, out var local))
            dot.anchoredPosition = local;
    }

    Vector2 SmoothScreen(Vector2 raw, float dt)
    {
        if (!_hasSmoothed)
        {
            _smoothedScreen = raw;
            _hasSmoothed = true;
            return raw;
        }

        // deadzone: jeśli mały ruch, zostaw poprzednią pozycję
        if ((raw - _smoothedScreen).sqrMagnitude < deadzonePx * deadzonePx)
            return _smoothedScreen;

        // EMA z half-life, stabilne niezależnie od FPS
        float hl = Mathf.Max(halfLife, 0.0001f);
        float alpha = 1f - Mathf.Exp(-Mathf.Log(2f) * dt / hl);

        _smoothedScreen = Vector2.Lerp(_smoothedScreen, raw, alpha);
        return _smoothedScreen;
    }

    static bool TryGetNormalizedXY(GazePoint gp, out float x, out float y)
    {
        x = y = 0f;

        // 1) gp.X / gp.Y jako field albo property
        if (TryGetFloatMember(gp, "X", out x) && TryGetFloatMember(gp, "Y", out y))
            return true;

        // 2) gp.Position.X / gp.Position.Y
        if (TryGetObjMember(gp, "Position", out var pos) && pos != null)
        {
            if (TryGetFloatMember(pos, "X", out x) && TryGetFloatMember(pos, "Y", out y))
                return true;
        }

        return false;
    }

    static bool TryGetFloatMember(object obj, string name, out float value)
    {
        value = 0f;
        if (!TryGetObjMember(obj, name, out var raw) || raw == null) return false;

        try { value = Convert.ToSingle(raw); return true; }
        catch { return false; }
    }

    static bool TryGetObjMember(object obj, string name, out object value)
    {
        value = null;
        if (obj == null) return false;

        var t = obj.GetType();

        var f = t.GetField(name, BindingFlags.Public | BindingFlags.Instance);
        if (f != null) { value = f.GetValue(obj); return true; }

        var p = t.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
        if (p != null && p.GetIndexParameters().Length == 0)
        {
            value = p.GetValue(obj);
            return true;
        }

        return false;
    }
#else
    void Start()
    {
        if (dot == null) dot = GetComponent<RectTransform>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
        Debug.Log("GazeDotController: działa w Windows buildzie (nie w Unity Editor).");
    }
#endif
}
