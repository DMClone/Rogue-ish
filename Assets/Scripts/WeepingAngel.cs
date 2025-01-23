using UnityEngine;

public class WeepingAngel : EnemyBase
{
    [SerializeField] private float requiredAngle;
    private bool _hasLOS;

    public override void FixedUpdate()
    {
        Vector2 directionToEnemy = (transform.position - _playerController.transform.position).normalized;

        float angle = Vector2.SignedAngle(_playerController.lookingDir, directionToEnemy);

        if (Mathf.Abs(angle) <= requiredAngle)
        {
            _hasLOS = true;
        }
        else { _hasLOS = false; }

        if (!_hasLOS) { _rigidbody.linearVelocity += (Vector2)(_playerController.transform.position - transform.position).normalized * movementSpeed; }
        else { _rigidbody.linearVelocity = Vector2.zero; }
    }

    private void Update()
    {
        FaceVelocity();

        if (_rigidbody.linearVelocity.magnitude != 0)
        {
            _animator.SetTrigger("Run");
        }
        else
        {
            _animator.SetTrigger("Idle");
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {

    }
}
