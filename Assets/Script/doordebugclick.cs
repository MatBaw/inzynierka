using UnityEngine;

public class DoorDebugClick : MonoBehaviour
{
    private EyeGazeDoorArea door;

    void Start()
    {
        door = GetComponent<EyeGazeDoorArea>();
    }

    void OnMouseDown()
    {
        if (door != null)
        {
            Debug.Log("DoorDebugClick: ToggleDoor()");
            door.ToggleDoor();
        }
    }
}
