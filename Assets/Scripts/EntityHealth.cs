using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public bool isPlayerOwned;
    public int maxHealth;
    [HideInInspector] public int health;

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("Die");
        }
    }
}
