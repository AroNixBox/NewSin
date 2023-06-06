using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image m_icon;
    [SerializeField] private TextMeshProUGUI m_label;
    [SerializeField] private GameObject m_stackObj;
    [SerializeField] private TextMeshProUGUI m_stackLabel;
    [Header("RemoveFromInventory")]
    [SerializeField] private Button DropButton;
    private InventoryItem currentSelectedItem;
    private void Start()
    {
        Button btn = DropButton.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
    public void Set(InventoryItem item)
    {
        currentSelectedItem = item;
        m_icon.sprite = item.data.icon;
        m_label.text = item.data.displayName;
        if (item.stackSize <= 1)
        {
            m_stackObj.SetActive(false);
            return;
        }
        m_stackLabel.text = item.stackSize.ToString();
    }
    public void OnClick()
    {
        if (!currentSelectedItem.data.isEdible)
            Instantiate(currentSelectedItem.data.prefab, InventorySystem.current.ObjectDropPoint.transform.position, Quaternion.identity);
        else
            PlayerStats.Instance.Eat(currentSelectedItem.data.eatStats);

        currentSelectedItem.RemoveFromStack();
        m_stackLabel.text = currentSelectedItem.stackSize.ToString();
        if(currentSelectedItem.stackSize <= 0)
            Destroy(this.gameObject);

    }
}
