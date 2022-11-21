using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{

    [SerializeField] private MeleeManager meleeManager;
    private float weaponDamage = 30f;
    [SerializeField] private int playerNumber; //por mientras, eliminar despues

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destructible" && meleeManager.IsAttacking)
        {
            //Debug.Log("melee hit");
            BuildingManager buildingManager = other.gameObject.GetComponent<BuildingManager>();
            if(buildingManager != null) {
                buildingManager.Hit(weaponDamage, playerNumber);
            }
        }
        if(other.tag == "debri" && meleeManager.IsAttacking){
            //Debug.Log("melee hit");
            other.attachedRigidbody.AddForce(transform.forward*300);
        }
        if(other.tag == "Player"){
            PlayerManager playerHealth = other.transform.GetComponent<PlayerManager>();
            if(playerHealth != null) {
                playerHealth.ReceiveDamage(weaponDamage/10);
                other.attachedRigidbody.AddForce(transform.forward*500);
            }
        }
    }
}
