using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("IsHover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("IsHover", false);
    }
}