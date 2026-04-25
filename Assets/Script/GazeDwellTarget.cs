using UnityEngine;
using UnityEngine.Events;

public class GazeDwellTarget : MonoBehaviour
{
    [Tooltip("Co ma się stać, gdy gracz popatrzy 3s na ten obiekt")]
    public UnityEvent OnDwellClick;

    public UnityEvent OnGazeEnter;
    public UnityEvent OnGazeExit;
}
