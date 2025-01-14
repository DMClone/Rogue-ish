using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class DroppedItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    public Item item;

    private void Update()
    {
        transform.eulerAngles += new Vector3(0, 1, 0) * 150 * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.GetComponent<PlayerController>() != null) && Inventory.instance.AddItem(item) != false)
        {
            Destroy(gameObject);
        }
    }
}