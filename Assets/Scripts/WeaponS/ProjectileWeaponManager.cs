using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileWeaponManager : MonoBehaviour
{
    

    [SerializeField] private int playerNumber;
    // bullet
    [SerializeField] private GameObject bullet;

    // bullet force
    [SerializeField] private float shootForce, upwardForce;

    // gun stats
    [SerializeField] private float spread, reloadTime, timeBetweenShots;
    [SerializeField] private int bulletsPerTap;
    private float timeBetweenShooting = .8f;

    private int _magazineSize = 20;
    public int MagazineSize
    {
        get{return _magazineSize;}
        set{_magazineSize = value;}
    }

    private bool alreadyShot=true;
    private int bulletsShot;

    private int _ammoLeft;
    public int AmmoLeft
    {
        get{return _ammoLeft;}
        set{_ammoLeft = value;}
    } 

    private bool readyToShoot, reloading;

    private bool _shooting;
    public bool IsShooting
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

    // bug fixing
    public bool allowInvoke = true;

    private void Awake() 
    {
        DoneReloading = true;
        // make sure magazine is full and is able to shoot
        AmmoLeft = MagazineSize;
        readyToShoot = true;
    }

    //ctrl+c ctrl+v yup
    public void onFire(InputAction.CallbackContext context){
        IsShooting  =  context.action.triggered;
    }
    public void onReload(InputAction.CallbackContext context){
        reloading  =  context.action.triggered;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        // check if it is allowed to hold down button and take corresponding input 
        //lets make up our minds if we want this gun to be automatic or not, so we can simplify this code

        // check if player wants to reload
        if(reloading) Reload();
    }

    void FixedUpdate()
    {
        // shooting
        if(readyToShoot && IsShooting && !reloading && AmmoLeft > 0) 
        {
            bulletsShot = 0;

            Shoot();
        }

        // invoke resetShot function (if not already invoked)
        if(allowInvoke && !IsShooting) {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // find the exact position using a raycast
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
        if(currentBullet.TryGetComponent<BulletHit>(out BulletHit a)){
            a.PlayerNumber = playerNumber;
        }
        // rotate bullet to shoot direction
        
        currentBullet.transform.forward = directionWithSpread.normalized;

        // adding force to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        // this one is only necessary when using proyectiles like granades (currently not needed)
        // currentBullet.GetComponent<Rigidbody>.AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        AmmoLeft--;
        bulletsShot++;



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