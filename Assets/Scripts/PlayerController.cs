using System;
using System.Collections;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerController : MonoBehaviour, IShoot
{
    public static PlayerController instance;

    private Inventory _inventory;
    [SerializeField] private GameObject _droppedItem;
    private InventoryItem _inventoryItem;
    private PlayerInput _playerInput;
    private Rigidbody2D _rigidbody;
    [SerializeField] private GameObject _arm;
    [SerializeField] private GameObject _barrelExit;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    public Vector2 _moveDirection;
    private Coroutine _usingCoroutine;
    private Coroutine _rumbleCoroutine;

    public bool isFireHeld;
    [SerializeField] private float useCooldown;
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

        #region Inputs
        InputAction _playerMove = InputSystem.actions.FindAction("Move");
        _playerMove.performed += Move;
        _playerMove.canceled += MoveStop;
        InputAction _playerLook = InputSystem.actions.FindAction("Look");
        _playerLook.performed += Look;
        _playerLook.canceled += Look;
        InputAction _playerUse = InputSystem.actions.FindAction("Use");
        _playerUse.performed += Use;
        _playerUse.canceled += UseRelease;
        InputAction _playerSwitchL = InputSystem.actions.FindAction("SwitchLeft");
        _playerSwitchL.performed += SwitchL;
        InputAction _playerSwitchR = InputSystem.actions.FindAction("SwitchRight");
        _playerSwitchR.performed += SwitchR;
        InputAction _playerDrop = InputSystem.actions.FindAction("Drop");
        _playerDrop.performed += Drop;
        #endregion

        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start() => Switch();

    #region Move/Look
    private void Move(InputAction.CallbackContext context) => _moveDirection = context.ReadValue<Vector2>();

    private void MoveStop(InputAction.CallbackContext context) => _moveDirection = context.ReadValue<Vector2>();

    private void Look(InputAction.CallbackContext context)
    {
        if (_playerInput.currentControlScheme != "Keyboard" && context.ReadValue<Vector2>() != Vector2.zero)
        {
            lookingAngle = Vector2.Angle(Vector2.up, context.ReadValue<Vector2>());
            lookingDir = context.ReadValue<Vector2>();
        }
        else if (context.ReadValue<Vector2>() != Vector2.zero)
        {
            lookingDir = ((Vector2)Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()) - (Vector2)_arm.transform.position).normalized;
            lookingAngle = CalculateAngle(Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>()));
        }

        _arm.transform.up = lookingDir;
        _arm.transform.eulerAngles = new Vector3(_arm.transform.eulerAngles.x, 0, _arm.transform.eulerAngles.z);

        if (_arm.transform.eulerAngles.z > 0 && _arm.transform.eulerAngles.z < 180)
        {
            _arm.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipY = true;
            _barrelExit.transform.localPosition = new Vector3(_barrelExit.transform.localPosition.x, -0.11f, _barrelExit.transform.localPosition.z);
        }
        else
        {
            _arm.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipY = false;
            _barrelExit.transform.localPosition = new Vector3(_barrelExit.transform.localPosition.x, 0.11f, _barrelExit.transform.localPosition.z);
        }
    }
    #endregion

    #region Using
    private void Use(InputAction.CallbackContext context)
    {
        isFireHeld = true;
        if (_usingCoroutine == null)
        {
            _usingCoroutine = StartCoroutine(Using());
        }
    }

    private void UseRelease(InputAction.CallbackContext context)
    {
        isFireHeld = false;
    }

    private void UseItem()
    {
        if (_inventoryItem != null)
        {
            switch (_inventoryItem.item)
            {
                case Gun gun:
                    Debug.Log("Shot a gun");
                    useCooldown = gun.fireRate;
                    Camera.main.GetComponent<CameraShake>().ShakeScreen(gun.screenshakeStrength);
                    if (_playerInput.currentControlScheme != "Keyboard")
                    {
                        ControllerRumble(gun.rumbleLeft, gun.rumbleRight, gun.rumbleDuration);
                    }
                    FireShot(lookingDir, gun.bulletsPerShot, gun.spread, gun.bulletPrefab);
                    break;
                case Throwable throwable:
                    Debug.Log("Just threw a throwable!");
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator Using()
    {
        UseItem();
        yield return new WaitForSeconds(useCooldown);
        if (isFireHeld)
        {
            _usingCoroutine = StartCoroutine(Using());
        }
        else
        {
            _usingCoroutine = null;
        }
    }
    #endregion

    #region Switching
    private void SwitchL(InputAction.CallbackContext context)
    {
        if (_inventory.slotSelected > 0)
        {
            _inventory.slotSelected--;
        }
        else
        {
            _inventory.slotSelected = _inventory.inventorySlots.Count - 1;
        }
        Switch();
        _inventory.UpdateSelectPos();
    }

    private void SwitchR(InputAction.CallbackContext context)
    {
        if (_inventory.slotSelected < _inventory.inventorySlots.Count - 1)
        {
            _inventory.slotSelected++;
        }
        else
        {
            _inventory.slotSelected = 0;
        }
        Switch();
        _inventory.UpdateSelectPos();
    }

    public void Switch()
    {
        _inventoryItem = _inventory.inventorySlots[_inventory.slotSelected].GetComponentInChildren<InventoryItem>();
        if (_inventoryItem != null)
            _arm.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = _inventoryItem.item.inGameImage;
        else
            _arm.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }
    #endregion

    #region Updates
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
    #endregion

    private void Drop(InputAction.CallbackContext context)
    {
        InventorySlot slot = _inventory.inventorySlots[_inventory.slotSelected];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if ((itemInSlot != null) && !itemInSlot.item.isStackable)
        {
            GameObject droppedItem = Instantiate(_droppedItem, transform.position, Quaternion.identity);
            droppedItem.GetComponent<DroppedItem>().item = itemInSlot.item;
            droppedItem.GetComponent<Rigidbody2D>().linearVelocity = lookingDir * 5;
            Destroy(itemInSlot.gameObject);
            _inventory.slotsOccupied--;
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

    public void FireShot(Vector2 shotDir, int shots, float spread, GameObject bulletPrefab)
    {
        for (int i = 0; i < shots; i++)
        {
            float randomSpread = UnityEngine.Random.Range(-spread, spread) * Mathf.Deg2Rad;
            Vector2 bulletDir = new Vector2(
                Mathf.Cos(randomSpread) * shotDir.x - Mathf.Sin(randomSpread) * shotDir.y,
                Mathf.Sin(randomSpread) * shotDir.x + Mathf.Cos(randomSpread) * shotDir.y
            );

            Bullet bullet = Instantiate(bulletPrefab, _barrelExit.transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.direction = bulletDir;
            bullet.speed = 10;
        }
    }
}
