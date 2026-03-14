using System.Collections;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public Camera cam;

    [Header("Default zoom (used when object doesn't override)")]
    public float zoomSize = 3f;

    [Header("Animation")]
    public float zoomTime = 0.7f;

    [Header("Camera offset (keep Z = -10 in 2D)")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    public bool IsZoomed { get; private set; }
    public Transform ZoomTarget { get; private set; }

    float defaultSize;
    Vector3 defaultPos;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
        defaultSize = cam.orthographicSize;
        defaultPos = cam.transform.position;
    }

    // OLD behaviour (still works): zoom using default zoomSize
    public void ZoomTo(Transform target)
    {
        ZoomTo(target, zoomSize);
    }

    // NEW behaviour: zoom using per-object size
    public void ZoomTo(Transform target, float customZoomSize)
    {
        ZoomTarget = target;
        StopAllCoroutines();
        StartCoroutine(ZoomRoutine(target, customZoomSize));
    }

    public void ZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomOutRoutine());
    }

    IEnumerator ZoomRoutine(Transform target, float targetZoomSize)
    {
        IsZoomed = true;

        Vector3 startPos = cam.transform.position;
        float startSize = cam.orthographicSize;

        Vector3 targetPos = target.position + offset;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / zoomTime;
            cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            cam.orthographicSize = Mathf.Lerp(startSize, targetZoomSize, t);
            yield return null;
        }

        cam.transform.position = targetPos;
        cam.orthographicSize = targetZoomSize;
    }

    IEnumerator ZoomOutRoutine()
    {
        IsZoomed = false;

        Vector3 startPos = cam.transform.position;
        float startSize = cam.orthographicSize;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / zoomTime;
            cam.transform.position = Vector3.Lerp(startPos, defaultPos, t);
            cam.orthographicSize = Mathf.Lerp(startSize, defaultSize, t);
            yield return null;
        }

        cam.transform.position = defaultPos;
        cam.orthographicSize = defaultSize;

        ZoomTarget = null;
    }
}