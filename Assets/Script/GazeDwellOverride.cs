using UnityEngine;

public class GazeDwellOverride : MonoBehaviour
{
    [Min(0.05f)]
    public float dwellSeconds = 0.4f;
}