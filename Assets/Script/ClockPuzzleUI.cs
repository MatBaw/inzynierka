using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClockPuzzleUI : MonoBehaviour
{
    [Header("TMP Texts (4 digits)")]
    [SerializeField] TMP_Text h1Text;
    [SerializeField] TMP_Text h2Text;
    [SerializeField] TMP_Text m1Text;
    [SerializeField] TMP_Text m2Text;

    [Header("Starting time (used only if player never set the clock yet)")]
    [SerializeField] int startHour = 11;
    [SerializeField] int startMinute = 0;

    [Header("Solution")]
    [SerializeField] bool useSolutionCheck = true;
    [SerializeField] int solutionHour = 14;
    [SerializeField] int solutionMinute = 25;

    [Header("Success feedback")]
    [SerializeField] float successSeconds = 2f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip successClip;

    [Tooltip("Elementy UI, które mają się zrobić zielone na sukces (np. wszystkie Button Images + tła pól)")]
    [SerializeField] Graphic[] successTintTargets;

    [Header("References")]
    [SerializeField] CameraZoomController zoomController;
    [SerializeField] GameObject inventoryBarToHide;
    [SerializeField] GameObject[] hideExtraUI;

    [Header("Events")]
    public UnityEvent onConfirmed;
    public UnityEvent onSolved;
    public UnityEvent onCancelled;

    int hour;
    int minute;
    bool closingLocked = false;

    Color[] originalColors;

    void Awake()
    {
        if (zoomController == null)
            zoomController = FindFirstObjectByType<CameraZoomController>();

        // zapamiętaj oryginalne kolory targetów
        if (successTintTargets != null && successTintTargets.Length > 0)
        {
            originalColors = new Color[successTintTargets.Length];
            for (int i = 0; i < successTintTargets.Length; i++)
                originalColors[i] = successTintTargets[i] ? successTintTargets[i].color : Color.white;
        }
    }

    void OnEnable()
    {
        closingLocked = false;
        RestoreColors();

        // wczytaj ostatni czas
        if (ClockState.HasValue)
        {
            hour = ClockState.Hour;
            minute = ClockState.Minute;
        }
        else
        {
            hour = Mathf.Clamp(startHour, 0, 23);
            minute = Mathf.Clamp(startMinute, 0, 59);
        }

        if (inventoryBarToHide != null) inventoryBarToHide.SetActive(false);
        if (hideExtraUI != null)
            foreach (var go in hideExtraUI)
                if (go != null) go.SetActive(false);

        RefreshTexts();
    }

    void OnDisable()
    {
        if (inventoryBarToHide != null) inventoryBarToHide.SetActive(true);
        if (hideExtraUI != null)
            foreach (var go in hideExtraUI)
                if (go != null) go.SetActive(true);

        StopAllCoroutines();
        RestoreColors();
    }

    // ======= przyciski cyfr =======

    public void IncH1() { if (closingLocked) return; SetHourDigits(GetH1() + 1, GetH2()); SaveState(); }
    public void DecH1() { if (closingLocked) return; SetHourDigits(GetH1() - 1, GetH2()); SaveState(); }

    public void IncH2() { if (closingLocked) return; SetHourDigits(GetH1(), GetH2() + 1); SaveState(); }
    public void DecH2() { if (closingLocked) return; SetHourDigits(GetH1(), GetH2() - 1); SaveState(); }

    public void IncM1() { if (closingLocked) return; SetMinuteDigits(GetM1() + 1, GetM2()); SaveState(); }
    public void DecM1() { if (closingLocked) return; SetMinuteDigits(GetM1() - 1, GetM2()); SaveState(); }

    public void IncM2() { if (closingLocked) return; SetMinuteDigits(GetM1(), GetM2() + 1); SaveState(); }
    public void DecM2() { if (closingLocked) return; SetMinuteDigits(GetM1(), GetM2() - 1); SaveState(); }

    // ✅ OK
    public void Confirm()
    {
        if (closingLocked) return;
        closingLocked = true;

        SaveState();
        onConfirmed?.Invoke();

        bool solved = (!useSolutionCheck) || (hour == solutionHour && minute == solutionMinute);

        if (solved)
        {
            onSolved?.Invoke();
            ClockState.MarkSolved();
            StartCoroutine(SuccessThenClose());
            return;
        }

        // zła godzina -> nic zielonego, po prostu zostaw UI albo zamknij
        // Ja bym ZOSTAWIŁ puzzle otwarte, żeby gracz poprawił:
        closingLocked = false;
        // jeśli chcesz jednak zamykać: ClosePuzzle();
    }

    // ✅ Anuluj
    public void Cancel()
    {
        if (closingLocked) return;
        closingLocked = true;

        SaveState();
        onCancelled?.Invoke();
        ClosePuzzle();
    }

    IEnumerator SuccessThenClose()
    {
        TintGreen();

        if (audioSource != null && successClip != null)
            audioSource.PlayOneShot(successClip);

        yield return new WaitForSeconds(successSeconds);

        ClosePuzzle();
    }

    void SaveState()
    {
        ClockState.Set(hour, minute);
    }

    void ClosePuzzle()
    {
        gameObject.SetActive(false);
        if (zoomController != null) zoomController.ZoomOut();
    }

    void TintGreen()
    {
        if (successTintTargets == null) return;

        for (int i = 0; i < successTintTargets.Length; i++)
        {
            if (successTintTargets[i] == null) continue;
            var c = successTintTargets[i].color;
            c.g = 1f;
            c.r *= 0.3f;
            c.b *= 0.3f;
            successTintTargets[i].color = c;
        }
    }

    void RestoreColors()
    {
        if (successTintTargets == null || originalColors == null) return;

        for (int i = 0; i < successTintTargets.Length; i++)
            if (successTintTargets[i] != null)
                successTintTargets[i].color = originalColors[i];
    }

    int GetH1() => hour / 10;
    int GetH2() => hour % 10;
    int GetM1() => minute / 10;
    int GetM2() => minute % 10;

    void SetHourDigits(int tens, int ones)
    {
        tens = Wrap0to9(tens);
        ones = Wrap0to9(ones);

        int newHour = tens * 10 + ones;
        if (newHour > 23) newHour = 23;
        if (newHour < 0) newHour = 0;

        hour = newHour;
        RefreshTexts();
    }

    void SetMinuteDigits(int tens, int ones)
    {
        tens = Wrap0to9(tens);
        ones = Wrap0to9(ones);

        int newMinute = tens * 10 + ones;
        if (newMinute > 59) newMinute = 59;
        if (newMinute < 0) newMinute = 0;

        minute = newMinute;
        RefreshTexts();
    }

    int Wrap0to9(int v)
    {
        v %= 10;
        if (v < 0) v += 10;
        return v;
    }

    void RefreshTexts()
    {
        if (h1Text) h1Text.text = GetH1().ToString();
        if (h2Text) h2Text.text = GetH2().ToString();
        if (m1Text) m1Text.text = GetM1().ToString();
        if (m2Text) m2Text.text = GetM2().ToString();
    }
}