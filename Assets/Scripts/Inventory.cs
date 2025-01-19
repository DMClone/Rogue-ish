using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] private GameObject _select;
    [SerializeField] private Sprite _slotSprite;
    [SerializeField] private GameObject _itemPrefab;
    public int slotSelected = 0;
    public int draggingSlot; // The slot where the item inside is currently being dragged
    public InventorySlot[] inventorySlots;
    public int slotsOccupied;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (instance == null)
            instance = this;

    }

    public void UpdateSelectPos()
    {
        _select.transform.position = inventorySlots[slotSelected].transform.position;
    }

    public bool AddItem(Item item)
    {
        if (slotsOccupied != inventorySlots.Length)
        {
            if (item.isStackable)
            {
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    InventorySlot slot = inventorySlots[i];
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null && itemInSlot.item == item)
                    {
                        itemInSlot.count++;
                        itemInSlot.UpdateCount();
                        return true;
                    }
                }
            }

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    slotsOccupied++;
                    return true;
                }
            }
            return false;
        }
        else { return false; }
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItem = Instantiate(_itemPrefab, slot.transform);
        newItem.name = item.name;
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
