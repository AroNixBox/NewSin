using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryItemDataSO, InventoryItem> m_itemDictionary;
    public static InventorySystem current;
    [field: SerializeField]
    public List<InventoryItem> inventory { get; private set; }
    public event Action onInventoryChangedEvent;
    [Header("NeedThis - ToDrop")]
    public Transform ObjectDropPoint;

    private void Awake()
    {
        if (current != null && current != this)
            Destroy(this);
        else
            current = this;

        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemDataSO, InventoryItem>();
    }
    public void Add(InventoryItemDataSO referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
        onInventoryChangedEvent.Invoke();
    }
    public InventoryItem Get(InventoryItemDataSO referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }
    public void Remove(InventoryItemDataSO referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
            onInventoryChangedEvent.Invoke();
        }
    }
}
