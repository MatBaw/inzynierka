using UnityEngine;

public class DrawerManager : MonoBehaviour
{
    private Drawer[] drawers;

    void Awake()
    {
        drawers = GetComponentsInChildren<Drawer>(true);
    }

    public void ToggleDrawer(Drawer clicked)
    {
        bool shouldOpenClicked = !clicked.IsOpen;

        foreach (var d in drawers)
        {
            d.SetOpen(shouldOpenClicked && d == clicked);
        }
    }
}
