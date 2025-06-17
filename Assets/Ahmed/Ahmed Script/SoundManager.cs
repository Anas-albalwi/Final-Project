using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;


    [Header("----------- Audio Source -----------")]
    [SerializeField] public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource fireSource;
    public AudioSource AlarmSource;
    public AudioSource WaterSource;
    public AudioSource FireDownSource;


    [Header("----------- Audio Clips -----------")]
    public AudioClip collect;
    public AudioClip Flame;
    public AudioClip gardenAmbience;
    public AudioClip Alarm;
    public AudioClip Water;
    public AudioClip FireDown;






    private Dictionary<string, AudioClip> sfxClips;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        sfxClips = new Dictionary<string, AudioClip>
        {
            { "collect",collect },
            { "Flame",Flame },
            { "garden", gardenAmbience },
            { "Alarm", Alarm },
            { "Water", Water },



        };
    }

    private void Start()
    {
        musicSource.loop = true;
        musicSource.Play();
        SFXSource.volume = 0.5f;

        if (fireSource != null && fireSource.clip != null)
        {
            fireSource.loop = true;
            fireSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
        SFXSource.pitch += 0.01f;

    }
}