using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [SerializeField]

    private bool isNotCollected = true;
    public List<InventoryItem> InventoryItems = new List<InventoryItem>(4);
    public List<Image> inventorySlotImages = new List<Image>();
    public TextMeshProUGUI take;
    public GameObject pickupPanel;
    private int currentSelectedIndex = 0;
    public LayerMask interactableLayer;












    private void Start()
    {

        

       


    }

    private void Update()
    {
        UpdateInventoryUI();
        CheckForItems();

        UseFromInventory();
        if (Input.GetMouseButtonDown(1))
        {
            UseItem(currentSelectedIndex);
        }
    }






    

   

   

   


    


    private void SelectInventoryItem(int index)
    {
        if (index >= 0 && index < InventoryItems.Count)
        {
            InventoryItem selectedItem = InventoryItems[index];
            ItemData data = selectedItem.itemData;


        }
        else
        {
            Debug.Log($" No item in slot {index + 1}");
        }
    }
    public void UseItem(int index)
    {
        if (index < 0 || index >= InventoryItems.Count)
        {
            Debug.Log($"No item in slot {index + 1}");
            return;
        }


        InventoryItem selectedItem = InventoryItems[index];
        ItemData data = selectedItem.itemData;

        Debug.Log($"Used item: {data.itemName}");

        switch (data.itemType)
        {
            case ItemType.meat:
               


                break;

            case ItemType.carrot:
         

                break;

        }

        UpdateInventoryUI();
    }
    public void UseFromInventory()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(InventoryItems[0]);
            SelectInventoryItem(0);
            currentSelectedIndex = 0;

           
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            SelectInventoryItem(1);
            Debug.Log(InventoryItems[1]);
            currentSelectedIndex = 1;


        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectInventoryItem(2);
            Debug.Log(InventoryItems[2]);
            currentSelectedIndex = 2;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectInventoryItem(3);
            Debug.Log(InventoryItems[3]);
            currentSelectedIndex = 3;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectInventoryItem(4);
            Debug.Log(InventoryItems[4]);
            currentSelectedIndex = 4;

        }

    }




    public void AddToInventory(ItemData data)
    {
        InventoryItem newItem = new InventoryItem(data);



        if (data == null || string.IsNullOrEmpty(data.itemName) || data.icon == null)
        {
            Debug.LogError("ItemData is missing values!");
            return;
        }
        InventoryItems.Add(newItem);

        Debug.Log($"{data.itemName} added to inventory.");
        UpdateInventoryUI();
        pickupPanel.SetActive(false);
        take.text = "";

    }


    void CheckForItems()
    {

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f, interactableLayer))
        {


            if (hit.collider.CompareTag("Item"))
            {
                Debug.Log("In");
                ItemData data = hit.collider.GetComponent<ItemData>();

                if (isNotCollected)
                {
                    if (InventoryItems.Count < 5)
                    {
                        take.color = Color.white;

                        take.text = $"Press E to take {data.itemName}";
                        pickupPanel.SetActive(true);
                        Debug.Log("i");
                    }
                    else
                    {
                        take.color = new Color32(255, 0, 0, 255);

                        take.text = "Reeto is full";
                        pickupPanel.SetActive(true);

                    }
                }
                Debug.LogWarning($"Press E to take {data.itemName}");

                if (Input.GetKeyDown(KeyCode.E) && isNotCollected)
                {
                    if (InventoryItems.Count < 5)
                    {
                        isNotCollected = false;

                        Debug.Log(data);
                        if (data != null)
                        {
                            Debug.Log(data);

                            AddToInventory(data);

                        }
                        else
                        {
                            Debug.LogWarning("No ItemData found on the object.");
                        }
                       
                        pickupPanel.SetActive(false);
                        take.text = "";

                        Destroy(hit.collider.gameObject);

                        Invoke("ResetIsNotCollected", 1f);
                    }
                    else
                    {
                        Debug.Log("no item");
                    }
                }

            }
            else if (hit.collider.CompareTag("RatHome"))
            {
                Debug.Log("store");

                take.color = Color.white;

                take.text = $"Press E to Store";
                pickupPanel.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E) && InventoryItems.Count != 0)
                {
                    // FindObjectOfType<ItemManager>().RespawnItems();
                    Debug.Log(InventoryItems.Count);
                    Debug.Log(InventoryItems.ToString());
                    //SoundManager.Instance.SFXSource.pitch = 1;
                   

                    for (int i = InventoryItems.Count - 1; i >= 0; i--)
                    {
                        Debug.Log("remove :" + i);
                        InventoryItems.RemoveAt(i);
                    }


                }


            }






            else
            {
                pickupPanel.SetActive(false);
            }
        }
        else
        {
            pickupPanel.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pickupPanel.SetActive(false);
        take.text = "";

    }


    private void ResetIsNotCollected()
    {
        isNotCollected = true;
        Debug.Log("isNotCollected reset to true");
    }
    public void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlotImages.Count; i++)
        {
            if (i < InventoryItems.Count && InventoryItems[i] != null)
            {
                inventorySlotImages[i].sprite = InventoryItems[i].itemData.icon;
                inventorySlotImages[i].enabled = true;
            }
            else
            {
                inventorySlotImages[i].sprite = null;
                inventorySlotImages[i].enabled = false;
            }
        }



    }



}

