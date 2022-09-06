using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public GameObject playerCamera;
    public float range = 100f;
    public float damage = 10f;
    public ParticleSystem muzzleFlash;
    public float fireRate = 15f;
    public int magazineSize = 35;
    public int bulletsLeft;
    public float spread = 5f;
    public float reloadTime = 2.5f;
    bool reloading;

    private float nextTimeToFire = 0f;

    public GameObject bulletHole;

    // Start is called before the first frame update
    void Start()
    {
        reloading = false;
        bulletsLeft = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time >= nextTimeToFire && bulletsLeft > 0) {
            nextTimeToFire = Time.time + 1f/fireRate;
            Debug.Log("Pew");
            Shoot();
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

        // Caculate direction with added spread, not working
        // Vector3 direction = playerCamera.transform.forward + new Vector3(xSpread, ySpread, 0);

        muzzleFlash.Play();

        bulletsLeft -= 1;

        RaycastHit hit;

        if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range)) {
            Debug.Log("Boom");

            GameObject impactGO = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);

            BuildingManager buildingManager = hit.transform.GetComponent<BuildingManager>();
            if(buildingManager != null) {
                buildingManager.Hit(damage);
            }
        }
    }

    private void Reload() 
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
