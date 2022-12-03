using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{

    [SerializeField] private AudioClip[] audioClips;// 0 -> building hit, 1 -> player hit, 2 -> ai hit

    private float _weaponDamage = 30f;
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
                AudioSource.PlayClipAtPoint(audioClips[0], Vector3.zero);//use collision.transform.position for 3d sound
                buildingManager.Hit(WeaponDamage, PlayerNumber);
                //Debug.Log("ouch");
            }
        }
        if(collision.gameObject.tag == "Player"){
            PlayerManager playerHealth = collision.transform.GetComponent<PlayerManager>();
            if(playerHealth != null) {
                AudioSource.PlayClipAtPoint(audioClips[1], Vector3.zero);
                playerHealth.ReceiveDamage(WeaponDamage/10);
            }
        }
        if(collision.gameObject.tag == "AI"){
            AudioSource.PlayClipAtPoint(audioClips[2], Vector3.zero);
            EnemyAI enemy = collision.transform.GetComponent<EnemyAI>();
            if(enemy != null) {
                enemy.ReceiveDamage(WeaponDamage);
            }
        }
        Destroy(gameObject, 3);
    }

    void Awake()
    {
        Destroy(gameObject, 6);
    }

}