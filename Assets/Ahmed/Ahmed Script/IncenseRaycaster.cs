using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class IncenseRaycaster : MonoBehaviour
{
    private PlayerInteractor player;
  //  public GameObject gameObject;
    public bool AlarmNotActivated= true;
    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    public GameObject fire;
    public GameObject MetalDoor;


    void Start()
    {
        player = FindObjectOfType<PlayerInteractor>();
       GameObject[] rains = GameObject.FindGameObjectsWithTag("Rain");
        fire = GameObject.FindGameObjectWithTag("Fire");
        //MetalDoor = GameObject.FindGameObjectWithTag("MetalDoor");

        for (int i = 0; i < rains.Length; i++)
        {
            particleSystems.Add(rains[i].GetComponent<ParticleSystem>());
        }


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
                    runRain();
                    fire.gameObject.SetActive(false); 

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

    void runRain()
    {
        for (int i = 0; i < particleSystems.Count; i++)
        {
            Debug.Log("rain is played");
           particleSystems[i].Play();
        }
    }
}
