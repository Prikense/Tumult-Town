using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{


    [SerializeField] private MeleeManager meleeManager;
    private float weaponDamage = 20f;
    //public float lastHealth;
    [SerializeField] private int playerNumber; //por mientras, eliminar después

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destructible" && meleeManager.IsAttacking)
        {
            Debug.Log("melee hit");
            BuildingManager buildingManager = other.gameObject.GetComponent<BuildingManager>();
            if(buildingManager != null) {
                buildingManager.Hit(weaponDamage, playerNumber);
                //lastHealth = buildingManager.Health;
            }
        }
    }
}
