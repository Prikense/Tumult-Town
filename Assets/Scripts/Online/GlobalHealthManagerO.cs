using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GlobalHealthManagerO : NetworkBehaviour
{

    // [Networked] [SerializeField] private float NetHeatlh{get;set;}
    
    /*
    private float _health;
    // [Networked(OnChanged = nameof(OnHPChanged))]
    public float Health
    {
        get{return NetHeatlh;}
        set{_health = value;}
    }
    */

    private float _health;
    [Networked(OnChanged = nameof(OnHPChanged))]
    public float Health
    {
        get{return _health;}
        set{_health = value;}
    }

    private float _maxHealth;
    public float MaxHealth
    {
        get{return _maxHealth;}
        set{_maxHealth = value;}
    }

    private float _healthRatio;
    public float HealthRatio
    {
        get{return _health/_maxHealth;}
        set{_healthRatio = value;}
    }

    public override void Spawned()
    {
        if(Object.Runner.IsServer){
           // NetHeatlh = _health;
        }
    }

    // Function only called on the server
    public void OnTakeDamage(float damage)
    {
        // Only take damage while alive
        if(Health <= 0){
            // return;
        }
        Health -= damage;
    }

    public override void FixedUpdateNetwork()
    {
        if(Object.Runner.IsServer){
           // NetHeatlh = _health;
        }
    }

    
    static void OnHPChanged(Changed<GlobalHealthManagerO> changed)
    {

    }

}
