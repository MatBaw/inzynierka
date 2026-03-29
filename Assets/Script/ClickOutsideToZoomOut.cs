using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOutsideToZoomOut : MonoBehaviour
{
    private CameraZoomController zoomController;
    private Camera cam;

    [Header("Block zoom-out while this UI is open (optional)")]
    [SerializeField] private GameObject blockZoomOutWhenActive;

    [Header("Ignore clicks right after zoom-in")]
    [SerializeField] private float ignoreClicksAfterZoomSeconds = 0.25f;

    private float ignoreClicksUntil = 0f;

    private void Start()
    {
        zoomController = FindFirstObjectByType<CameraZoomController>();
        cam = Camera.main;
    }

    // Wywołaj to zaraz po wejściu w zoom
    public void IgnoreNextClick()
    {
        ignoreClicksUntil = Time.time + ignoreClicksAfterZoomSeconds;
    }

    // Opcjonalnie: ręczne ustawienie czasu blokady
    public void IgnoreClicksFor(float seconds)
    {
        ignoreClicksUntil = Time.time + seconds;
    }

    private void Update()
    {
        if (zoomController == null || cam == null)
            return;

        if (blockZoomOutWhenActive != null && blockZoomOutWhenActive.activeInHierarchy)
            return;

        if (!zoomController.IsZoomed)
            return;

        // Ignoruj klik zaraz po wejściu w zoom
        if (Time.time < ignoreClicksUntil)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld3D = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseWorld2D = mouseWorld3D;

            bool clickedOnTarget = false;

            if (zoomController.ZoomTarget != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(mouseWorld2D, Vector2.zero);

                if (hit.collider != null)
                {
                    Transform t = hit.collider.transform;
                    if (t == zoomController.ZoomTarget || t.IsChildOf(zoomController.ZoomTarget))
                    {
                        clickedOnTarget = true;
                    }
                }

                if (!clickedOnTarget)
                {
                    SpriteRenderer[] renderers =
                        zoomController.ZoomTarget.GetComponentsInChildren<SpriteRenderer>();

                    foreach (var sr in renderers)
                    {
                        if (!sr.enabled) continue;

                        if (sr.bounds.Contains(mouseWorld3D))
                        {
                            clickedOnTarget = true;
                            break;
                        }
                    }
                }
            }

            if (!clickedOnTarget)
            {
                ZoomOutNow();
            }
        }
    }

    public void ZoomOutNow()
    {
        if (zoomController == null || !zoomController.IsZoomed)
            return;

        if (Time.time < ignoreClicksUntil)
            return;

        if (blockZoomOutWhenActive != null && blockZoomOutWhenActive.activeInHierarchy)
            return;

        if (zoomController.ZoomTarget != null)
        {
            var zoomCabinet = zoomController.ZoomTarget.GetComponent<ZoomSzafka>();
            if (zoomCabinet != null)
                zoomCabinet.ResetSprite();
        }

        zoomController.ZoomOut();
    }
}