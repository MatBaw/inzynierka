using UnityEngine;

public class EyeGazeDoorArea : MonoBehaviour
{
    public SpriteRenderer openDoorSprite;

    public BoxCollider2D closedCollider;
    public BoxCollider2D openCollider;

    bool isOpen = false;

    void Start()
    {
        SetOpen(false);
    }

    public void ToggleDoor()
    {
        SetOpen(!isOpen);
    }

    public void SetOpen(bool open)
    {
        isOpen = open;

        if (openDoorSprite != null)
            openDoorSprite.enabled = open;

        if (closedCollider != null)
            closedCollider.enabled = !open;

        if (openCollider != null)
            openCollider.enabled = open;

        Debug.Log($"EyeGazeDoorArea: {name} open = {open}");
    }

    public bool IsOpen => isOpen;
}
