using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator animator;
    private AudioSource audioSource;

    public AudioClip hoverClip;
    public AudioClip clickClip;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("IsHover", true);
        if (audioSource && hoverClip)
            audioSource.PlayOneShot(hoverClip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("IsHover", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource && clickClip)
            audioSource.PlayOneShot(clickClip);
    }
}