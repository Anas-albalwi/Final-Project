using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;

    public InventoryItem(ItemData data)
    {
        itemData = data;
    }
}
