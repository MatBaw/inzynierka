using UnityEngine;

public class Drawer : MonoBehaviour
{
    public SpriteRenderer openSprite;   // szuflada1_0
    public DrawerManager manager;       // obiekt Drawers

    private bool isOpen = false;
    public bool IsOpen => isOpen;

    void Start()
    {
        if (openSprite != null)
            openSprite.enabled = false;   // na start niewidoczna
    }

    void OnMouseDown()
    {
        if (manager != null)
        {
            manager.ToggleDrawer(this);   // powiedz managerowi "kliknięto mnie"
        }
        else
        {
            SetOpen(!isOpen);             // awaryjnie: przełącz sama
        }
    }

    public void SetOpen(bool open)
    {
        isOpen = open;

        if (openSprite != null)
            openSprite.enabled = open;
    }
}
