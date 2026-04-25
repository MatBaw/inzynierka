using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockSolvedAction : MonoBehaviour
{
    [Header("Where to go after solving (from present -> past)")]
    [SerializeField] string goToPastScene = "pastroom1";

    [Header("Where to go back (from past -> present)")]
    [SerializeField] string goToPresentScene = "presentroom2";

    [Header("Puzzle open (only used in present before solved)")]
    [SerializeField] ClockPuzzleOpenClose puzzleOpenClose;

    public void Interact()
    {
        string current = SceneManager.GetActiveScene().name;

        if (current.StartsWith("pastroom"))
        {
            if (SceneFadeTransition.Instance != null)
                SceneFadeTransition.Instance.FadeToScene(goToPresentScene);
            else
                SceneManager.LoadScene(goToPresentScene);

            return;
        }

        if (ClockState.IsSolved)
        {
            InventoryState.SetVisitedPastOnce(true);

            if (SceneFadeTransition.Instance != null)
                SceneFadeTransition.Instance.FadeToScene(goToPastScene);
            else
                SceneManager.LoadScene(goToPastScene);

            return;
        }

        if (puzzleOpenClose != null)
            puzzleOpenClose.OpenPuzzle();
    }

    void OnMouseDown()
    {
        Interact();
    }
}