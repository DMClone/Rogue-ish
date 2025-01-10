using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class DroppedItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    public Item item;

    private void Update()
    {
        transform.eulerAngles += new Vector3(0, 1, 0) * 0.4f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            Debug.Log("picked Item up");
        }
    }
}
