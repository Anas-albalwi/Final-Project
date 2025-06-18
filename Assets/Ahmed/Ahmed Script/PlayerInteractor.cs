using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerInteractor : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayer;
    private int selectedSlotIndex = 0;

    public ItemData[] inventoryItems = new ItemData[10];
    public Image[] inventorySlotImages;
    private int currentItemCount = 0;

    public TextMeshProUGUI take;
    public GameObject pickupPanel;
    private GameObject currentPreviewObject;

    public Transform throwPoint;
    public float throwForce = 5f;
    public static bool incenseActivated = false;
    SoundManager SoundManager;
    public GameObject flashlight;
    public static bool flashlightactivated = true;




    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryInteract();
            electercal();
        }
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ThrowItem();

        }
    }
    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UseItem();

        }
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            invantoryOnRight();
            Debug.Log(selectedSlotIndex);
        }
    }
    public void OnLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            invantoryOnLeft();
            Debug.Log(selectedSlotIndex);

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
    void electercal() {
        
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
            {
                ElectricBoxInteraction box = hit.collider.GetComponent<ElectricBoxInteraction>();
                if (box != null)
                {
                    box.TryActivate(this);
                }

            }
        
    }
     void Start()
    {
        SoundManager = SoundManager.Instance;

    }
    void Update()
    {
        CheckHover();


        if (Input.GetKeyDown(KeyCode.E))
        {
            electercal();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedSlotIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedSlotIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedSlotIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedSlotIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) selectedSlotIndex = 4;

        ShowItemPreview();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
        if (Input.GetMouseButtonDown(1))
        {
            UseItem();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowItem();
        }

    
       
    }
    void LateUpdate()
    {
        if (currentPreviewObject != null)
        {
            currentPreviewObject.transform.position = throwPoint.position + transform.forward * 1f;
            currentPreviewObject.transform.rotation = Quaternion.LookRotation(transform.forward);
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
                take.text = $"You see a {hit.collider.name}, but you can't take it.";
                pickupPanel.SetActive(true);
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
        SoundManager.PlaySFX(SoundManager.collect);

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
            Debug.Log(" No item in selected slot to throw.");
            return;
        }

        if (item.worldPrefab == null)
        {
            Debug.LogWarning($" Item '{item.itemName}' does not have a worldPrefab assigned.");
            return;
        }

        GameObject droppedItem = Instantiate(item.worldPrefab, throwPoint.position, Quaternion.identity);

        Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        inventoryItems[selectedSlotIndex] = null;
        currentItemCount--;
        UpdateInventoryUI();

        Debug.Log($"🟡 Threw item: {item.itemName} from slot {selectedSlotIndex + 1}");
    }
    void UseItem()
    {
        if (selectedSlotIndex < 0 || selectedSlotIndex >= inventoryItems.Length)
            return;

        ItemData item = inventoryItems[selectedSlotIndex];
        if (item == null)
        {
            Debug.Log("No item in selected slot.");
            return;
        }

        switch (item.itemType)
        {
            case ItemType.Lighter:
                Debug.Log("Used lighter.");
                break;

            case ItemType.flashlight:
                if (flashlightactivated) {
                    flashlight.SetActive(true);
                    flashlightactivated = false;
                Debug.Log("Used flashlight.");
                }
                else {
                    flashlight.SetActive(false);

                    flashlightactivated = true;

                }
                break;  case ItemType.ReceptionKey:
                Debug.Log("Used ReceptionKey.");

                break;  case ItemType.PoisonKey:
                Debug.Log("Used PoisonKey.");

                break;  case ItemType.ElectricalKey:
                Debug.Log("Used ElectricalKey.");
                break;

            case ItemType.Charcoal:
                Debug.Log("Used charcoal.");
                break;

           

            case ItemType.Tool:
                Debug.Log("Used tool.");
                break;

           

            case ItemType.Incense:
                bool hasCharcoal = false;
                bool hasLighter = false;

                foreach (ItemData i in inventoryItems)
                {
                    if (i == null) continue;

                    if (i.itemType == ItemType.Charcoal)
                        hasCharcoal = true;
                    else if (i.itemType == ItemType.Lighter)
                        hasLighter = true;
                }

                if (hasCharcoal && hasLighter)
                {
                    PlayerInteractor.incenseActivated = true;
                    Debug.Log("Incense has been activated.");
                }
                else
                {
                    Debug.Log("You need both charcoal and a lighter to activate the incense.");
                }
                break;

            default:
                Debug.Log("Unknown item type.");
                break;
        }

        // inventoryItems[selectedSlotIndex] = null;
        // currentItemCount--;
        UpdateInventoryUI();
    }

    void ShowItemPreview()
    {
        if (currentPreviewObject != null)
        {
            Destroy(currentPreviewObject);
            currentPreviewObject = null;
        }

        ItemData item = inventoryItems[selectedSlotIndex];
        if (item != null && item.previewPrefab != null)
        {
            currentPreviewObject = Instantiate(item.previewPrefab, throwPoint.position + transform.forward * 1f, Quaternion.identity);
            currentPreviewObject.transform.SetParent(null); 
            currentPreviewObject.transform.rotation = Quaternion.LookRotation(transform.forward);
        }
    }
    void invantoryOnRight() {

        if (selectedSlotIndex<4) { 
        selectedSlotIndex +=1;
    }else{
            selectedSlotIndex = 0;
        }


    }
    void invantoryOnLeft()
    {

        if (selectedSlotIndex >0)
        {
            selectedSlotIndex -= 1;
        }
        else
        {
            selectedSlotIndex = 4;
        }

    }
}
