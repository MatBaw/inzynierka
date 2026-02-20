using UnityEngine;

public class ZoomExitOnRightClick : MonoBehaviour
{
    private CameraZoomController zoomController;

    void Start()
    {
        zoomController = FindFirstObjectByType<CameraZoomController>();
    }

    void Update()
    {
        if (zoomController == null) return;
        if (!zoomController.IsZoomed) return;

        // PPM = wyjście z zooma
        if (Input.GetMouseButtonDown(1))
        {
            if (zoomController.ZoomTarget != null)
            {
                var szafka = zoomController.ZoomTarget.GetComponent<ZoomSzafka>();
                if (szafka != null)
                    szafka.ResetSprite();
            }

            zoomController.ZoomOut();
        }
    }
}
