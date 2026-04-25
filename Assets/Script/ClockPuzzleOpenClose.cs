using UnityEngine;

public class ClockPuzzleOpenClose : MonoBehaviour
{
    [SerializeField] GameObject puzzleUI;
    [SerializeField] CameraZoomController zoom;
    [SerializeField] Transform myZoomPoint;

    void Awake()
    {
        if (zoom == null)
            zoom = FindFirstObjectByType<CameraZoomController>();

        if (puzzleUI != null)
            puzzleUI.SetActive(false);
    }

    public void OpenPuzzle()
    {
        if (ClockState.IsSolved)
            return;

        if (puzzleUI != null)
            puzzleUI.SetActive(true);
    }

    public void ClosePuzzle()
    {
        if (puzzleUI != null)
            puzzleUI.SetActive(false);

        if (zoom != null)
            zoom.ZoomOut();
    }
}