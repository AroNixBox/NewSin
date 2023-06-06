using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingPlace : MonoBehaviour
{
    [SerializeField] private BoxCollider placeItemsArea;
    [SerializeField] private Transform itemSpawnPoint;
    [SerializeField] private GameObject thisResult;
    [SerializeField] private int requiredAmount;
    [SerializeField] private TextMeshProUGUI ItemText;

    private void Update()
    {
        ItemText.text = "You need " + requiredAmount + "             to craft            !";
    }
    public void Craft()
    {
        Collider[] colliderArray = Physics.OverlapBox(transform.position + placeItemsArea.center, placeItemsArea.size, placeItemsArea.transform.rotation);

        List<int> ingredient1 = new List<int>();
        List<GameObject> destroyables = new List<GameObject>();
        foreach (Collider collider in colliderArray)
        {
            if(collider.TryGetComponent(out Wood myWood))
            {
                ingredient1.Add(1);
                destroyables.Add(collider.gameObject);
            }
        }
        if (ingredient1.Count == requiredAmount)
        {
            Instantiate(thisResult, itemSpawnPoint.position, itemSpawnPoint.rotation);
            foreach(GameObject destoryable in destroyables)
            {
                Destroy(destoryable);   
            }

        }
        Debug.Log(ingredient1.Count);
    }
}
