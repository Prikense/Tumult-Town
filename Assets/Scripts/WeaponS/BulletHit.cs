using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{

    private float _weaponDamage = 5f;
    public float WeaponDamage
    {
        get{return _weaponDamage;}
        set{_weaponDamage = value;}
    }

    private int _playerNumber;
    public int PlayerNumber
    {
        get{return _playerNumber;}
        set{_playerNumber = value;}
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Destructible"){
            BuildingManager buildingManager = collision.gameObject.GetComponent<BuildingManager>();
            if(buildingManager != null) {
                buildingManager.Hit(WeaponDamage, PlayerNumber);
                //Debug.Log("ouch");
            }
        }
        if(collision.gameObject.tag == "Player"){
            PlayerManager playerHealth = collision.transform.GetComponent<PlayerManager>();
            if(playerHealth != null) {
                playerHealth.ReceiveDamage(WeaponDamage/10);
            }
        }
        if(collision.gameObject.tag == "AI"){
            EnemyAI enemy = collision.transform.GetComponent<EnemyAI>();
            if(enemy != null) {
                enemy.ReceiveDamage(WeaponDamage);
            }
        }
        Destroy(gameObject);
    }

    void Awake()
    {
        Destroy(gameObject, 6);
    }

}