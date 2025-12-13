using UnityEngine;

public class DrawerManager : MonoBehaviour
{
    public Drawer[] drawers;

    public void ToggleDrawer(Drawer drawer)
    {
        if (drawer.IsOpen)
        {
            // kliknęliśmy otwartą -> zamknij ją
            drawer.SetOpen(false);
            return;
        }

        // otwieramy tę, a resztę zamykamy
        foreach (var d in drawers)
        {
            d.SetOpen(d == drawer);
        }
    }

    public bool AnyOpen()
    {
        foreach (var d in drawers)
        {
            if (d.IsOpen) return true;
        }
        return false;
    }
}
