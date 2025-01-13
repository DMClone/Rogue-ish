using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Sprite _slotSprite;
    public int slotSelected;
    public int slots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TestSwitch());
    }

    IEnumerator TestSwitch()
    {
        yield return new WaitForSeconds(2f);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
