using UnityEngine;

public class IncenseRaycaster : MonoBehaviour
{
    private PlayerInteractor player;

    void Start()
    {
        player = FindObjectOfType<PlayerInteractor>();
    }

    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerInteractor>();
            if (player == null) return;
        }
        if (player.hasActivatedIncense)
        {
            Ray ray = new Ray(transform.position, Vector3.up);

            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                if (hit.collider.CompareTag("Alarm"))
                {
                    Debug.Log("Alarm triggered!");
                }
            }
        }
    }
}
