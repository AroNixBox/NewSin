using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeResult : MonoBehaviour
{

    public Image resultImage;
    public TextMeshProUGUI resultText;

    public void SetResult(ItemRecipeSO recipe)
    {
        resultImage.sprite = recipe.result.icon;
        resultText.text = recipe.result.displayName;
    }
}
