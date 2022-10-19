using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public int playerNumber;
    public GameObject playerCamera;
    public float range = 100f;
    public float damage = 10f;
    public ParticleSystem muzzleFlash;
    public float fireRate = 15f;
    public int magazineSize = 35;
    public int ammoLeft;
    public float spread = 0.001f;
    public float reloadTime = 1.0f;
    public bool reloading;
    public float impactForce = 155f;

    // used to updat ui after reloaded
    public bool doneReloading;

    public bool isShooting;
    public float lastHealth = 0;

    private float nextTimeToFire = 0f;

    public GameObject bulletHole;

    // Start is called before the first frame update
    void Start()
    {
        doneReloading = false;
        reloading = false;
        ammoLeft = magazineSize;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time >= nextTimeToFire && ammoLeft > 0) {
            nextTimeToFire = Time.time + 1f/fireRate;
            isShooting = true;
            Shoot();
        }
        else {
            isShooting = false;
        }

        if(Input.GetKeyDown(KeyCode.R) && !reloading) {
            Reload();
        }
    }

    void Shoot()
    {
        // Spread in x and y axis
        float xSpread = Random.Range(-spread, spread);
        float ySpread = Random.Range(-spread, spread);
        float zSpread = Random.Range(-spread, spread);


        // Testing (this works)
        Vector3 deviation3D = Random.insideUnitCircle * spread;

        Quaternion rot = Quaternion.LookRotation(Vector3.forward * range + deviation3D);

        Vector3 forwardVector = playerCamera.transform.rotation * rot * Vector3.forward;

        // Caculate direction with added spread, not working
        Vector3 direction = playerCamera.transform.forward + new Vector3(xSpread, ySpread, zSpread); //also doesnt work
        direction.Normalize(); //this doesnt work

        muzzleFlash.Play();

        ammoLeft -= 1;

        RaycastHit hit;

        if(Physics.Raycast(playerCamera.transform.position, forwardVector, out hit, range)) {

            GameObject impactGO = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.3f);

            BuildingManager buildingManager = hit.transform.GetComponent<BuildingManager>();
            if(buildingManager != null) {
                buildingManager.Hit(damage, playerNumber);
                lastHealth = buildingManager.healthRatio;
            }

            if(hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }

    private void Reload() 
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
        doneReloading = false;
    }

    private void ReloadFinished()
    {
        ammoLeft = magazineSize;
        reloading = false;
        doneReloading = true;
    }
}
