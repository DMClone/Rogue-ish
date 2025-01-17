using UnityEngine;

interface IShoot
{
    [SerializeField] public GameObject _bulletPrefab { get; set; }
    public void FireShot(Vector2 shotDir)
    {

    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
