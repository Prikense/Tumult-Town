using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GlobalHealthManagerO : NetworkBehaviour
{

    [Networked] [SerializeField] private float NetHeatlh{get;set;}
    private float _health;
    public float Health
    {
        get{return NetHeatlh;}
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
           NetHeatlh = _health;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(Object.Runner.IsServer){
           NetHeatlh = _health;
        }
    }

}
