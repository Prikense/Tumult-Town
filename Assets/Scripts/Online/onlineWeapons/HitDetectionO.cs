using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetectionO : MonoBehaviour
{
    [SerializeField] private AudioSource gunsfx;
    [SerializeField] private AudioClip[] audioClips;// 0 -> swing, 1 -> building hit, 2 -> player hit, 3 -> small rubble
    [SerializeField] private MeleeManagerO meleeManager;
    private float weaponDamage = 30f;
    [SerializeField] private int playerNumber; //por mientras, eliminar despues

    private void OnEnable(){
        gunsfx.PlayOneShot(audioClips[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destructible" && meleeManager.IsAttacking)
        {
            //Debug.Log("melee hit");
            BuildingManagerO buildingManager = other.gameObject.GetComponent<BuildingManagerO>();
            if(buildingManager != null) {
                gunsfx.PlayOneShot(audioClips[1]);
                buildingManager.Hit(weaponDamage, playerNumber);
            }
        }
        if(other.tag == "debri" && meleeManager.IsAttacking){
            //Debug.Log("melee hit");
            gunsfx.PlayOneShot(audioClips[3]);
            other.attachedRigidbody.AddForce(transform.forward*300);
        }
        if(other.tag == "AI"){
            EnemyAI enemy = other.transform.GetComponent<EnemyAI>();
            if(enemy != null) {
                gunsfx.PlayOneShot(audioClips[2]);
                enemy.ReceiveDamage(weaponDamage);
                //enemy doesnt have rigidbodies as of now
                //other.attachedRigidbody.AddForce(transform.forward*300);
            }
        }
        if(other.tag == "Player"){
            PlayerManagerO playerHealth = other.transform.GetComponent<PlayerManagerO>();
            if(playerHealth != null) {
                gunsfx.PlayOneShot(audioClips[2]);
                playerHealth.ReceiveDamage(weaponDamage/10);
                other.attachedRigidbody.AddForce(transform.forward*500);
            }
        }
    }
}
