using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Gun")]
public class Gun : Item
{
    public float fireRate;
    public int bulletsPerShot;
    public int damagePetBullet;
}
