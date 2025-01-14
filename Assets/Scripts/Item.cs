using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    [Header("Image")]
    public Sprite image;

    [Header("Gameplay")]
    public bool isStackable = true;
    public bool isUsable = true;
    [Range(0, 2)] public float useTime = 0f;
    public UseType useType;
}

public enum UseType
{
    InfiniteUse,
    ConsumeOnUse
}
