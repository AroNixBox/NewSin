using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemPickup", menuName = "ItemRecipe")]
public class ItemRecipeSO : ScriptableObject
{
    [field: SerializeField]
    public List<Ingredient> ingredients;
    public InventoryItemDataSO result;

    public bool MeetsRecipeRequirements()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            if (!ingredient.HasIngredient())
                return false;
        }
        return true;
    }
    public void RemoveRecipeRequirements()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            for (int i = 0; i < ingredient.amount; i++)
            {
                InventorySystem.current.Remove(ingredient.item);
            }
        }
    }

}
[System.Serializable]
public struct Ingredient
{
    public InventoryItemDataSO item;
    public uint amount;

    public bool HasIngredient()
    {
        InventoryItem thatItem = InventorySystem.current.Get(item);

        if (thatItem == null || thatItem.stackSize < amount)
            return false;
        return true;
    }
}
