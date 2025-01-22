using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

interface IShoot
{
    public void FireShot(Vector2 shotDir)
    {

    }
}

public enum GameState
{
    InGame,
    InShop
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Image _timerMeter;

    public GameState state;
    public int[] timerTimes = new int[5] { 50, 20, 50, 20, 100 }; // the amount of seconds between level and shop
    public int _onTime; // on which part we are of the timerTimes

    public int coins;

    public int mobSpawnMult = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        StartCoroutine(RoundTimer());
    }

    private void Update()
    {
        _timerMeter.fillAmount += Time.deltaTime / 240;
    }

    private IEnumerator RoundTimer()
    {
        yield return new WaitForSeconds(timerTimes[_onTime]);
        switch (_onTime)
        {
            case 0:
            case 2:
                ToggleShop();
                break;
            case 1:
            case 3:
                ToggleGame();
                break;
            case 4:
                Debug.Log("End game");
                break;
            default:
                break;
        }
        _onTime++;
        if (_onTime != timerTimes.Length) { StartCoroutine(RoundTimer()); }
    }

    private void ToggleShop()
    {
        state = GameState.InShop;
        // PlayerController.instance.gameObject.GetComponent<PlayerInput>().enabled = false;
    }

    private void ToggleGame()
    {
        state = GameState.InGame;
        // PlayerController.instance.gameObject.GetComponent<PlayerInput>().enabled = true;
    }
}
