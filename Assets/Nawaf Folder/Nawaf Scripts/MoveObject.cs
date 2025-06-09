using UnityEngine;
using UnityEngine.EventSystems;

public class MoveObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetTrigger("Hover");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger("Idle"); // Or use a bool if you want smooth transitions
    }
}