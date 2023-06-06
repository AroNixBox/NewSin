using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    [field: SerializeField]
    public InventoryItemDataSO data { get; private set; }
    [field: SerializeField]
    public int stackSize { get; private set; }
    public InventoryItem(InventoryItemDataSO source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }
    public void RemoveFromStack()
    {
        stackSize--;
    }
}

