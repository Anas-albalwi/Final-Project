using UnityEngine;
public enum ItemType
{
    Burger,
    meat,
    Cheese,
    carrot,



}
public class ItemData : MonoBehaviour
{
   
    public string itemName;
    //public Sprite icon;
    public ItemType itemType;
   public GameObject GameObject;
}

