using UnityEngine;

public class GardenArea : MonoBehaviour
{
    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isPlayerInside && other.CompareTag("Player"))
        {
            isPlayerInside = true;

            SoundManager.Instance.musicSource.clip = SoundManager.Instance.gardenAmbience;
            SoundManager.Instance.musicSource.loop = true;
            SoundManager.Instance.musicSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlayerInside && other.CompareTag("Player"))
        {
            isPlayerInside = false;

            // √Êﬁ› «·„Ê”ÌﬁÏ √Ê ‘€· „Ê”ÌﬁÏ «› —«÷Ì… Õ”»  ’„Ì„ ·⁄» ﬂ
            SoundManager.Instance.musicSource.Stop();
            // √Ê  —Ã⁄ ·„Ê”ÌﬁÏ √Œ—Ï:
            // SoundManager.Instance.musicSource.clip = SoundManager.Instance.defaultMusic;
            // SoundManager.Instance.musicSource.Play();
        }
    }
}
