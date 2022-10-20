using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{

    public float weaponDamage = 5f;
    public float lastHealth;
    public int playerNumber; //por mientras jsjs

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Destructible"){
            BuildingManager buildingManager = collision.gameObject.GetComponent<BuildingManager>();
            if(buildingManager != null) {
                buildingManager.Hit(weaponDamage, playerNumber);
                lastHealth = buildingManager.Health;
                Debug.Log("ouch");
            }
        }
        Destroy(gameObject);
    }

    void Awake()
    {
        Destroy(gameObject, 6);
    }

}