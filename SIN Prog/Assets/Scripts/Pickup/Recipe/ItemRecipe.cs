using UnityEngine;

public class ItemRecipe : MonoBehaviour
{
    public ItemRecipeSO recipe;
    public UIRecipe uiRecipe;
    public ToolController toolController;
    [field: SerializeField] VoidEventChannel VoidEventChannel { get; set; }
    //public Action<string> myString;
    //private string myName = "Nixon";
    public void OnHandlePickupRecipe()
    {
        //public event instead of making this type of reference?
        //Delegate!!!
        //myString?.Invoke(myName);
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
            //TODO handle proper way of removing Recipe
            uiRecipe.DestroyAllRecipeUI();
            Destroy(this.gameObject);
        }
    }
}
