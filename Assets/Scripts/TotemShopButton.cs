using UnityEngine;

public class TotemShopButton : MonoBehaviour
{
    protected GameManager _gameManager;
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
            default:
                break;
        }
    }
}
