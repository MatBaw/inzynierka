/*using System.Collections;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public Camera cam;              // przypisz Main Camera
    public float zoomSize = 3f;     // jak bardzo przybliżyć
    public float zoomTime = 0.7f;   // czas najazdu
    public Vector3 offset = new Vector3(0f, 0f, -10f); // z = -10 dla 2D

    float defaultSize;
    Vector3 defaultPos;
    bool isZoomed = false;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
        defaultSize = cam.orthographicSize;
        defaultPos  = cam.transform.position;
    }

    public void ZoomTo(Transform target)
    {
        StopAllCoroutines();
        StartCoroutine(ZoomRoutine(target));
    }

    public void ZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomOutRoutine());
    }

    IEnumerator ZoomRoutine(Transform target)
    {
        isZoomed = true;
        Vector3 startPos = cam.transform.position;
        float startSize = cam.orthographicSize;

        Vector3 targetPos = target.position + offset;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / zoomTime;
            cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            cam.orthographicSize   = Mathf.Lerp(startSize, zoomSize, t);
            yield return null;
        }
    }

    IEnumerator ZoomOutRoutine()
    {
        isZoomed = false;
        Vector3 startPos = cam.transform.position;
        float startSize = cam.orthographicSize;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / zoomTime;
            cam.transform.position = Vector3.Lerp(startPos, defaultPos, t);
            cam.orthographicSize   = Mathf.Lerp(startSize, defaultSize, t);
            yield return null;
        }
    }
}*/
using System.Collections;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public Camera cam;
    public float zoomSize = 3f;
    public float zoomTime = 0.7f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    public bool IsZoomed { get; private set; }     // <-- publiczny stan
    public Transform ZoomTarget { get; private set; } // <-- na co jest zoom

    float defaultSize;
    Vector3 defaultPos;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
        defaultSize = cam.orthographicSize;
        defaultPos  = cam.transform.position;
    }

    public void ZoomTo(Transform target)
    {
        ZoomTarget = target;
        StopAllCoroutines();
        StartCoroutine(ZoomRoutine(target));
    }

    public void ZoomOut()
    {
        StopAllCoroutines();
        StartCoroutine(ZoomOutRoutine());
    }

    IEnumerator ZoomRoutine(Transform target)
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
            cam.orthographicSize   = Mathf.Lerp(startSize, zoomSize, t);
            yield return null;
        }
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
            cam.orthographicSize   = Mathf.Lerp(startSize, defaultSize, t);
            yield return null;
        }

        ZoomTarget = null;
    }
}

