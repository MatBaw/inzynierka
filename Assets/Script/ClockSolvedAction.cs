using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockSolvedAction : MonoBehaviour
{
    [Header("Where to go after solving (from present -> past)")]
    [SerializeField] string goToPastScene = "pastroom1";

    [Header("Where to go back (from past -> present)")]
    [SerializeField] string goToPresentScene = "presentroom2";

    [Header("Puzzle open (only used in present before solved)")]
    [SerializeField] ClockPuzzleOpenClose puzzleOpenClose; // podepnij, jeśli w present ma się otwierać UI

    public void Interact()
    {
        string current = SceneManager.GetActiveScene().name;

        // Jesteśmy w przeszłości? -> wróć do teraźniejszości (presentroom2)
        if (current.StartsWith("pastroom"))
        {
            SceneManager.LoadScene(goToPresentScene);
            return;
        }

        // Jesteśmy w teraźniejszości:
        // jeśli puzzle już rozwiązane -> idź do pastroom1
        if (ClockState.IsSolved)
        {
            SceneManager.LoadScene(goToPastScene);
            return;
        }

        // jeśli nie rozwiązane -> otwórz puzzle
        if (puzzleOpenClose != null)
        {
            puzzleOpenClose.OpenPuzzle();
        }
    }

    // mysz
    void OnMouseDown()
    {
        Interact();
    }
}