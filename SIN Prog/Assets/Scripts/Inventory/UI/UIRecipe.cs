using UnityEngine;

public class UIRecipe : MonoBehaviour
{
    [SerializeField] private GameObject m_recipeSlotPrefab;
    /*[SerializeField] private ItemRecipe m_recipe;

    private void OnEnable()
    {
        //Make sure Method is added before invoke is called.
        //m_recipe.myString += StringMethod => { Debug.Log(StringMethod); };
    }*/
    public void SetUpRecipeUI(ItemRecipeSO recipe)
    {
        for (int i = 0; i < recipe.ingredients.Count; i++)
        {
            GameObject obj = Instantiate(m_recipeSlotPrefab);
            obj.transform.SetParent(transform, false);

            RecipeSlot slot = obj.GetComponent<RecipeSlot>();
            slot.Set(recipe, i);
        }
    }

    public void DestroyAllRecipeUI()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
}
