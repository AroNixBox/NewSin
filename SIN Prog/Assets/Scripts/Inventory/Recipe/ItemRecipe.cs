using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemRecipe : MonoBehaviour
{
    [Header("Important-Stetup")]
    public ItemRecipeSO recipe;
    public UIRecipe uiRecipe;
    public ToolController toolController;
    [field: SerializeField] VoidEventChannel VoidEventChannel { get; set; }

    [Header("NextRecipe-Properties")]
    [SerializeField] private GameObject nextRecipeToSpawn;
    public Transform[] spawnPoints;
    public void OnHandlePickupRecipe()
    {
        uiRecipe.SetUpRecipeUI(recipe);
        VoidEventChannel.OnEventRaised += CheckRecipeIngredients;
        this.gameObject.SetActive(false);
    }
    public void CheckRecipeIngredients()
    {
        if (recipe.MeetsRecipeRequirements())
        {
            VoidEventChannel.OnEventRaised -= CheckRecipeIngredients;
            recipe.RemoveRecipeRequirements();
            //Change here if want to add result to other Inventory (Equippable Tools)
            if (recipe.result.isTool)
            {
                toolController.AddTool(recipe.result);
            }
            else
            {
                InventorySystem.current.Add(recipe.result);
            }
            uiRecipe.DestroyAllRecipeUI();
            //Instantiating new Recipe when this one will be destroyed and inserting!
            if(nextRecipeToSpawn != null)
            {
                var nextRec = Instantiate(nextRecipeToSpawn, spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
                ItemRecipe nextRecipe = nextRec.GetComponent<ItemRecipe>();
                nextRecipe.uiRecipe = this.uiRecipe;
                nextRecipe.toolController = this.toolController;
                nextRecipe.spawnPoints = this.spawnPoints;
            }
            Destroy(this.gameObject);
        }
    }
}
