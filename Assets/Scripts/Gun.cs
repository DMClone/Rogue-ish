using UnityEngine;

[CreateAssetMenu(menuName = "Items/Gun")]
public class Gun : Item
{
    public AudioClip gunSound;
    public UseType useType;

    [Header("Gun specifics")]
    public int bulletsPerShot;
    public int damagePetBullet;
    public float fireRate;
}
