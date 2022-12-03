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

    [SerializeField] private ScoreScriptO scoreboard;
    private int lastPlayerHit=0;

    public override void Spawned()
    {
        healthManager = gameObject.GetComponent<GlobalHealthManagerO>(); 
        healthManager.MaxHealth = _buildingHealth;
        if(healthManager.Health == 0){
            healthManager.Health = _buildingHealth;
        }
    }

    public void Hit(float damage, int player)
    {
        /*
        Health -= damage;
        HealthRatio = Health / maxHealth;
        */
        healthManager.Health -= damage;
        lastPlayerHit=player;
        //healthManager.HealthRatio = healthManager.Health / healthManager.MaxHealth;

    }
    public override void FixedUpdateNetwork()
    {
        if(healthManager.Health <= 0) {
            // Destroy the building
            Vector3 posFix = new Vector3(0f, 0f, 0f);
            if(needsPosFix) posFix = new Vector3(2.9f, 1f, 0f);
            //GameObject ruins = Instantiate(shatteredBuilding, transform.position + posFix, transform.rotation);
            
            //gameObject.active = false;
            //ScoreScript scoreboard = gameObject.GetComponent<scoreManager>();
            if(Object.Runner.IsServer){
                if(lastPlayerHit == 1){
                    scoreboard.Player1Score += Value;
                }else if(lastPlayerHit == 2){
                    scoreboard.Player2Score += Value;
                }
            }
            Runner.Despawn(Object);
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        GameObject ruins = Instantiate(shatteredBuilding, transform);
        transform.DetachChildren();
        Destroy(ruins,5);
    }
}
