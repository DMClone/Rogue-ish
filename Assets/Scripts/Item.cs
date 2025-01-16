using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [Header("Image")]
    public Sprite image;

    [Header("Main")]
    public bool isStackable = true;
    public bool isUsable = true;
}

public enum UseType
{
    InfiniteUse,
    ConsumeOnUse
}
