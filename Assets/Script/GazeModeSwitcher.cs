using UnityEngine;

public class GazeModeSwitcher : MonoBehaviour
{
    [SerializeField] GazeDwellClick2D gazeWorld2D;
    [SerializeField] GazeDwellClickUI gazeUI;

    void OnEnable()
    {
        if (gazeWorld2D != null) gazeWorld2D.enabled = false;
        if (gazeUI != null) gazeUI.enabled = true;
    }

    void OnDisable()
    {
        if (gazeWorld2D != null) gazeWorld2D.enabled = true;
        if (gazeUI != null) gazeUI.enabled = false;
    }
}