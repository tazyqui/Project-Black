using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Melee,
        Projectile
    }
    public WeaponType weaponType = WeaponType.Melee;

    private float startTime;
    private bool projectileStart = false;
    private Vector3 startPosition;
    [Tooltip("The hit box of the weapon (This MUST have a collider on it to work)")]
    public GameObject weaponHitBox;
    [Tooltip("The speed of the projectile if this is a projectile weapon")]
    public float projectileSpeed = 5f;

    [Tooltip("Use this to flip the weapon if its trailing behind you for some reason")]
    public bool flipWeapon = false;

    [Tooltip("This is if you only want one projectile to be active at one time, like a boomerang or something")]
    public bool isOnlyOneProjectile = false;


    [Tooltip("How long the projectile is allowed to travel")]
    public float projectileLifeTime = 2.0f;
    // Start is called before the first frame update

    void Update()
    {
        if (projectileStart)
        {
            transform.position += (-1 * transform.right) * Time.deltaTime * projectileSpeed;
            if (Time.time - startTime >= projectileLifeTime)
            {
                projectileStart = false;
                transform.position = startPosition;
                weaponHitBox.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

    public void WeaponAttack()
    {
        if (weaponType == WeaponType.Melee)
        {
            weaponHitBox.SetActive(true);
        }
        else if (weaponType == WeaponType.Projectile)
        {
            weaponHitBox.SetActive(true);
            startTime = Time.time;
            projectileStart = true;
            startPosition = Vector3.zero;
            transform.localPosition = startPosition;
        }

    }

    public void WeaponFinished()
    {
        if (weaponType == WeaponType.Melee)
        {
            weaponHitBox.SetActive(false);
        }
    }
}
