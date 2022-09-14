using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    public GameObject shatteredBuilding;
    // fix for the presentation
    public bool needsPosFix = false;

    public float health = 100f;

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

    public void Hit(float damage)
    {
        health -= damage;
        if(health <= 0) {
            // Destroy the building
            Vector3 posFix = new Vector3(0f, 0f, 0f);
            if(needsPosFix) posFix = new Vector3(2.9f, 1f, 0f);
            GameObject ruins = Instantiate(shatteredBuilding, transform.position + posFix, transform.rotation);
            Destroy(ruins, 5);
            Destroy(gameObject); 
        }
    }


    IEnumerator DestroyRemainings(GameObject instance)
    {
        Debug.Log("Bye bye");
        yield return new WaitForSeconds(5f);
        Destroy(instance);
    }
}
