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

        // chowamy jak najwcześniej, zanim cokolwiek zdąży mignąć
        if (puzzleUI != null)
            puzzleUI.SetActive(false);
    }

    public void OpenPuzzle()
    {
        // dodatkowe zabezpieczenie: jeśli już rozwiązane, nie pokazuj UI nigdy
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

/*using UnityEngine;

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
}*/