using UnityEngine;

public class PuzzleUIOverlay : MonoBehaviour
{
    [SerializeField] GameObject inventoryBar;   // przeciągnij tu InventoryBar
    [SerializeField] GameObject[] hideExtraUI;   // opcjonalnie: inne rzeczy do ukrycia (strzałki, gaze dot itp.)

    void OnEnable()
    {
        if (inventoryBar != null) inventoryBar.SetActive(false);
        if (hideExtraUI != null)
            foreach (var go in hideExtraUI)
                if (go != null) go.SetActive(false);
    }

    void OnDisable()
    {
        if (inventoryBar != null) inventoryBar.SetActive(true);
        if (hideExtraUI != null)
            foreach (var go in hideExtraUI)
                if (go != null) go.SetActive(true);
    }
}