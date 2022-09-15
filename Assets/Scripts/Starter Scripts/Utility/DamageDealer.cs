using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    //This component is supposed to be put on anything that deals damage, but it also needs a collider
    [Tooltip("This is how much damage this object should do")]
    public int DamageValue = 0;

    public enum DamageType//You can add another type here if you want another damamge source
    {
        Player,
        Enemy,
        Environment
    }

    [Tooltip("This is the damage source")]
    public DamageType damageType = DamageType.Enemy;
}
