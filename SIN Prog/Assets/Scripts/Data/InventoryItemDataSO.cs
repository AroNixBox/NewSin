using UnityEngine;

[CreateAssetMenu(menuName = "InventoryItem")]
public class InventoryItemDataSO : ScriptableObject
{
    public bool isEdible;
    public float eatStats;
    public bool isTool;
    public string id;
    public string displayName;
    public Sprite icon;
    public GameObject prefab;
}

