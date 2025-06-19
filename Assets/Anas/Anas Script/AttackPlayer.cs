using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
