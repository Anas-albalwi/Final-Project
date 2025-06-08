using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayer;
    private int selectedSlotIndex = 0;

    public ItemData[] inventoryItems = new ItemData[10]; // ← المصفوفة
    public Image[] inventorySlotImages; // ← لعرض صور العناصر
    private int currentItemCount = 0;

    public TextMeshProUGUI take;
    public GameObject pickupPanel;

    public Transform throwPoint;       // المكان اللي ينرمي منه الغرض
    public float throwForce = 5f;

    void Update()
    {
        CheckHover();

        // اختيار العنصر حسب الرقم
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedSlotIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedSlotIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedSlotIndex = 2;

        // جمع العناصر
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        // رمي العنصر المحدد بالضغط على G
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowItem();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Collect(this); // تمرير اللاعب
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
                take.text = $"Press E to take {hit.collider.name}";
                pickupPanel.SetActive(true);
            }
            else
            {
                pickupPanel.SetActive(false);
                take.text = "";
            }
        }
        else
        {
            pickupPanel.SetActive(false);
            take.text = "";
        }
    }

    public bool AddToInventory(ItemData item)
    {
        if (selectedSlotIndex < 0 || selectedSlotIndex >= inventoryItems.Length)
        {
            Debug.Log(" Invalid slot index.");
            return false;
        }

        if (inventoryItems[selectedSlotIndex] != null)
        {
            Debug.Log($" Slot {selectedSlotIndex + 1} is already occupied! Please select an empty slot.");
            return false;
        }

        inventoryItems[selectedSlotIndex] = item;
        currentItemCount++;

        Debug.Log($" Added '{item.itemName}' to slot {selectedSlotIndex + 1}.");
        UpdateInventoryUI();
        return true;
    }


    void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlotImages.Length; i++)
        {
            if (i < inventoryItems.Length && inventoryItems[i] != null)
            {
                inventorySlotImages[i].sprite = inventoryItems[i].icon;
                inventorySlotImages[i].enabled = true;
            }
            else
            {
                inventorySlotImages[i].sprite = null;
                inventorySlotImages[i].enabled = false;
            }
        }
    }

    void ThrowItem()
    {
        if (selectedSlotIndex < 0 || selectedSlotIndex >= inventoryItems.Length)
            return;

        ItemData item = inventoryItems[selectedSlotIndex];

        if (item == null)
        {
            Debug.Log("⚠️ No item in selected slot to throw.");
            return;
        }

        if (item.worldPrefab == null)
        {
            Debug.LogWarning($"⚠️ Item '{item.itemName}' does not have a worldPrefab assigned.");
            return;
        }

        // Instantiate the item's specific prefab
        GameObject droppedItem = Instantiate(item.worldPrefab, throwPoint.position, Quaternion.identity);

        Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        // Remove item from inventory
        inventoryItems[selectedSlotIndex] = null;
        currentItemCount--;
        UpdateInventoryUI();

        Debug.Log($"🟡 Threw item: {item.itemName} from slot {selectedSlotIndex + 1}");
    }
}
