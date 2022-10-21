using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] private int playerNumber;
    [SerializeField] private GameObject playerCamera;
    private float range = 100f;
    private float damage = 10f;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private float fireRate = 15f;

    [SerializeField] private int _magazineSize = 350;
    public int MagazineSize
    {
        get{return _magazineSize;}
        set{_magazineSize = value;}
    }

    private int _ammoLeft;
    public int AmmoLeft
    {
        get{return _ammoLeft;}
        set{_ammoLeft = value;}
    }

    [SerializeField] private float spread = 0.001f;
    [SerializeField] private float reloadTime = 1.0f;
    private bool reloading;
    [SerializeField] private float impactForce = 155f;

    // used to update ui after reloaded
    private bool _doneReloading;
    public bool DoneReloading
    {
        get{return _doneReloading;}
        set{_doneReloading = value;}
    }

    private bool _isShooting;
    public bool IsShooting
    {
        get{return _isShooting;}
        set{_isShooting = value;}
    }

    private float nextTimeToFire = 0f;

    [SerializeField] private GameObject bulletHole;

    // Start is called before the first frame update
    void Start()
    {
        DoneReloading = false;
        reloading = false;
        AmmoLeft = MagazineSize;
        IsShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && Time.time >= nextTimeToFire && AmmoLeft > 0) {
            nextTimeToFire = Time.time + 1f/fireRate;
            IsShooting = true;
            //Shoot();
        }
        else {
            IsShooting = false;
        }

        if(Input.GetKeyDown(KeyCode.R) && !reloading) {
            Reload();
        }
    }

    void FixedUpdate()
    {
        if(IsShooting)
        {
            Shoot();
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

        // Caculate direction with added spread, not working
        Quaternion rot = Quaternion.LookRotation(Vector3.forward * range + deviation3D);
        Vector3 forwardVector = playerCamera.transform.rotation * rot * Vector3.forward;

        muzzleFlash.Play();

        AmmoLeft -= 1;

        RaycastHit hit;

        Vector3 raycastOrigin = new Vector3 (playerCamera.transform.position.x - 0.0f, playerCamera.transform.position.y, playerCamera.transform.position.z+.05f);

        if(Physics.Raycast(raycastOrigin, forwardVector, out hit, range, ~LayerMask.GetMask("Debri"))) {

            GameObject impactGO = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.3f);

            BuildingManager buildingManager = hit.transform.GetComponent<BuildingManager>();
            if(buildingManager != null) {
                buildingManager.Hit(damage, playerNumber);
            }

            EnemyAI enemyAI = hit.transform.GetComponent<EnemyAI>();
            if(enemyAI != null)
            {
                enemyAI.ReceiveDamage(damage);
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
        DoneReloading = false;
    }

    private void ReloadFinished()
    {
        AmmoLeft = MagazineSize;
        reloading = false;
        DoneReloading = true;
    }
}
