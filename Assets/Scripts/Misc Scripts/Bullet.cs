using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDir;
    public void Setup(Vector3 shootDir)
    {
        this.shootDir = shootDir.normalized;
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        float moveSpeed = 25f; //bullet speed
        transform.position += shootDir * moveSpeed * Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {


        //Only works for 1 enemy rn
        GameObject enemy_test = GameObject.FindGameObjectWithTag("Enemy");
        GameObject wall_test = GameObject.FindGameObjectWithTag("Terrain");

        if (collider.CompareTag("Enemy")) {
          Destroy(gameObject);
          enemy_test.GetComponent<EnemyHealth>().DecreaseHealth(5);
          collider.GetComponentInChildren<HealthBar>().hp -= 5;
        }
        else if (collider.CompareTag("Terrain")) {
          Destroy(gameObject);
        }

    }
}
