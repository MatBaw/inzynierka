using UnityEngine;

public class DrawerClick : MonoBehaviour
{
    public SpriteRenderer openSpriteRenderer;

    bool isOpen = false;

    void Start()
    {
        // jeśli nie ustawisz ręcznie, sam szuka dziecka z SpriteRendererem
        if (openSpriteRenderer == null)
        {
            openSpriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        }

        SetOpen(false);
    }

    void OnMouseDown()
    {
        // NA RAZIE bez sprawdzania zoomu – tylko test
        SetOpen(!isOpen);
    }

    public void SetOpen(bool open)
    {
        isOpen = open;

        if (openSpriteRenderer != null)
        {
            openSpriteRenderer.enabled = open;
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
