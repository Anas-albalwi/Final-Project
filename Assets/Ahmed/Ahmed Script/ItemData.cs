using UnityEngine;

public enum ItemType
{
    Burger,
    Meat,
    Cheese,
    Carrot,
    Tool,
    Key
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public GameObject worldPrefab;
}
