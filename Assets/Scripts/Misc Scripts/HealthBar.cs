using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image hpImage;
    public Image hpEffectImage;
    [HideInInspector]public float hp;
    public float maxHp;
    public float hurtSpeed = 0.005f;
    public GameObject EnemyEntity;

    private void Start() {
      //hp = maxHp;
      hp = EnemyEntity.GetComponent<EnemyHealth>().maxHealth;
    }

    private void Update() {
      hp = EnemyEntity.GetComponent<EnemyHealth>().currentHealth;
      hpImage.fillAmount = hp/maxHp;

      if (hpEffectImage.fillAmount > hpImage.fillAmount)  {
        hpEffectImage.fillAmount -= hurtSpeed;
      }
      else {
        hpEffectImage.fillAmount = hpImage.fillAmount;
      }
    }
}
