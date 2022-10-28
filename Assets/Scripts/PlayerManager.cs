using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    // The original idea was to have 2 different mechas but for now that won't be
    private float _playerHealth = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerHealth <= 0)
        {
            Death();
        }
    }

    // When player is hit by other player or AI
    public void ReceiveDamage(float damage)
    {
        _playerHealth -= damage;
        Debug.Log(_playerHealth);
    }

    // Functions that I think we are going to need in the future
    void Death()
    {

    }

    void Respawn()
    {
        
    }
}
