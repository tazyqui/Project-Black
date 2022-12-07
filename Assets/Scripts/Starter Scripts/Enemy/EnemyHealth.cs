using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //This class should be placed on anything enemy related! Or anything that the player can damage
    public int maxHealth = 100;

    public int currentHealth;
    public AudioClip deathSound1, deathSound2;
    [HideInInspector]public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GameObject.FindWithTag("SkeeterDeath").GetComponent<AudioSource>();
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

                  int random = UnityEngine.Random.Range(0, 2);
                  if (random == 0) {
                    audioSource.PlayOneShot(deathSound1);
                  }
                  else {
                    audioSource.PlayOneShot(deathSound2);
                  }

                  Destroy(this.gameObject);//If this enemy reaches 0 health, they are straight up destroyed.
                    //If you want something fancy like an animation or the like, you can try to implement it here
                  //audioSource.PlayOneShot(deathSound1);






                }
            }
        }
    }
}
