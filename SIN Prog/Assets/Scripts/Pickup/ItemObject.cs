using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemDataSO referenceItem;
    public void OnHandlePickupItem()
    {
        InventorySystem.current.Add(referenceItem);
        gameObject.SetActive(false);
    }
}