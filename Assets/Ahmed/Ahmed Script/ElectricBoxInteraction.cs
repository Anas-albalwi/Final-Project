using UnityEngine;

public class ElectricBoxInteraction : MonoBehaviour
{
    public GameObject successActionObject;

    public void TryActivate(PlayerInteractor player)
    {
        if (player == null) return;

        foreach (ItemData item in player.inventoryItems)
        {
            if (item != null && item.itemType == ItemType.Fuse) 
            {
                Debug.Log(" Fuse used, electricity restored.");
                if (successActionObject != null)
                    successActionObject.SetActive(true);
                return;
            }
        }

        Debug.Log("❌ No fuse in inventory.");
    }
}