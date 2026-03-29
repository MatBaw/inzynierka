using UnityEngine;

public class OpenPastCabinetOnce : MonoBehaviour
{
    [Header("Obiekty")]
    [SerializeField] private GameObject insideObject;
    [SerializeField] private GameObject cheeseObject;

    [Header("Opcjonalnie")]
    [SerializeField] private Collider2D clickableColliderToDisable;

    private void Awake()
    {
        if (clickableColliderToDisable == null)
            clickableColliderToDisable = GetComponent<Collider2D>();
    }

    private void Start()
    {
        ApplyState();
    }

    private void OnMouseDown()
    {
        OpenCabinet();
    }

    public void OpenCabinet()
    {
        if (InventoryState.IsPastCabinetOpened())
            return;

        InventoryState.SetPastCabinetOpened(true);
        ApplyState();

        Debug.Log("[OpenPastCabinetOnce] Szafka została otwarta.");
    }

    private void ApplyState()
    {
        bool opened = InventoryState.IsPastCabinetOpened();

        if (insideObject != null)
            insideObject.SetActive(opened);

        if (cheeseObject != null)
        {
            bool showCheese = opened && !InventoryState.HasCheese() && !InventoryState.IsMouseHoleSolved();
            cheeseObject.SetActive(showCheese);
        }

        if (opened && clickableColliderToDisable != null)
            clickableColliderToDisable.enabled = false;
    }
}