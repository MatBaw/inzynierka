using UnityEngine;

public class ClickOutsideToZoomOut : MonoBehaviour
{
    private CameraZoomController zoomController;
    private Camera cam;

    // ten klik ma być zignorowany (klik, który zrobił zoom)
    private bool ignoreNextClick = false;

    void Start()
    {
        zoomController = FindFirstObjectByType<CameraZoomController>();
        cam = Camera.main;
    }

    // wołane z ZoomSzafka.ZoomIn()
    public void IgnoreNextClick()
    {
        ignoreNextClick = true;
    }

    void Update()
    {
        if (zoomController == null) return;

        if (!zoomController.IsZoomed)
        {
            // jak nie ma zooma, to i tak nie ma czego ignorować
            ignoreNextClick = false;
            return;
        }

        // jeśli mamy klik do zignorowania – „zjadamy” pierwszy click i wychodzimy
        if (ignoreNextClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // pierwszy klik po zoomie – tylko kasujemy flagę
                ignoreNextClick = false;
            }
            return;
        }

        // od teraz każdy klik LPM może wywołać odzoomowanie
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld3D = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseWorld2D = mouseWorld3D;

            bool clickedOnTarget = false;

            if (zoomController.ZoomTarget != null)
            {
                // 1) najpierw po colliderach (szuflady, drzwiczki)
                RaycastHit2D hit = Physics2D.Raycast(mouseWorld2D, Vector2.zero);

                if (hit.collider != null)
                {
                    Transform t = hit.collider.transform;

                    if (t == zoomController.ZoomTarget || t.IsChildOf(zoomController.ZoomTarget))
                    {
                        clickedOnTarget = true;
                    }
                }

                // 2) jeśli colliderów nie ma, sprawdzamy, czy klik jest w boundsach sprite'ów szafki
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
        if (zoomController == null) return;
        if (!zoomController.IsZoomed) return;

        if (zoomController.ZoomTarget != null)
        {
            var zoomCabinet = zoomController.ZoomTarget.GetComponent<ZoomSzafka>();
            if (zoomCabinet != null)
                zoomCabinet.ResetSprite();
        }

        zoomController.ZoomOut();
        ignoreNextClick = false;
    }
}
