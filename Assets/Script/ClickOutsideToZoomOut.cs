using UnityEngine;

public class ClickOutsideToZoomOut : MonoBehaviour
{
    CameraZoomController zoomController;
    Camera cam;

    void Start()
    {
        zoomController = FindObjectOfType<CameraZoomController>();
        cam = Camera.main;
    }

    void Update()
    {
        if (zoomController == null) return;
        if (!zoomController.IsZoomed) return;

        // lewy przycisk myszy
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseWorld2D = mouseWorld;

            RaycastHit2D hit = Physics2D.Raycast(mouseWorld2D, Vector2.zero);

            bool clickedOnTarget = false;

            if (hit.collider != null && zoomController.ZoomTarget != null)
            {
                Transform t = hit.collider.transform;

                // klik na obiekt powiększony albo któryś z jego dzieci (np. szuflada)
                if (t == zoomController.ZoomTarget || t.IsChildOf(zoomController.ZoomTarget))
                {
                    clickedOnTarget = true;
                }
            }

            // jeśli klik NIE był na szafce ani jej dzieciach -> wyjdź z zooma
            if (!clickedOnTarget)
            {
                ZoomOutNow();
            }
        }
    }

    // NOWE: do wywołania z UnityEvent / wzroku
    public void ZoomOutNow()
    {
        if (zoomController == null) return;
        if (!zoomController.IsZoomed) return;

        if (zoomController.ZoomTarget != null)
        {
            var zoomCabinet = zoomController.ZoomTarget.GetComponent<ZoomCabinet>();
            if (zoomCabinet != null)
                zoomCabinet.ResetSprite();
        }

        zoomController.ZoomOut();
    }
}
