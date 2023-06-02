using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPlace : MonoBehaviour
{
    [SerializeField] private BoxCollider placeItemsArea;
    [SerializeField] private Transform itemSpawnPoint;
    public GameObject thisResult;

    public void Craft()
    {
        Collider[] colliderArray = Physics.OverlapBox(transform.position + placeItemsArea.center, placeItemsArea.size, placeItemsArea.transform.rotation);

        List<int> ingredient1 = new List<int>();
        List<GameObject> destroyables = new List<GameObject>();
        //List<int> flowerIngredient = new List<int>();
        foreach (Collider collider in colliderArray)
        {
            if(collider.TryGetComponent(out HolderForItemSO itemSOHolder))
            {
                ingredient1.Add(1);
                destroyables.Add(collider.gameObject);
            }
            /*if(collider.TryGetComponent(out Flower flower))
            {
                flowerIngredient.Add(1);
                destroyables.Add(collider.gameObject);
            }*/
        }
        if (ingredient1.Count == 4)
        {
            Instantiate(thisResult, itemSpawnPoint.position, itemSpawnPoint.rotation);
            foreach(GameObject destoryable in destroyables)
            {
                Destroy(destoryable);   
            }

        }
        /*if (ingredient1.Count == 2 && flowerIngredient.Count == 3)
        {
            Instantiate(thisResult);
        }
        Debug.Log(ingredient1.Count);*/
    }
}
