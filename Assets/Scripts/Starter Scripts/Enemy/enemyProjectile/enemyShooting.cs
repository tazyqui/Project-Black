using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShooting : MonoBehaviour
{
    private float breakTime;
    private Transform targetPlayer;
    public float startTime;
    public GameObject projectile;
    int MaxDist = 25;
    int MinDist = 1;
    // Start is called before the first frame update
    void Start()
    {
        breakTime = startTime;
        targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

      if (Vector3.Distance(transform.position, targetPlayer.position) <= MaxDist)
      {
        if(breakTime<=0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            breakTime = startTime;
        }
        else
        {
            breakTime -= Time.deltaTime;
        }
      }

    }
}
