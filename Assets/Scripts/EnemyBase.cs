using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public PlayerController _playerController;
    protected Rigidbody2D _rigidbody;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;

    public float movementSpeed;
    protected float _distanceFromPlayer;

    protected void Start()
    {
        _playerController = PlayerController.instance;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

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

}
