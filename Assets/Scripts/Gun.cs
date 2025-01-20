using UnityEngine;

[CreateAssetMenu(menuName = "Items/Gun")]
public class Gun : Item
{
    public AudioClip gunSound;
    public UseType useType;

    [Header("Gun specifics")]
    public GameObject bulletPrefab;
    public int bulletsPerShot;
    public float fireRate;
    public float spread;
    public int damagePetBullet;

    [Header("Screenshake/Rumble Settings")]
    [Range(0, 1)] public float screenshakeStrength;
    [Range(0, 1)] public float rumbleLeft;
    [Range(0, 1)] public float rumbleRight;
    [Range(0, 1)] public float rumbleDuration;
}
