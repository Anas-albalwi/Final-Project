using UnityEngine;
using UnityEngine.EventSystems;

public class MoveObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;
    private AudioSource audioSource; // 1

    void Start()
    {
        animator = GetComponent<Animator>();
        
        audioSource = GetComponent<AudioSource>(); // 2
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetTrigger("Hover");

        if (audioSource != null) // 3
            audioSource.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger("Idle"); // Or use a bool if you want smooth transitions
    }
}