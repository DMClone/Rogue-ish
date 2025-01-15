using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DroppedItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public Item item;
    public bool _isPlayerInCollider = true;

    private void Start()
    {
        if (item != null)
            _spriteRenderer.sprite = item.image;
    }

    private void Update()
    {
        transform.GetChild(0).transform.eulerAngles += new Vector3(0, 1, 0) * 150 * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.GetComponent<PlayerController>() != null) && !_isPlayerInCollider && Inventory.instance.AddItem(item) != false)
            Destroy(gameObject);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
            _isPlayerInCollider = false;
    }
}
