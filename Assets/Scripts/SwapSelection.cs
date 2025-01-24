using UnityEngine;

public class SwapSelection : MonoBehaviour
{
    public static SwapSelection instance;
    public int selectedSlot;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
}
