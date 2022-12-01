using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

[OrderAfter(typeof(HitboxManager))]

public class WeaponManagerO : NetworkBehaviour
{

    [Networked(OnChanged = nameof(OnFireChanged))]
    public bool isFiring { get; set; }

    public Transform aimPoint; // start of the raycast
    public LayerMask collisionLayers;


    [SerializeField] private AudioSource gunsfx;
    [SerializeField] private AudioSource loadsfx;
    [SerializeField] private AudioClip[] audioClips;//0 -> shot, 1 -> no ammo, 2 -> loading, 3 -> reload end
    //4 -> impact, 5 -> player impact, 6 -> ai impact
    private bool noAmmoFirstShot;
    [SerializeField] private int playerNumber;
    [SerializeField] private GameObject playerCamera;
    private float range = 200f;
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
    [SerializeField] private float reloadTime = 3f;
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

    [Networked] private NetworkBool shit {get;set;} = false;

    private float nextTimeToFire = 0f;

    [SerializeField] private GameObject bulletHole;

    // Start is called before the first frame update
    public override void Spawned()
    {
        DoneReloading = true;
        reloading = false;
        AmmoLeft = MagazineSize;
        IsShooting = false;
        playerCamera = transform.parent.parent.gameObject;//GameObject.Find("Main Camera");//2 parents (fisrt weapon holder then anotherCam)
    }

    // public void onFire(InputAction.CallbackContext context){
    //     IsShooting  =  context.action.triggered;
    // }
    // public void onReload(InputAction.CallbackContext context){
    //     reloading  =  context.action.triggered;
    // }

    /*
    public override void FixedUpdateNetwork(){
        if (GetInput(out NetworkInputData data)){
            shit = data.buttons.IsSet(TheButtons.Fire);
            reloading = data.buttons.IsSet(TheButtons.Reload);
        }
    }
    */

    // Update is called once per frame
    void FixedUpdate()
    {
        // if(Input.GetAxisRaw("Fire1") == 1 && Time.time >= nextTimeToFire && AmmoLeft > 0) {
        //     nextTimeToFire = Time.time + 1f/fireRate;
        //     shit = true;
        //     //Shoot();
        // }
        // else {
        //     IsShooting = false;
        // }


        if(shit && Time.time >= nextTimeToFire && AmmoLeft > 0  && DoneReloading) {
            nextTimeToFire = Time.time + 1f/fireRate;
            gunsfx.PlayOneShot(audioClips[0], .25f);
            noAmmoFirstShot = true;
            Shoot();
        }
        if(shit && AmmoLeft <= 0 && noAmmoFirstShot && DoneReloading){
            noAmmoFirstShot = false;
            gunsfx.PlayOneShot(audioClips[1], .5f);
        }
        if(!shit){
            noAmmoFirstShot = true;
        }

        if(reloading && DoneReloading && AmmoLeft != MagazineSize){
            if(!loadsfx.isPlaying){
                loadsfx.PlayOneShot(audioClips[2]);
            }
            Reload();
        }
    }

    // void FixedUpdate()
    // {

    // }

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

        //uncommnet to ignore layer debri, making it kinda like a laser that goes through debri, but doesnt send debri flying
        if(Physics.Raycast(raycastOrigin, forwardVector, out hit, range/*, ~LayerMask.GetMask("debri")*/)) {

            GameObject impactGO = Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.3f);

            // Debug.Log("distance: "+hit.distance);
            // Debug.Log("damage: "+damage/Mathf.Max(hit.distance/12, 2));
            BuildingManagerO buildingManager = hit.transform.GetComponent<BuildingManagerO>();
            if(buildingManager != null) {
                AudioSource.PlayClipAtPoint(audioClips[4], Vector3.zero, .3f);//use hit.point for positional sound
                buildingManager.Hit(damage/Mathf.Max(hit.distance/12, 2), playerNumber);
            }

            //players take less damage from each other to discourage killing each other
            PlayerManagerO playerHealth = hit.transform.GetComponent<PlayerManagerO>();
            if(playerHealth != null) {
                AudioSource.PlayClipAtPoint(audioClips[5], Vector3.zero, .3f);
                playerHealth.ReceiveDamage(Mathf.Max(damage/Mathf.Max(hit.distance/12, 1)/5));
            }

            EnemyAI enemyAI = hit.transform.GetComponent<EnemyAI>();
            if(enemyAI != null)
            {
                AudioSource.PlayClipAtPoint(audioClips[6], Vector3.zero, .3f);
                enemyAI.ReceiveDamage(damage/Mathf.Max(hit.distance/12, 2));
            }
            if(enemyAI == null && buildingManager == null && playerHealth == null){
                AudioSource.PlayClipAtPoint(audioClips[4], Vector3.zero, .3f);
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
        if(!loadsfx.isPlaying){
            loadsfx.PlayOneShot(audioClips[3]);
        }
        AmmoLeft = MagazineSize;
        reloading = false;
        DoneReloading = true;
    }


    IEnumerator FireEffectCO()
    {
        isFiring = true;
        muzzleFlash.Play();

        yield return new WaitForSeconds(0.05f);

        isFiring = false;
    }

    public override void FixedUpdateNetwork()
    {
        // Get the input from the Newtwork
        if (GetInput(out NetworkInputData networkInputData)){
            IsShooting = networkInputData.buttons.IsSet(TheButtons.Fire);
            reloading = networkInputData.buttons.IsSet(TheButtons.Reload);
        }

        if(IsShooting){
            Fire(networkInputData.aimForwardVector); // currently doesnt exist on data input
        }

        /*
        if(GetInput(out NetworkInputData networkInputData))
        {
            if(networkInputData.buttons.Fire){
                Fire(networkInputData.aimForwardVector); // currently doesnt exist on data input
            }
        }
        */
    }

    void Fire(Vector3 aimForwardVector)
    {

        // Falta lo del fire rate

        StartCoroutine(FireEffectCO());

        Runner.LagCompensation.Raycast(transform.position, transform.forward, range, Object.InputAuthority, out var hitInfo, collisionLayers, HitOptions.IgnoreInputAuthority);

        bool isOtherPlayerHit = false;

        if(hitInfo.Type == HitType.Hitbox){
            Debug.Log($"Hit hitbox {hitInfo.Hitbox.transform.root.name}");
            Debug.Log("Hit");

            //if(Object.HasStateAuthority){
            hitInfo.Hitbox.transform.root.GetComponent<GlobalHealthManagerO>().OnTakeDamage(5.0f);
            //}

            isOtherPlayerHit = true;
        }
        else if(hitInfo.Collider != null){
            Debug.Log($"Hit physx collider {hitInfo.Collider.transform.name}");
        }

        // Debug
        if(isOtherPlayerHit){
            Debug.DrawRay(aimPoint.position, aimForwardVector * range, Color.red, 1);
        }
    }

    static void OnFireChanged(Changed<WeaponManagerO> changed)
    {

        bool isFiringCurrent = changed.Behaviour.isFiring;

        // Load the old value
        changed.LoadOld();

        bool isFiringOld = changed.Behaviour.isFiring;

        if(isFiringCurrent && !isFiringOld){
            changed.Behaviour.OnFireRemote();
        }
    }

    void OnFireRemote()
    {
        if(!Object.HasInputAuthority){
            muzzleFlash.Play();
        }
    }
}
