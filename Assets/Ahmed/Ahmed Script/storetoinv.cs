using UnityEngine;

public class storetoinv : MonoBehaviour , IInteractable
{
    public ItemData itemData;

    public void Collect(PlayerInteractor player)
    {
        if (player != null && itemData != null)
        {
            if (player.AddToInventory(itemData))
            {
                Destroy(gameObject);
            }
        }
    }
}
