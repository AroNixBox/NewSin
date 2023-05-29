using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour
{
    public Transform equippedToolPosition;
    public GameObject equippedToolGO;
    public List<InventoryItemDataSO> ownedWeapons { get; private set; }
    int currentWeaponSelected;

    private void Awake()
    {
        ownedWeapons = new List<InventoryItemDataSO>();
    }
    public void AddTool(InventoryItemDataSO tool)
    {
        ownedWeapons.Add(tool);
        currentWeaponSelected = ownedWeapons.Count - 1;
    }
    public void SwitchTool()
    {
        if (equippedToolGO != null)
        {
            equippedToolGO.SetActive(false);
            equippedToolGO = null;
        }
        if (ownedWeapons.Count - 1 < 0)
        {
            Debug.Log("noway");
            return;
        }
        GameObject obj = Instantiate(ownedWeapons[currentWeaponSelected].prefab);
        obj.transform.SetParent(equippedToolPosition, false);
        if (currentWeaponSelected < ownedWeapons.Count - 1)
        {
            currentWeaponSelected++;
        }
        else if (currentWeaponSelected >= ownedWeapons.Count - 1)
        {
            currentWeaponSelected = 0;
        }
        equippedToolGO = obj;
    }
}
