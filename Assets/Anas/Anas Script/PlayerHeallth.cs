using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("ÇááÇÚÈ ãÇÊ");
        // ÊÞÏÑ ÊÖíÝ åäÇ ÇäãíÔä ãæÊ Ãæ ÊÚØíá ÇáÍÑßÉ Ãæ ÅÚÇÏÉ ÇáãÔåÏ
        //gameObject.SetActive(false);
        SceneManager.LoadScene(4);

    }
}
