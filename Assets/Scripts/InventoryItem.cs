using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public Text countText;

    public Transform parentAfterDrag;
    public Item item;
    public int count = 1;

    private void Start()
    {
        parentAfterDrag = transform.parent;
        UpdateCount();
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        UpdateCount();
    }

    public void UpdateCount()
    {
        if (count > 1)
        {
            countText.text = count.ToString();
            countText.gameObject.SetActive(true);
        }
        else
        {
            countText.gameObject.SetActive(false);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Began dragging");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Stopped dragging");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
