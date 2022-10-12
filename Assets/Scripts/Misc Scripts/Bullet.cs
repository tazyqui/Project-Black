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

        Destroy(gameObject);

        //Only works for 1 enemy rn
        GameObject enemy_test = GameObject.FindGameObjectWithTag("Enemy");
        Collider2D enemy_collider = enemy_test.GetComponent<Collider2D>();

        if (enemy_test != null)
        {
            //Hit a target
            enemy_test.GetComponent<EnemyHealth>().DecreaseHealth(5);

        }

    }
}
