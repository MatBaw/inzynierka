using UnityEngine;

public class GazeDwellClick2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RectTransform gazeDot;   // Canvas/GazeDot
    [SerializeField] Camera worldCam;         // Main Camera

    [Header("Dwell - select target")]
    [SerializeField] float dwellSeconds = 3f;

    [Header("Dwell - look outside to zoom out")]
    [SerializeField] bool enableLookOutsideZoomOut = true;
    [SerializeField] float outsideSeconds = 1.0f; // po ilu sekundach patrzenia w puste ma odzoomować
    [SerializeField] MonoBehaviour zoomOutBehaviour; // przeciągnij tu obiekt ze skryptem ClickOutsideToZoomOut
    [SerializeField] string zoomOutMethodName = "ZoomOut"; // jeśli masz inną nazwę metody, zmień

    private GazeDwellTarget current;
    private float timer;

    private float outsideTimer;

    void Awake()
    {
        if (worldCam == null) worldCam = Camera.main;
    }

    void Update()
    {
        if (gazeDot == null || worldCam == null) return;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, gazeDot.position);

        Vector3 worldPos = worldCam.ScreenToWorldPoint(new Vector3(
            screenPos.x, screenPos.y, -worldCam.transform.position.z));

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        var next = hit.collider ? hit.collider.GetComponent<GazeDwellTarget>() : null;

        // ---- OUTSIDE LOOK LOGIC ----
        if (enableLookOutsideZoomOut)
        {
            bool lookingAtNothing = (hit.collider == null);

            if (lookingAtNothing)
            {
                outsideTimer += Time.deltaTime;

                if (outsideTimer >= outsideSeconds)
                {
                    outsideTimer = 0f;
                    TryZoomOut();
                }
            }
            else
            {
                outsideTimer = 0f;
            }
        }

        // ---- DWELL SELECT LOGIC ----
        if (next != current)
        {
            if (current != null) current.OnGazeExit?.Invoke();
            current = next;
            timer = 0f;
            if (current != null) current.OnGazeEnter?.Invoke();
        }

        if (current == null) return;

        timer += Time.deltaTime;

        if (timer >= dwellSeconds)
        {
            current.OnDwellClick?.Invoke();
            current = null;   // żeby nie klikało w pętli
            timer = 0f;
        }
    }

    void TryZoomOut()
    {
        if (zoomOutBehaviour == null) return;

        // wywołaj publiczną metodę bez parametrów
        zoomOutBehaviour.Invoke(zoomOutMethodName, 0f);
    }
}
