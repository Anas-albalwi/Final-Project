using UnityEngine;

public class Anmation : MonoBehaviour
{
    [Header("Enemy Animator")]
    public Animator enemyAnimator;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }

    public void Walk()
    {
        enemyAnimator.SetBool("Running", true);
    }

    public void StopWalking()
    {
        enemyAnimator.SetBool("Walk", false);
    }

    public void Attack()
    {
        enemyAnimator.SetBool("Attack", true);
    }

    public void StopAttacking()
    {
        enemyAnimator.SetBool("Attack", false);
    }

    public void Die()
    {
        enemyAnimator.SetTrigger("Die");
    }

    public void ReactToDamage()
    {
        enemyAnimator.SetTrigger("Damage");
    }
}