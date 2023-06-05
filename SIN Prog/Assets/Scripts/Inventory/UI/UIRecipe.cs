using UnityEngine;

public class UIRecipe : MonoBehaviour
{
    [SerializeField] private GameObject m_recipeSlotPrefab;
    [SerializeField] private GameObject m_RecipeResultPrefab;
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
        //Show Result
        GameObject resultObj = Instantiate(m_RecipeResultPrefab);
        resultObj.transform.SetParent(transform, false);

        RecipeResult result = resultObj.GetComponent<RecipeResult>();
        result.SetResult(recipe);
    }

    public void DestroyAllRecipeUI()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
}
