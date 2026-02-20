using UnityEngine;
using UnityEngine.UI;

public class GazeCursorRingUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Image ringBg;    // RingBG (Image)
    [SerializeField] private Image ringFill;  // RingFill (Image)

    [Header("Colors")]
    [SerializeField] private Color idleColor = new Color(1f, 1f, 1f, 0.6f);
    [SerializeField] private Color hoverBgColor = new Color(1f, 0f, 0f, 0.65f);
    [SerializeField] private Color fillColor = new Color(0f, 1f, 0f, 0.85f);

    void Awake()
    {
        SetIdle();
    }

    public void SetIdle()
    {
        if (ringBg) ringBg.color = idleColor;

        if (ringFill)
        {
            ringFill.color = fillColor;
            ringFill.fillAmount = 0f;
            ringFill.enabled = false;
        }
    }

    public void SetHoverProgress(float progress01)
    {
        if (ringBg) ringBg.color = hoverBgColor;

        if (ringFill)
        {
            ringFill.enabled = true;
            ringFill.color = fillColor;
            ringFill.fillAmount = Mathf.Clamp01(progress01);
        }
    }

    void Update()
{
    if (Input.GetKeyDown(KeyCode.T)) SetHoverProgress(0.25f);
    if (Input.GetKeyDown(KeyCode.Y)) SetHoverProgress(0.75f);
    if (Input.GetKeyDown(KeyCode.U)) SetIdle();
}

}
