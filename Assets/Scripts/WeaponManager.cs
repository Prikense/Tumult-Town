using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public GameObject playerCamera;
    public float range = 100f;
    public float damage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            Debug.Log("Pew");
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if(Physics.Raycast(playerCamera.transform.position, transform.forward, out hit, range)) {
            Debug.Log("Boom");

            BuildingManager buildingManager = hit.transform.GetComponent<BuildingManager>();
            if(buildingManager != null) {
                buildingManager.Hit(damage);
            }
        }
    }
}
