using UnityEngine;

public class MouseRevealTrigger : MonoBehaviour
{
    [SerializeField] private RevealPaperWithPencil reveal;

    private void OnMouseDown()
    {
        if (reveal != null)
        {
            Debug.Log("MYSZKA -> TryReveal()");
            reveal.TryReveal();
        }
    }
}