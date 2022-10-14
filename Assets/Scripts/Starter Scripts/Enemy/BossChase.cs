using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChase : MonoBehaviour
{
    private Transform targetPlayer;
    public float speed;
    int MaxDist = 15;
    int MinDist = 5;

    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPlayer.position) <= MaxDist)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, speed * Time.deltaTime);
        }

        if (transform.position.x < targetPlayer.position.x)
        {
            transform.localScale = new Vector3(-50, 50, 50);
        }
        else
        {
            transform.localScale = new Vector3(50, 50, 50);
        }
    }
}
