using System.Collections;
using UnityEngine;

public class DrawerOpenClose : MonoBehaviour
{
    [Header("Ruch szuflady")]
    [SerializeField] private Vector3 openOffset = new Vector3(0f, -0.25f, 0f);
    [SerializeField] private float moveDuration = 0.2f;

    private static DrawerOpenClose currentlyOpenDrawer;

    private Vector3 closedPos;
    private Vector3 openPos;

    private bool isOpen = false;
    private bool isMoving = false;
    private Coroutine moveRoutine;

    private void Awake()
    {
        closedPos = transform.localPosition;
        openPos = closedPos + openOffset;
    }

    public void ToggleDrawer()
    {
        if (isMoving) return;

        // Jeśli klikam otwartą szufladę -> zamknij ją
        if (isOpen)
        {
            CloseDrawer();
            currentlyOpenDrawer = null;
            return;
        }

        // Jeśli inna szuflada jest otwarta -> zamknij tamtą
        if (currentlyOpenDrawer != null && currentlyOpenDrawer != this)
        {
            currentlyOpenDrawer.CloseDrawer();
        }

        OpenDrawer();
        currentlyOpenDrawer = this;
    }

    public void OpenDrawer()
    {
        if (isMoving || isOpen) return;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveDrawer(openPos));
        isOpen = true;
    }

    public void CloseDrawer()
    {
        if (isMoving || !isOpen) return;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveDrawer(closedPos));
        isOpen = false;
    }

    private IEnumerator MoveDrawer(Vector3 targetPos)
    {
        isMoving = true;

        Vector3 startPos = transform.localPosition;
        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / moveDuration);
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.localPosition = targetPos;
        isMoving = false;
    }

    private void OnMouseDown()
    {
        ToggleDrawer();
    }
}