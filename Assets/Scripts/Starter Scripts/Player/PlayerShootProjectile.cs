using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{
    [SerializeField] private Transform pfBullet;


    private void Awake()
    {
        GetComponent<PlayerAimWeapon>().OnShoot += PlayerShootProjectile_OnShoot;
    }

    private void PlayerShootProjectile_OnShoot(object sender, PlayerAimWeapon.OnShootEventArgs e)
    {
        //Shoot
        Transform bulletTransform = Instantiate(pfBullet, e.gunEndPointPosition, Quaternion.identity);
        gameObject.transform.GetChild(4).gameObject.GetComponent<AudioSource>().Play();

        Vector3 shootDir = (e.shootPosition - e.gunEndPointPosition).normalized;
        //shootDir.z = 0;
        bulletTransform.GetComponent<Bullet>().Setup(shootDir);
    }
}
