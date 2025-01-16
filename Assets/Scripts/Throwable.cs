using UnityEngine;

[CreateAssetMenu(menuName = "Items/Throwable")]
public class Throwable : Item
{
    public AudioClip gunSound;
    public UseType useType;

    [Header("Throwable Specifics")]
    public float explosionDamage;
    public float explosionRadius;
    public float throwVelocity;
    public float cooldown;
}
