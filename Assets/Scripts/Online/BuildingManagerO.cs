using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BuildingManagerO : NetworkBehaviour
{

    [SerializeField] private GameObject shatteredBuilding;
    // fix for the presentation (check if still neede if not erase)
    [SerializeField] private bool needsPosFix = false;

   
    [SerializeField] private float _buildingHealth = 100;
    //private float maxHealth;

   
    private GlobalHealthManagerO healthManager;

    [SerializeField] private int _value = 5;
    public int Value
    {
        get{return _value;}
        set{_value = value;}
    }

    [SerializeField] private ScoreScript scoreboard;

    public override void Spawned()
    {
        healthManager = gameObject.GetComponent<GlobalHealthManagerO>(); 
        if(healthManager.Health == 0){
            healthManager.Health = _buildingHealth;
            healthManager.MaxHealth = healthManager.Health;
        }
    }

    public void Hit(float damage, int player)
    {
        /*
        Health -= damage;
        HealthRatio = Health / maxHealth;
        */
        healthManager.Health -= damage;
        //healthManager.HealthRatio = healthManager.Health / healthManager.MaxHealth;

        if(healthManager.Health <= 0) {
            // Destroy the building
            Vector3 posFix = new Vector3(0f, 0f, 0f);
            if(needsPosFix) posFix = new Vector3(2.9f, 1f, 0f);
            //GameObject ruins = Instantiate(shatteredBuilding, transform.position + posFix, transform.rotation);
            GameObject ruins = Instantiate(shatteredBuilding, transform);
            transform.DetachChildren();
            Destroy(ruins,5);
            //gameObject.active = false;
            Runner.Despawn(Object);
            //ScoreScript scoreboard = gameObject.GetComponent<scoreManager>();
            if(player == 1){
                scoreboard.Player1Score += Value;
            }else if(player == 2){
                scoreboard.Player2Score += Value;
            }
        }
    }
}
