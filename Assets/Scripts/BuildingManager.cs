using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    [SerializeField] private GameObject shatteredBuilding;
    // fix for the presentation
    [SerializeField] private bool needsPosFix = false;

    [SerializeField] private float _health = 100f;
    public float Health 
    {
        get{return _health;}
        set{_health = value;}
    }

    private float maxHealth;

    private float _healthRatio;
    public float HealthRatio
    {
        get{return _healthRatio;}
        set{_healthRatio = value;}
    }

    private int _value = 5;
    public int Value
    {
        get{return _value;}
        set{_value = value;}
    }

    [SerializeField] private ScoreScript scoreboard;

    void Start()
    {
        maxHealth = Health;
        HealthRatio = Health / maxHealth;
    }

    public void Hit(float damage, int player)
    {
        Health -= damage;
        HealthRatio = Health / maxHealth;

        if(Health <= 0) {
            // Destroy the building
            Vector3 posFix = new Vector3(0f, 0f, 0f);
            if(needsPosFix) posFix = new Vector3(2.9f, 1f, 0f);
            //GameObject ruins = Instantiate(shatteredBuilding, transform.position + posFix, transform.rotation);
            GameObject ruins = Instantiate(shatteredBuilding, transform);
            transform.DetachChildren();
            Destroy(ruins,5);
            //gameObject.active = false;
            Destroy(gameObject);
            //ScoreScript scoreboard = gameObject.GetComponent<scoreManager>();
            if(player == 1){
                scoreboard.Player1Score += Value;
            }else if(player == 2){
                scoreboard.Player2Score += Value;
            }
        }
    }


    IEnumerator DestroyRemainings(GameObject instance)
    {
        Debug.Log("Bye bye");
        yield return new WaitForSeconds(5f);
        Destroy(instance);
    }
}
