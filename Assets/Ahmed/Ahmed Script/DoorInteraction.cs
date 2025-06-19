using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    public float rayDistance = 2f;
    public PlayerInteractor interactor;
    public LayerMask doorLayer;
    public Camera playerCamera;

    private bool isHit;
    private bool triggred = false;

    public Animator animator;
    public void OnInteracts(InputAction.CallbackContext context)
    {
        triggred = context.performed;
    }
    void Update()
    {


        CheckForDoor();
        //Debug.Log(triggred);

    }
    void Awake()
    {
        interactor = GetComponent<PlayerInteractor>();
        animator = GetComponent<Animator>();

    }


    void CheckForDoor()
    {
        RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        isHit = Physics.Raycast(ray, out hit, rayDistance,doorLayer );

        if (isHit)
        {
            if (hit.collider.CompareTag("Room") && hit.collider.gameObject.layer == LayerMask.NameToLayer("Door"))
            {
                Debug.Log("i see it");
                if (interactor == null)
                {
                    Debug.LogError("❌ interactor is NULL! تأكد أن PlayerInteractor موجود في المشهد.");
                    return;
                }

                if (interactor.inventoryItems == null)
                {
                    Debug.LogError("❌ inventoryItems is NULL! تأكد أنه تم تهيئته.");
                    return;
                }

                Debug.Log("✅ interactor and inventoryItems exist.");
                Debug.Log(interactor.inventoryItems[0]);





                for (int i = 0; i < interactor.inventoryItems.Length; i++)
                {
                    Debug.Log("Current Index: " + i);
                    if (interactor.inventoryItems[i] == null)
                    {
                        Debug.LogWarning("Slot is Empty");
                        continue;
                    }

                    if (interactor.inventoryItems[i].itemType == ItemType.PoisonKey)
                    {
                        interactor.pickupPanel.SetActive(true);
                        interactor.take.text = "Press E to open The Door";
                        if (triggred)
                        {
                            hit.collider.GetComponent<Animator>().SetBool("IsOpen",true);
                            Debug.Log("You got it");
                            break;
                        }
                    }
                }
            }

           
        }
       
    }

    void OnDrawGizmos()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.green;
        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.forward * rayDistance;

        Gizmos.DrawRay(origin, direction);
    }

}