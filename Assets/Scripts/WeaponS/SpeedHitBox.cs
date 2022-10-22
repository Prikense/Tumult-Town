using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedHitBox : MonoBehaviour
{

    public float weaponDamage = 1;
    [SerializeField] private float DmgMultiplier = 5;
    public float lastHealth;
    public int playerNumber; //por mientras jsjs
    [SerializeField] private FPSMoveRigidBody playerStuff; 


    void FixedUpdate(){
        weaponDamage = Mathf.Max(playerStuff.velocity*DmgMultiplier, DmgMultiplier);
    }

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
            playerStuff.body.AddForce(-8*(transform.forward));
        }else if(other.tag == "debri"){
            Debug.Log("speed hit debri");
            other.attachedRigidbody.AddForce(transform.up*40);
        }
    }
}
