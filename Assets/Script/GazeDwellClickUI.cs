using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GazeDwellClickUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] RectTransform gazeDot;          // Canvas/GazeDot
    [SerializeField] GraphicRaycaster raycaster;     // GraphicRaycaster z CANVAS
    [SerializeField] EventSystem eventSystem;        // EventSystem ze sceny

    [Header("Dwell")]
    [SerializeField] float dwellSeconds = 3f;
    [SerializeField] bool fireOnlyOnceUntilLookAway = true;

    [Header("Debug")]
    [SerializeField] bool debugLogHits = false;

    private Button currentButton;
    private float timer;

    void Awake()
    {
        if (eventSystem == null) eventSystem = EventSystem.current;

        // jeśli nie podpięto ręcznie, spróbuj znaleźć Canvas i jego GraphicRaycaster
        if (raycaster == null)
        {
            var c = GetComponentInParent<Canvas>();
            if (c != null) raycaster = c.GetComponent<GraphicRaycaster>();
        }
    }

    void Update()
    {
        if (gazeDot == null || raycaster == null || eventSystem == null) return;

        // Canvas, na którym działa raycaster
        var canvas = raycaster.GetComponent<Canvas>();

        // WAŻNE: dla WorldSpace / ScreenSpaceCamera musisz użyć worldCamera z Canvas
        Camera uiCam = null;
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            uiCam = canvas.worldCamera;

        // pozycja spojrzenia w screen-space
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCam, gazeDot.position);

        // UI raycast
        var pointer = new PointerEventData(eventSystem) { position = screenPos };
        var results = new List<RaycastResult>();
        raycaster.Raycast(pointer, results);

        if (debugLogHits)
        {
            Debug.Log(results.Count > 0 ? $"UI hit: {results[0].gameObject.name}" : "UI hit: NONE");
        }

        Button next = null;

        // znajdź pierwszy obiekt z Buttonem (albo jego parent)
        for (int i = 0; i < results.Count; i++)
        {
            var go = results[i].gameObject;
            next = go.GetComponent<Button>() ?? go.GetComponentInParent<Button>();
            if (next != null) break;
        }

        // zmiana celu -> reset timera
        if (next != currentButton)
        {
            currentButton = next;
            timer = 0f;
        }

        if (currentButton == null) return;

        timer += Time.deltaTime;

        if (timer >= dwellSeconds)
        {
            currentButton.onClick.Invoke();

            if (fireOnlyOnceUntilLookAway)
                currentButton = null;

            timer = 0f;
        }
    }
}
