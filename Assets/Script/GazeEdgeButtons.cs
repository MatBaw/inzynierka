using UnityEngine;
using UnityEngine.UI;

public class GazeEdgeButtons : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] RectTransform gazeDot;      // Canvas/GazeDot
    [SerializeField] Button leftButton;          // ButtonLeft
    [SerializeField] Button rightButton;         // ButtonRight
    [SerializeField] CameraZoomController zoomController; // (opcjonalnie) blokada podczas zooma

    [Header("Edge zones")]
    [Range(0.03f, 0.20f)]
    [SerializeField] float edgeWidth01 = 0.08f;  // 8% szerokości ekranu (bezpieczne)
    [SerializeField] float dwellSeconds = 0.6f;  // ile patrzeć w strefę
    [SerializeField] float cooldownSeconds = 0.8f; // pauza po przełączeniu

    private float dwellTimer;
    private float cooldownTimer;
    private int currentZone; // -1 lewa, +1 prawa, 0 żadna

    void Awake()
    {
        if (zoomController == null) zoomController = FindObjectOfType<CameraZoomController>();
    }

    void Update()
    {
        if (gazeDot == null || leftButton == null || rightButton == null) return;

        // Bezpiecznik: nie zmieniaj ścian, gdy kamera jest w zoomie
        if (zoomController != null && zoomController.IsZoomed) return;

        // cooldown
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        // screen pos z kropki
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(null, gazeDot.position);
        float x01 = screen.x / Mathf.Max(Screen.width, 1);

        int zone = 0;
        if (x01 <= edgeWidth01) zone = -1;
        else if (x01 >= 1f - edgeWidth01) zone = 1;

        // zmiana strefy = reset timera
        if (zone != currentZone)
        {
            currentZone = zone;
            dwellTimer = 0f;
        }

        if (currentZone == 0) return;

        dwellTimer += Time.deltaTime;

        if (dwellTimer >= dwellSeconds)
        {
            dwellTimer = 0f;
            cooldownTimer = cooldownSeconds;

            if (currentZone == -1) leftButton.onClick.Invoke();
            else rightButton.onClick.Invoke();
        }
    }
}
