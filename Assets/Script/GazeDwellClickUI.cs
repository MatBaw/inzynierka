using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GazeDwellClickUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] RectTransform gazeDot;          // Canvas/GazeDot
    [SerializeField] GraphicRaycaster raycaster;     // z Canvas
    [SerializeField] EventSystem eventSystem;        // EventSystem ze sceny

    [Header("Dwell")]
    [SerializeField] float dwellSeconds = 3f;
    [SerializeField] bool fireOnlyOnceUntilLookAway = true;

    private Button currentButton;
    private float timer;

    void Awake()
    {
        if (eventSystem == null) eventSystem = EventSystem.current;
        if (raycaster == null)
        {
            var canvas = GetComponentInParent<Canvas>();
            if (canvas != null) raycaster = canvas.GetComponent<GraphicRaycaster>();
        }
    }

    void Update()
    {
        if (gazeDot == null || raycaster == null || eventSystem == null) return;

        // pozycja spojrzenia w screen-space
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, gazeDot.position);

        // UI raycast
        var pointer = new PointerEventData(eventSystem) { position = screenPos };
        var results = new List<RaycastResult>();
        raycaster.Raycast(pointer, results);

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
            // kliknięcie UI
            currentButton.onClick.Invoke();

            if (fireOnlyOnceUntilLookAway)
            {
                currentButton = null; // wymuś zejście wzroku i powrót
            }

            timer = 0f;
        }
    }
}
