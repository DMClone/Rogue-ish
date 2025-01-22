using DG.Tweening;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void MoveShopIn()
    {
        transform.DOLocalMoveY(0, 1);
    }

    public void MoveShopAway()
    {
        transform.DOLocalMoveY(155, 1);
    }
}
