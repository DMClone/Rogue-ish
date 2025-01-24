using UnityEngine;

public class TotemShopButton : MonoBehaviour
{
    protected GameManager _gameManager;
    [SerializeField] private GameObject _inventorySlot;
    [SerializeField] protected int _itemCost;
    public Item item;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    public void BuyItem()
    {
        if (_gameManager.coins >= _itemCost && Inventory.instance.AddItem(item) != false)
        {
            _gameManager.coins -= _itemCost;
            Debug.Log("Bought Totem");
            ActivateTotem();
        }
    }

    private void ActivateTotem()
    {
        switch (item)
        {
            case HerdTotem herdTotem:
                _gameManager.mobSpawnMult += herdTotem.mobIncrease;
                break;
            case SizeTotem sizeTotem:
                for (int i = 0; i < sizeTotem.slotsAdded; i++)
                {
                    GameObject addedSlot = Instantiate(_inventorySlot, Inventory.instance.transform);
                    Inventory.instance.inventorySlots.Add(addedSlot.GetComponent<InventorySlot>());
                }
                Inventory.instance.SlotsAdded();
                break;
            default:
                break;
        }
    }
}
