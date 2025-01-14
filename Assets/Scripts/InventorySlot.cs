using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem draggableItem = dropped.GetComponent<InventoryItem>();
        if (transform.childCount == 1)
        {
            gameObject.transform.GetChild(0).transform.SetParent(draggableItem.parentAfterDrag);
        }
        draggableItem.parentAfterDrag = transform;
    }

}
