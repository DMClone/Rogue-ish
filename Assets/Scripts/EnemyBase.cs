using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public PlayerController _playerController;
    protected Rigidbody2D _rigidbody;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;

    [SerializeField] protected int _damage;
    [SerializeField] protected float _attackCooldown;
    protected Coroutine _c_attack;
    public float movementSpeed;
    protected float _distanceFromPlayer;

    protected void Start()
    {
        _playerController = PlayerController.instance;
        _rigidbody = GetComponent<Rigidbody2D>();
        GameManager.instance.ue_sceneClear.AddListener(RemoveFromScene);
    }

    protected void RemoveFromScene() => Destroy(gameObject);

    public virtual void FixedUpdate()
    {
        PlayerDistance();
    }

    protected void FaceVelocity()
    {
        if (_rigidbody.linearVelocityX < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_rigidbody.linearVelocityX > 0)
        {
            _spriteRenderer.flipX = false;
        }
    }

    protected void PlayerDistance()
    {
        _distanceFromPlayer = Vector2.Distance(_playerController.transform.position, transform.position);
    }

    protected void CollisionDamage(Health health)
    {
        health.TakeDamage(_damage);
    }
}
