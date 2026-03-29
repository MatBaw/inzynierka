using System.Collections;
using UnityEngine;

public class RevealPaperWithPencil : MonoBehaviour
{
    [Header("Obrazy")]
    [SerializeField] private GameObject blurredBase;
    [SerializeField] private GameObject revealStep1;
    [SerializeField] private GameObject revealStep2;
    [SerializeField] private GameObject revealStep3;

    [Header("Zoom")]
    [SerializeField] private PaperZoomOpenClose paperZoom;

    [Header("Animacja")]
    [SerializeField] private float stepDuration = 0.35f;

    [Header("Opcjonalnie")]
    [SerializeField] private bool deselectPencilAfterUse = true;

    [Header("Audio")]
    [SerializeField] private AudioSource eraseAudioSource;

    private bool revealed = false;
    private bool busy = false;

    private void Awake()
    {
        if (eraseAudioSource == null)
            eraseAudioSource = GetComponent<AudioSource>();

        if (InventoryState.IsPaperRevealed())
            ShowFinalState();
        else
            ResetVisuals();
    }

    public void TryReveal()
    {
        Debug.Log("[Reveal] paperZoom=" + (paperZoom ? paperZoom.name : "NULL")
            + " | id=" + (paperZoom ? paperZoom.GetInstanceID().ToString() : "brak")
            + " | IsZoomedOnPaper=" + (paperZoom ? paperZoom.IsZoomedOnPaper.ToString() : "NULL"));

        Debug.Log("TryReveal");

        if (busy || revealed)
            return;

        if (paperZoom != null && !paperZoom.IsZoomedOnPaper)
        {
            Debug.Log("Brak zoomu");
            return;
        }

        if (!InventoryState.HasPencil())
        {
            Debug.Log("Brak ołówka");
            return;
        }

        if (!InventoryState.IsSelected(InventoryState.Pencil))
        {
            Debug.Log("Ołówek nie jest zaznaczony");
            return;
        }

        StartCoroutine(RevealRoutine());
    }

    private IEnumerator RevealRoutine()
    {
        busy = true;

        if (eraseAudioSource != null)
            eraseAudioSource.Play();

        ResetVisuals();
        yield return new WaitForSeconds(stepDuration);

        if (revealStep1 != null) revealStep1.SetActive(true);
        yield return new WaitForSeconds(stepDuration);

        if (revealStep2 != null) revealStep2.SetActive(true);
        yield return new WaitForSeconds(stepDuration);

        if (revealStep3 != null) revealStep3.SetActive(true);

        revealed = true;
        busy = false;

        InventoryState.SetPaperRevealed(true);

        if (deselectPencilAfterUse)
        {
            InventoryState.SetSelectedItem(InventoryState.None);

            InventorySlotItemUI[] all = FindObjectsOfType<InventorySlotItemUI>(true);
            foreach (var slot in all)
                slot.Refresh();
        }
    }

    private void ResetVisuals()
    {
        revealed = false;

        if (blurredBase != null) blurredBase.SetActive(true);
        if (revealStep1 != null) revealStep1.SetActive(false);
        if (revealStep2 != null) revealStep2.SetActive(false);
        if (revealStep3 != null) revealStep3.SetActive(false);
    }

    private void ShowFinalState()
    {
        revealed = true;

        if (blurredBase != null) blurredBase.SetActive(true);
        if (revealStep1 != null) revealStep1.SetActive(true);
        if (revealStep2 != null) revealStep2.SetActive(true);
        if (revealStep3 != null) revealStep3.SetActive(true);
    }
}






/*using System.Collections;
using UnityEngine;

public class RevealPaperWithPencil : MonoBehaviour
{
    [Header("Obrazy")]
    [SerializeField] private GameObject blurredBase;
    [SerializeField] private GameObject revealStep1;
    [SerializeField] private GameObject revealStep2;
    [SerializeField] private GameObject revealStep3;

    [Header("Zoom")]
    [SerializeField] private PaperZoomOpenClose paperZoom;

    [Header("Animacja")]
    [SerializeField] private float stepDuration = 0.35f;

    [Header("Opcjonalnie")]
    [SerializeField] private bool deselectPencilAfterUse = true;

    private bool revealed = false;
    private bool busy = false;

    private void Awake()
    {
        if (InventoryState.IsPaperRevealed())
            ShowFinalState();
        else
            ResetVisuals();
    }

    public void TryReveal()
    {
        Debug.Log("[Reveal] paperZoom=" + (paperZoom ? paperZoom.name : "NULL")
            + " | id=" + (paperZoom ? paperZoom.GetInstanceID().ToString() : "brak")
            + " | IsZoomedOnPaper=" + (paperZoom ? paperZoom.IsZoomedOnPaper.ToString() : "NULL"));

        Debug.Log("TryReveal");

        if (busy || revealed)
            return;

        if (paperZoom != null && !paperZoom.IsZoomedOnPaper)
        {
            Debug.Log("Brak zoomu");
            return;
        }

        if (!InventoryState.HasPencil())
        {
            Debug.Log("Brak ołówka");
            return;
        }

        if (!InventoryState.IsSelected(InventoryState.Pencil))
        {
            Debug.Log("Ołówek nie jest zaznaczony");
            return;
        }

        StartCoroutine(RevealRoutine());
    }

    private IEnumerator RevealRoutine()
    {
        busy = true;

        ResetVisuals();
        yield return new WaitForSeconds(stepDuration);

        if (revealStep1 != null) revealStep1.SetActive(true);
        yield return new WaitForSeconds(stepDuration);

        if (revealStep2 != null) revealStep2.SetActive(true);
        yield return new WaitForSeconds(stepDuration);

        if (revealStep3 != null) revealStep3.SetActive(true);

        revealed = true;
        busy = false;

        InventoryState.SetPaperRevealed(true);

        if (deselectPencilAfterUse)
        {
            InventoryState.SetSelectedItem(InventoryState.None);

            InventorySlotItemUI[] all = FindObjectsOfType<InventorySlotItemUI>(true);
            foreach (var slot in all)
                slot.Refresh();
        }
    }

    private void ResetVisuals()
    {
        revealed = false;

        if (blurredBase != null) blurredBase.SetActive(true);
        if (revealStep1 != null) revealStep1.SetActive(false);
        if (revealStep2 != null) revealStep2.SetActive(false);
        if (revealStep3 != null) revealStep3.SetActive(false);
    }

    private void ShowFinalState()
    {
        revealed = true;

        if (blurredBase != null) blurredBase.SetActive(true);
        if (revealStep1 != null) revealStep1.SetActive(true);
        if (revealStep2 != null) revealStep2.SetActive(true);
        if (revealStep3 != null) revealStep3.SetActive(true);
    }
}*/