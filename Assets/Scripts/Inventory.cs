using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] private GameObject _select;
    [SerializeField] private Sprite _slotSprite;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _slot;
    [SerializeField] private GameObject _score;
    [SerializeField] private GameObject _coins;
    public int slotSelected = 0;
    public int draggingSlot; // The slot where the item inside is currently being dragged
    public List<InventorySlot> inventorySlots;
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
        if (SwapSelection.instance != null) SwapSelection.instance.transform.position = inventorySlots[SwapSelection.instance.selectedSlot].transform.position;
    }

    public bool AddItem(Item item)
    {
        if (slotsOccupied != inventorySlots.Count)
        {
            if (item.isStackable)
            {
                for (int i = 0; i < inventorySlots.Count; i++)
                {
                    InventorySlot slot = inventorySlots[i];
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null && itemInSlot.item == item)
                    {
                        itemInSlot.count++;
                        itemInSlot.UpdateCount();
                        PlayerController.instance.SwitchItem();
                        return true;
                    }
                }
            }

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    slotsOccupied++;
                    PlayerController.instance.SwitchItem();
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

    public void SlotsAdded()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(15 + 20 * inventorySlots.Count, 30);
        UpdateSelectPos();
        _score.GetComponent<RectTransform>().localPosition = new Vector2(30 + -10 * inventorySlots.Count, _score.GetComponent<RectTransform>().localPosition.y);
        _coins.GetComponent<RectTransform>().localPosition = new Vector2(-30 + 10 * inventorySlots.Count, _coins.GetComponent<RectTransform>().localPosition.y);
    }
}

