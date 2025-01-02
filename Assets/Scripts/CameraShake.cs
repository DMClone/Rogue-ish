using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform _player;
    [SerializeField] private CinemachineCamera _cinemachine;
    private CinemachineFollow _cinemachineFollow;
    private Vector2 _direction;
    private float _multiplier;

    private void Awake()
    {
        _cinemachineFollow = _cinemachine.GetComponent<CinemachineFollow>();
    }

    private void Start()
    {
        _player = PlayerController.instance.transform;
    }

    public void ShakeScreen(float intensity)
    {
        _direction = Random.insideUnitCircle.normalized;
        _multiplier = intensity;
        UpdateCinemamachine();
    }

    private void UpdateCinemamachine()
    {
        _cinemachine.gameObject.transform.localPosition = Vector3.zero;
        _cinemachineFollow.FollowOffset = new Vector3(_direction.x * _multiplier, _direction.y * _multiplier, _cinemachineFollow.FollowOffset.z);
    }

    private void Update()
    {
        if (_multiplier > 0)
        {

            _multiplier -= Time.deltaTime * (_multiplier + 1f);
        }
        else
        {
            _multiplier = 0;
        }
        UpdateCinemamachine();
    }
}
