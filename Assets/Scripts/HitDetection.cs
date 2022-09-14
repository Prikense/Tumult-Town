using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{

    public MeleeManager meleeManager;
    public float weaponDamage = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destructible" && meleeManager.isAttacking)
        {
            Debug.Log("melee hit");
            BuildingManager buildingManager = other.gameObject.GetComponent<BuildingManager>();
            if(buildingManager != null) {
                buildingManager.Hit(weaponDamage);
            }
        }
    }
}
