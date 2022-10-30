using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    // The original idea was to have 2 different mechas but for now that won't be
    private float _playerHealth = 100f;

    private GlobalHealthManager healthManager;

    // Start is called before the first frame update
    void Start()
    {
        healthManager = gameObject.GetComponent<GlobalHealthManager>(); 
        healthManager.Health = _playerHealth;  
    }

    // Update is called once per frame
    void Update()
    {
        if(healthManager.Health <= 0)
        {
            Death();
        }
    }

    // When player is hit by other player or AI
    public void ReceiveDamage(float damage)
    {
        healthManager.Health -= damage;
        Debug.Log(healthManager.Health);
    }

    // Functions that I think we are going to need in the future
    void Death()
    {

    }

    void Respawn()
    {
        
    }
}
