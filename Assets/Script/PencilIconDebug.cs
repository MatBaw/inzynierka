using UnityEngine;
using UnityEngine.UI;

public class PencilIconDebug : MonoBehaviour
{
    [SerializeField] private GameObject pencilIcon;

    [ContextMenu("Debug Pencil Icon")]
    public void DebugPencilIcon()
    {
        if (pencilIcon == null)
        {
            Debug.LogWarning("PencilIconDebug: pencilIcon nie jest podpięty.");
            return;
        }

        Debug.Log("=== PENCIL ICON DEBUG ===");
        Debug.Log("Name: " + pencilIcon.name);
        Debug.Log("activeSelf: " + pencilIcon.activeSelf);
        Debug.Log("activeInHierarchy: " + pencilIcon.activeInHierarchy);

        RectTransform rt = pencilIcon.GetComponent<RectTransform>();
        if (rt != null)
        {
            Debug.Log("anchoredPosition: " + rt.anchoredPosition);
            Debug.Log("sizeDelta: " + rt.sizeDelta);
            Debug.Log("localScale: " + rt.localScale);
            Debug.Log("anchorMin: " + rt.anchorMin);
            Debug.Log("anchorMax: " + rt.anchorMax);
            Debug.Log("pivot: " + rt.pivot);
        }
        else
        {
            Debug.LogWarning("Brak RectTransform na PencilIcon.");
        }

        Image img = pencilIcon.GetComponent<Image>();
        if (img != null)
        {
            Debug.Log("Image enabled: " + img.enabled);
            Debug.Log("Color: " + img.color);
            Debug.Log("Alpha: " + img.color.a);
            Debug.Log("Sprite: " + (img.sprite != null ? img.sprite.name : "NULL"));
            Debug.Log("Type: " + img.type);
            Debug.Log("Maskable: " + img.maskable);
            Debug.Log("Raycast Target: " + img.raycastTarget);
        }
        else
        {
            Debug.LogWarning("Brak Image na PencilIcon.");
        }

        Mask mask = pencilIcon.GetComponentInParent<Mask>();
        RectMask2D rectMask = pencilIcon.GetComponentInParent<RectMask2D>();

        Debug.Log("Parent Mask: " + (mask != null ? mask.name : "none"));
        Debug.Log("Parent RectMask2D: " + (rectMask != null ? rectMask.name : "none"));

        Canvas parentCanvas = pencilIcon.GetComponentInParent<Canvas>();
        if (parentCanvas != null)
            Debug.Log("Parent Canvas: " + parentCanvas.name);
        else
            Debug.LogWarning("Brak Canvas w rodzicach.");

        Debug.Log("=========================");
    }
}
