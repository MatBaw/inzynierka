using UnityEngine;

public class ClockPuzzleOpenClose : MonoBehaviour
{
    [SerializeField] GameObject puzzleUI;              // ClockPuzzleUI panel
    [SerializeField] CameraZoomController zoom;        // camera zoom controller
    [SerializeField] Transform myZoomPoint;            // zostaw puste, jeśli używasz tego z ZoomTarget

    void Start()
    {
        if (zoom == null) zoom = FindFirstObjectByType<CameraZoomController>();
        if (puzzleUI != null) puzzleUI.SetActive(false);
    }

    public void OpenPuzzle()
    {
        if (puzzleUI != null) puzzleUI.SetActive(true);
    }

    public void ClosePuzzle()
    {
        if (puzzleUI != null) puzzleUI.SetActive(false);
        if (zoom != null) zoom.ZoomOut();
    }
}