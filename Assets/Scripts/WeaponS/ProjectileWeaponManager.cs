using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponManager : MonoBehaviour
{

    // bullet
    [SerializeField] private GameObject bullet;

    // bullet force
    [SerializeField] private float shootForce, upwardForce;

    // gun stats
    [SerializeField] private float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    [SerializeField] private int bulletsPerTap;

    private int _magazineSize = 60;
    public int MagazineSize
    {
        get{return _magazineSize;}
        set{_magazineSize = value;}
    }

    private bool allowButtonHold;
    private int bulletsShot;

    private int _ammoLeft;
    public int AmmoLeft
    {
        get{return _ammoLeft;}
        set{_ammoLeft = value;}
    } 

    // bools
    private bool readyToShoot, reloading;

    private bool _shooting;
    public bool Shooting
    {
        get{return _shooting;}
        set{_shooting = value;}
    }

    private bool _doneReloading;
    public bool DoneReloading
    {
        get{return _doneReloading;}
        set{_doneReloading = value;}
    }

    // references
    public Camera fpsCam;
    public Transform attackPoint;

    // bug fixing?
    public bool allowInvoke = true;

    private void Awake() 
    {
        // make sure magazine is full and is able to shoot
        AmmoLeft = MagazineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        // check if it is allowed to hold down button and take corresponding input 
        if(allowButtonHold) Shooting = Input.GetKey(KeyCode.Mouse0);
        else Shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // check if player wants to reload
        if(Input.GetKeyDown(KeyCode.R) && !reloading) Reload();
    }

    void FixedUpdate()
    {
        // shooting
        if(readyToShoot && Shooting && !reloading && AmmoLeft > 0) 
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // find the excat position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //A ray through the middle of your screen
        RaycastHit hit;

        // check if the raycast hits something
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit)) {
            targetPoint = hit.point;
        } else {
            targetPoint = ray.GetPoint(100); // a pont far away
        }

        // calculate direction from attack point to target point
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // calculate spread
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);

        // calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(spreadX, spreadY, 0);

        // instantiate bullet or projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        // rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        // adding force to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        // this one is only necessary when using proyectiles like granades
        // currentBullet.GetComponent<Rigidbody>.AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        AmmoLeft--;
        bulletsShot++;

        // invoke resetShot function (if not already invoked)
        if(allowInvoke) {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        // if more than one bullet per tap make sure to repeat shoot
        if (bulletsShot < bulletsPerTap && AmmoLeft > 0) {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        // allow shooting and reset invoking
        readyToShoot = true;
        allowInvoke = true;
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