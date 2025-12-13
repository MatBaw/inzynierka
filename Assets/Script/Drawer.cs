using UnityEngine;

public class Drawer : MonoBehaviour
{
    public SpriteRenderer openSpriteRenderer;  // dziecko z otwartą szufladą

    [HideInInspector]
    public bool IsOpen { get; private set; }

    DrawerManager manager;
    CameraZoomController zoomController;

    void Start()
    {
        manager = GetComponentInParent<DrawerManager>();
        zoomController = FindObjectOfType<CameraZoomController>();
        SetOpen(false);  // na start zamknięta
    }

    public void SetOpen(bool open)
    {
        IsOpen = open;
        if (openSpriteRenderer != null)
            openSpriteRenderer.enabled = open;
    }

    void OnMouseDown()
    {
        // Reagujemy tylko, gdy jesteśmy w zbliżeniu na szafkę
        if (zoomController != null && !zoomController.IsZoomed)
            return;

        if (manager != null)
        {
            manager.ToggleDrawer(this);
        }
        else
        {
            // awaryjnie: brak managera = zwykłe przełączanie
            SetOpen(!IsOpen);
        }
    }
}
