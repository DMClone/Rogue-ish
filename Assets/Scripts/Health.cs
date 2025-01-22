using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public bool isPlayer = false;
    public int health;
    public int maxHealth;

    private void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            _animator.SetTrigger("Death");
            StartCoroutine(DestroySelf());
        }
        else { _animator.SetTrigger("Hurt"); }

    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
