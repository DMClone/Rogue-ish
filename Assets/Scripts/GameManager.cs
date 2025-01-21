using UnityEngine;

interface IShoot
{
    public void FireShot(Vector2 shotDir)
    {

    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int coins;

    public int mobSpawnMult;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
