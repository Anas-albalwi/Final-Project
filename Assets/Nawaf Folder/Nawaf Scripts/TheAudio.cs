using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public AudioSource[] speakerAudioSources; // Assign in Inspector
    public int dialogueIndex = 0; // 0 for Dialogue 1, 1 for Dialogue 2, etc.

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var audioSource in speakerAudioSources)
            {
                if (audioSource.isPlaying)
                    audioSource.Stop();
            }
            if (dialogueIndex >= 0 && dialogueIndex < speakerAudioSources.Length)
            {
                speakerAudioSources[dialogueIndex].Play();
            }
        }
    }
}