using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//Script Credit: CodeMonkey
public class PlayerAimWeapon : MonoBehaviour
{
    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }

    private Transform aimTransform;
    private Transform aimGunEndPointTransform;
    private Transform playerTransform;

    public float fireRate = 2f;
    private float timeStamp;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        playerTransform = gameObject.transform;
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
    }
    // Update is called once per frame
    private void Update()
    {
        aimTransform = transform.Find("Aim");
        playerTransform = gameObject.transform;
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
        HandleCrosshair();
        HandleAiming();
        HandleShooting();
        //gameObject.transform.GetChild(4).gameObject.GetComponent<AudioSource>().Play();
    }

    private void HandleCrosshair()
    {
        Cursor cursor;
    }

    private void HandleAiming()
    {
        Vector3 aimMousePosition = GetMouseWorldPosition(Input.mousePosition, Camera.main);

        Vector3 aimDirection = (aimMousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        //Debug.Log(angle);

        HandlePlayerOrientation(angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.y = +1f;
        }
        aimTransform.localScale = aimLocalScale;

    }

    private Vector3 GetMouseWorldPosition(Vector3 ScreenPosition, Camera worldCamera)
    {
        Vector3 vec = worldCamera.ScreenToWorldPoint(ScreenPosition);
        vec.z = 0f;
        return vec;
    }

    private void HandlePlayerOrientation(float angle)
    {
        Vector3 playerRotation = playerTransform.eulerAngles;

        if (angle > 90 || angle < -90)
        {
            playerRotation.y = 180f;
        }
        else
        {
            playerRotation.y = 0f;
        }
        playerTransform.eulerAngles = playerRotation;
    }

    private void HandleShooting()
    {
        Collider2D playerCollider = gameObject.GetComponent<Collider2D>();

        Vector3 mousePosition = GetMouseWorldPosition(Input.mousePosition, Camera.main);

        float firingCooldown = 1/fireRate;

        if (Input.GetMouseButton(0) && !playerCollider.OverlapPoint(mousePosition) && timeStamp <= Time.time)
        {
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                gunEndPointPosition = aimGunEndPointTransform.position,
                shootPosition = mousePosition,
            });

            timeStamp = Time.time + firingCooldown;
        }
    }




}
