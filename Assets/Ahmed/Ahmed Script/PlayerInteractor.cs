using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using System.Collections;

public class PlayerInteractor : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayer;
    private int selectedSlotIndex = 0;

    public ItemData[] inventoryItems = new ItemData[5];
    public Image[] inventorySlotImages;
    private int currentItemCount = 0;

    public TextMeshProUGUI take;
    public GameObject pickupPanel;
    private GameObject currentPreviewObject;

    public Transform throwPoint;
    public float throwForce = 5f;
    public static bool incenseActivated = false;
    SoundManager SoundManager;
    public Light pointLight;
    public static bool flashlightactivated = false;
    public Camera playerCamera;
    public int button;
    public static int CountButton;
public static bool IsPowerOn = false;
    private bool canget = true;
    public bool getbutton = true;
    public bool ele = false;
    public bool bu = false;



    public void Button(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bu = true;
        }
    }
    public void Electric(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ele = true;
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryInteract();
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
            ShowItemPreview();
            Debug.Log(selectedSlotIndex);
        }
    }
    public void OnLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            invantoryOnLeft();
            ShowItemPreview();
            Debug.Log(selectedSlotIndex);

        }
    }
    void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Collect(this);
            }
        }
    }
    

    
     private void Start()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        SoundManager = SoundManager.Instance;




    }
    void Update()
    {
        CheckHover();


        if (CountButton >= 2 && canget) {


            GameObject[] doorss = GameObject.FindGameObjectsWithTag("Metaldoor");
            foreach (GameObject door in doorss)
            {
                door.GetComponent<Animator>().SetBool("IsOpen", true);
            }



        }

        checckItemSelect();

        // ShowItemPreview();

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

    private void checckItemSelect()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { selectedSlotIndex = 0;
            ShowItemPreview();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) { selectedSlotIndex = 1;
            ShowItemPreview();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) { selectedSlotIndex = 2;
            ShowItemPreview();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) { selectedSlotIndex = 3;
            ShowItemPreview();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedSlotIndex = 4;
            ShowItemPreview();
        }
    }

    void LateUpdate()
    {
        if (currentPreviewObject != null)
        {
            //Debug.Log("here");
            currentPreviewObject.transform.position = throwPoint.position + transform.forward * 1f;
            currentPreviewObject.transform.rotation = Quaternion.LookRotation(transform.forward);
        }
    }

    void CheckHover()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                take.text = $"Press E to take {hit.collider.name}";
                pickupPanel.SetActive(true);
            }
            else
            {
                //take.text = $"You see a {hit.collider.name}, but you can't take it.";
                //pickupPanel.SetActive(true);
                if (hit.collider.CompareTag("Electric")) {
                    pickupPanel.SetActive(true);
                    take.text = "You dont have Fuse";
                    Debug.Log(hit.collider.name);
                   
                        for (int i = 0; i < inventoryItems.Length; i++)
                        {

                            //Debug.Log(inventoryItems[i].itemType);
                            
                            if (inventoryItems[i] != null && inventoryItems[i].itemType == ItemType.Fuse)
                            {
                            pickupPanel.SetActive(true);
                            take.text = "Press L1 to Turn in The Electrical";
                            if (ele)
                            {

                                IsPowerOn = true;
                                Debug.Log("You Opend Doors");

                                GameObject[] doors = GameObject.FindGameObjectsWithTag("MetalDoor");
                                foreach (GameObject door in doors)
                                {
                                    door.GetComponent<Animator>().SetBool("IsOpen", true);
                                }

                                break;

                            }

                        }
                    }
                }

                
                if (hit.collider.CompareTag("Button"))
                {

                    pickupPanel.SetActive(true);
                    take.text = "Press X to Trigger The button";
                    if (bu) {
                        button = 1;
                        if (getbutton) {
                            CountButton += 1;
                            Debug.Log(CountButton);
                            getbutton = false;

                        }
                    }
                }
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
                if (flashlightactivated)
                {
                    pointLight.enabled = false; 
                    flashlightactivated = false; 
                    Debug.Log("Flashlight deactivated.");
                }
                else
                {
                    pointLight.enabled = true; 
                    flashlightactivated = true; 
                    Debug.Log("Used flashlight.");
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
            //currentPreviewObject.transform.SetParent(null); 
            //currentPreviewObject.transform.rotation = Quaternion.LookRotation(transform.right);
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
