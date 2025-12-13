using UnityEngine;
using UnityEngine.Events;

public class GazeDwellTarget : MonoBehaviour
{
    [Tooltip("Co ma się stać, gdy gracz popatrzy 3s na ten obiekt")]
    public UnityEvent OnDwellClick;

    // opcjonalnie: do podświetlania
    public UnityEvent OnGazeEnter;
    public UnityEvent OnGazeExit;
}
