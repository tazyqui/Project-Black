using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private Transform targetPlayer;
    public float speed;

    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, speed * Time.deltaTime);

        if (transform.position.x < targetPlayer.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
