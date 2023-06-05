using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
{
    public Image m_icon;
    public TextMeshProUGUI m_label;

    public void Set(ItemRecipeSO recipe, int index)
    {
        m_label.text = recipe.ingredients[index].amount.ToString();
        m_icon.sprite = recipe.ingredients[index].item.icon;
    }

}
