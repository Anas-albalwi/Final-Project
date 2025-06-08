using TMPro;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayer;

    public ItemData[] inventoryItems = new ItemData[10]; // 🧠 10 خانات فقط
    private int currentItemCount = 0; // عدد العناصر المضافة
    public TextMeshProUGUI take;
    public GameObject pickupPanel;

    void Update()
    {
        CheckHover();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Collect(this);
            }
        }
    }

    void CheckHover()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                Debug.Log("Looking at: " + hit.collider.name);
                take.text = $"Press E to take {hit.collider.name}";
                pickupPanel.SetActive(true);
            }
            else
            {
                pickupPanel.SetActive(false);
                take.text = "";
                Debug.Log("Not interactable");
            }
        }
        else
        {
            pickupPanel.SetActive(false);
            take.text = "";
            Debug.Log("Looking at nothing");
        }
    }

    public bool AddToInventory(ItemData item)
    {
        if (currentItemCount >= inventoryItems.Length)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        inventoryItems[currentItemCount] = item;
        currentItemCount++;
        Debug.Log("Added to inventory: " + item.itemName);
        return true;
    }
}
