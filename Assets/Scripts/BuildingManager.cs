using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    public float health = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float damage)
    {
        health -= damage;
        if(health <= 0) {
            // Destroy the building
            Destroy(gameObject); 
        }
    }
}
