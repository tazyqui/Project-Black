using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Weapons")]
    [Tooltip("This is the list of all the weapons that your player uses")]
    public List<Weapon> weaponList;
    [Tooltip("This is the current weapon that the player is using")]
    public Weapon weapon;
    private Vector2 lastKnownDirection;
    [Tooltip("The coolDown before you can attack again")]
    public float coolDown = 0.4f;

    private bool canAttack = true;

    private void Start()
    {
        if (weapon == null && weaponList.Count > 0)
        {
            weapon = weaponList[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))//Here is where you can hit the "1" key on your keyboard to activate this weapon
        {
            if (weaponList.Count > 0)
            {
                switchWeaponAtIndex(0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))//Remove this if you don't have multiple weapons
        {
            if (weaponList.Count > 1)
            {
                switchWeaponAtIndex(1);
            }
        }
    }

    public void Attack(float xDirection, float yDirection)
    {
        //This is where the weapon is rotated in the right direction that you are facing
        if (weapon)
        {
            if (xDirection != 0 || yDirection != 0)
            {
                weapon.transform.rotation = transform.rotation;
                lastKnownDirection = new Vector2(xDirection, yDirection);
                weapon.transform.Rotate(0,0,Mathf.Atan2(lastKnownDirection.y, lastKnownDirection.x) * Mathf.Rad2Deg);
                //Debug.Log(weapon.transform.eulerAngles.z);
                if (weapon.flipWeapon)//Rotation before scale, always.
                {
                    weapon.transform.Rotate(0, 0, 180);
                }
                if (Mathf.Abs(weapon.transform.eulerAngles.z) > 90 && weapon.transform.eulerAngles.z < 270)
                {
                    weapon.transform.localScale = new Vector3(weapon.transform.localScale.x, Mathf.Abs(weapon.transform.localScale.y) * -1, weapon.transform.localScale.z);
                }
                else
                {
                    weapon.transform.localScale = new Vector3(weapon.transform.localScale.x, Mathf.Abs(weapon.transform.localScale.y), weapon.transform.localScale.z);
                }
            }

            if (weapon.weaponType == Weapon.WeaponType.Projectile)
            {
                //TODO do the spawn thingy
                if (canAttack && !weapon.isOnlyOneProjectile)
                {
                    Weapon weapon2 = Instantiate(weapon, transform);
                    weapon2.WeaponAttack();
                    canAttack = false;
                    StartCoroutine(CoolDown());
                }
                else if (canAttack)
                {
                    weapon.WeaponAttack();
                }
                //Weapon weapon2 = Instantiate(weapon);
                //weapon2.WeaponAttack();
            }
            else
            {
                weapon.WeaponAttack();
            }
            //weapon.WeaponAttack();
        }
    }

    public void StopAttack()
    {
        if (weapon)
        {
            weapon.WeaponFinished();
        }
    }

    public void switchWeaponAtIndex(int index)
    {
        //Makes sure that if the index exists, then a switch will occur
        if (index < weaponList.Count && weaponList[index])
        {
            //Deactivate current weapon
            weapon.gameObject.SetActive(false);

            //Switch weapon to index then activate
            weapon = weaponList[index];
            weapon.gameObject.SetActive(true);
        }

    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolDown);
        canAttack = true;
    }
}
