﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //This class should be placed on anything enemy related! Or anything that the player can damage
    public int maxHealth = 100;

    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void DecreaseHealth(int value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DamageDealer damageValues))
        {
            if (damageValues.damageType == DamageDealer.DamageType.Player)
            {
                DecreaseHealth(damageValues.DamageValue);
                if (currentHealth == 0)
                {
                    Destroy(this.gameObject);//If this enemy reaches 0 health, they are straight up destroyed. 
                    //If you want something fancy like an animation or the like, you can try to implement it here
                }
            }
        }
    }
}
