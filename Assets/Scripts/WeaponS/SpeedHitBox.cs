using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedHitBox : MonoBehaviour
{

    public float weaponDamage = 50f;
    public float lastHealth;
    public int playerNumber; //por mientras jsjs

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destructible")
        {
            Debug.Log("speed hit");
            BuildingManager buildingManager = other.gameObject.GetComponent<BuildingManager>();
            if(buildingManager != null) {
                buildingManager.Hit(weaponDamage, playerNumber);
                lastHealth = buildingManager.Health;
            }
        }else if(other.tag == "debri"){
            Debug.Log("speed hit debri");
            other.attachedRigidbody.AddForce(transform.up*40);
        }
    }
}
