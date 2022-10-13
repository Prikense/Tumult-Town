using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    public GameObject shatteredBuilding;
    // fix for the presentation
    public bool needsPosFix = false;

    public float health = 100f;
    public int value = 5;

    public ScoreScript scoreboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Melee"){
            Instantiate(shatteredBuilding, this.transform.position, this.transform.rotation);
        }
    }
    */

    public void Hit(float damage, int player)
    {
        health -= damage;
        if(health <= 0) {
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
                scoreboard.player1Score += value;
            }else if(player == 2){
                scoreboard.player2Score += value;
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
