using UnityEngine;

public enum ItemType
{
    Lighter,
    Charcoal,
    Incense,
    Tool,
    Key,
    Fuse,
    flashlight
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public GameObject worldPrefab;
    public GameObject previewPrefab;
}
