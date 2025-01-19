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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
