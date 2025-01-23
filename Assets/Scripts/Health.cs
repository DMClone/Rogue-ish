using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [HideInInspector] public UnityEvent ue_died;
    public bool isPlayer = false;
    public int health;
    public int maxHealth;
    [SerializeField] private int _scoreOnDeath;
    [SerializeField] private int _coinsOnDeath;

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
        ue_died.Invoke();
        GetComponent<Collider2D>().enabled = false;
        GameManager gameManager = GameManager.instance;
        gameManager.score += _scoreOnDeath;
        gameManager.coins += _coinsOnDeath;
        gameManager.UpdateScoreAndCoins();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
