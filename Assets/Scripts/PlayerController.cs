using System;
using System.Collections;
using Unity.Mathematics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private Inventory _inventory;
    private PlayerInput _playerInput;
    private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _gun;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    public Vector2 _moveDirection;
    private Coroutine _shootingCoroutine;
    private Coroutine _rumbleCoroutine;

    public bool isFireHeld;
    public float lookingAngle;
    public Vector2 lookingDir;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _inventory = Inventory.instance;
        _playerInput = GetComponent<PlayerInput>();
        _animator = transform.GetChild(1).GetComponent<Animator>();

        InputAction _playerMove = InputSystem.actions.FindAction("Move");
        _playerMove.performed += Move;
        _playerMove.canceled += MoveStop;
        InputAction _playerLook = InputSystem.actions.FindAction("Look");
        _playerLook.performed += Look;
        _playerLook.canceled += Look;
        InputAction _playerShoot = InputSystem.actions.FindAction("Shoot");
        _playerShoot.performed += Shoot;
        _playerShoot.canceled += ShootRelease;
        InputAction _playerSwitchL = InputSystem.actions.FindAction("SwitchLeft");
        _playerSwitchL.performed += SwitchL;
        InputAction _playerSwitchR = InputSystem.actions.FindAction("SwitchRight");
        _playerSwitchR.performed += SwitchR;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
    }

    #region Move/Look
    private void Move(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
        // _animator.ResetTrigger("Todefault");
    }

    private void MoveStop(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
        // _animator.ResetTrigger("Run");
    }

    private void Look(InputAction.CallbackContext context)
    {
        if (_playerInput.currentControlScheme != "Keyboard" && context.ReadValue<Vector2>() != Vector2.zero)
        {
            lookingAngle = Vector2.Angle(Vector2.up, context.ReadValue<Vector2>());
            lookingDir = context.ReadValue<Vector2>();
        }
        else if (context.ReadValue<Vector2>() != Vector2.zero)
        {
            lookingDir = ((Vector2)Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()) - (Vector2)_gun.transform.position).normalized;
            lookingAngle = CalculateAngle(Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()));
        }

        _gun.transform.up = lookingDir;
        _gun.transform.eulerAngles = new Vector3(_gun.transform.eulerAngles.x, 0, _gun.transform.eulerAngles.z);

        if (_gun.transform.eulerAngles.z > 0 && _gun.transform.eulerAngles.z < 180)
        {
            _gun.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            _gun.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipY = false;
        }
    }
    #endregion

    #region Shooting
    private void Shoot(InputAction.CallbackContext context)
    {
        isFireHeld = true;
        if (_shootingCoroutine == null)
        {
            _shootingCoroutine = StartCoroutine(Shooting());
        }
    }

    private void ShootRelease(InputAction.CallbackContext context)
    {
        isFireHeld = false;
    }

    private void FireShot()
    {
        Camera.main.GetComponent<CameraShake>().ShakeScreen(0.2f);
        if (_playerInput.currentControlScheme != "Keyboard")
        {
            ControllerRumble(0.25f, 0.55f, 0.25f);
        }
    }

    private IEnumerator Shooting()
    {
        FireShot();
        yield return new WaitForSeconds(.25f);
        if (isFireHeld)
        {
            _shootingCoroutine = StartCoroutine(Shooting());
        }
        else
        {
            _shootingCoroutine = null;
        }
    }
    #endregion

    #region Switching
    private void SwitchL(InputAction.CallbackContext context)
    {
        Debug.Log("Went to the left");
        if (_inventory.slotSelected > 0)
        {
            _inventory.slotSelected--;
        }
        else
        {
            _inventory.slotSelected = _inventory.inventorySlots.Length - 1;
        }
        _inventory.UpdateSelectPos();
    }

    private void SwitchR(InputAction.CallbackContext context)
    {
        Debug.Log("Went to the right");
        if (_inventory.slotSelected < _inventory.inventorySlots.Length - 1)
        {
            _inventory.slotSelected++;
        }
        else
        {
            _inventory.slotSelected = 0;
        }
        _inventory.UpdateSelectPos();
    }
    #endregion

    void FixedUpdate()
    {
        _rigidbody.linearVelocity += _moveDirection;
    }

    void Update()
    {
        if (_rigidbody.linearVelocity.magnitude >= 0.4f)
        {
            _animator.SetTrigger("Run");
        }
        else
        {
            _animator.SetTrigger("ToDefault");
        }
        if (_rigidbody.linearVelocityX < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    public float CalculateAngle(Vector2 ownPos)
    {
        Vector2 _direction = ownPos - (Vector2)transform.position;
        float _calculatedAngle = Vector2.Angle(Vector2.up, _direction);
        return _calculatedAngle;
    }

    public void ControllerRumble(float lowFreq, float highFreq, float duration)
    {
        if (_rumbleCoroutine != null)
        {
            StopCoroutine(_rumbleCoroutine);
        }
        _rumbleCoroutine = StartCoroutine(Rumble(lowFreq, highFreq, duration));
    }

    private IEnumerator Rumble(float lowFreq, float highFreq, float duration)
    {
        Gamepad.current.SetMotorSpeeds(lowFreq, highFreq);
        yield return new WaitForSeconds(duration);
        Gamepad.current.SetMotorSpeeds(0, 0);
    }
}
