using UnityEngine;
using System.Collections;

public class IncenseRaycaster : MonoBehaviour
{
    private PlayerInteractor player;
    public GameObject gameObject;
    public bool AlarmNotActivated= true;
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

        Debug.Log(PlayerInteractor.incenseActivated);
        if (PlayerInteractor.incenseActivated)
        {
            Ray ray = new Ray(transform.position, Vector3.up);

            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                if (hit.collider.CompareTag("Alarm") && AlarmNotActivated)
                {
                    StartCoroutine(TriggerAlarmSequence());

                    AlarmNotActivated = false;
                    Debug.Log("Alarm triggered!");
                }
            }
        }
    }
    IEnumerator TriggerAlarmSequence()
    {
        SoundManager.Instance.AlarmSource.Play();
        Debug.Log("Alarm triggered!");

        yield return new WaitForSeconds(2f);

        SoundManager.Instance.WaterSource.Play();

        Debug.Log("Water sound started");
        SoundManager.Instance.FireDownSource.Play();

        yield return new WaitForSeconds(5f);
        SoundManager.Instance.WaterSource.Stop();
        SoundManager.Instance.AlarmSource.Stop();
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.FireDownSource.Stop();

        Debug.Log("Water sound stopped");
    }
}
