using UnityEngine;

public class PaperZoomOpenClose : MonoBehaviour
{
    [Header("Referencje")]
    [SerializeField] private ZoomTarget zoomTarget;
    [SerializeField] private CameraZoomController zoomController;
    [SerializeField] private ClickOutsideToZoomOut clickOutsideToZoomOut;
    [SerializeField] private GameObject revealHitbox;
    [SerializeField] private GameObject closeZoomHitbox;

    [Header("Wyłączane po zoomie")]
    [SerializeField] private Collider2D rootCollider;
    [SerializeField] private GazeDwellTarget rootGazeTarget;

    [Header("Po zoomie")]
    [SerializeField] private float gazeBlockDurationOnZoom = 1.0f;

    public bool IsZoomedOnPaper { get; private set; }

    private void Awake()
    {
        if (zoomTarget == null)
            zoomTarget = GetComponent<ZoomTarget>();

        if (zoomController == null)
            zoomController = FindFirstObjectByType<CameraZoomController>();

        if (clickOutsideToZoomOut == null)
            clickOutsideToZoomOut = FindFirstObjectByType<ClickOutsideToZoomOut>();

        if (rootCollider == null)
            rootCollider = GetComponent<Collider2D>();

        if (rootGazeTarget == null)
            rootGazeTarget = GetComponent<GazeDwellTarget>();

        if (revealHitbox != null)
            revealHitbox.SetActive(false);

        if (closeZoomHitbox != null)
            closeZoomHitbox.SetActive(false);

        IsZoomedOnPaper = false;
    }

    public void TryOpenZoomByGaze()
    {
        if (IsZoomedOnPaper)
            return;

        if (zoomTarget != null)
        {
            zoomTarget.ZoomNow();
        }
        else
        {
            Debug.LogWarning("[PaperZoom] Brak ZoomTarget.");
        }
    }

    public void OpenZoom()
    {
        if (IsZoomedOnPaper)
            return;

        if (clickOutsideToZoomOut != null)
            clickOutsideToZoomOut.IgnoreNextClick();

        if (GazeDwellClick2D.Instance != null)
            GazeDwellClick2D.Instance.TriggerCooldown(gazeBlockDurationOnZoom);

        if (revealHitbox != null)
            revealHitbox.SetActive(true);

        if (closeZoomHitbox != null)
            closeZoomHitbox.SetActive(true);

        if (rootCollider != null)
            rootCollider.enabled = false;

        if (rootGazeTarget != null)
            rootGazeTarget.enabled = false;

        IsZoomedOnPaper = true;
        Debug.Log("[PaperZoom] OpenZoom -> IsZoomedOnPaper = true | obiekt=" + gameObject.name + " | id=" + GetInstanceID());
    }

    public void CloseZoom()
    {
        if (revealHitbox != null)
            revealHitbox.SetActive(false);

        if (closeZoomHitbox != null)
            closeZoomHitbox.SetActive(false);

        if (rootCollider != null)
            rootCollider.enabled = true;

        if (rootGazeTarget != null)
            rootGazeTarget.enabled = true;

        if (zoomController != null)
            zoomController.ZoomOut();

        IsZoomedOnPaper = false;
        Debug.Log("[PaperZoom] CloseZoom -> IsZoomedOnPaper = false | obiekt=" + gameObject.name + " | id=" + GetInstanceID());
    }
}