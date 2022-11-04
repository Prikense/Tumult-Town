using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHealthManager : MonoBehaviour
{

    private float _health;
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
}
