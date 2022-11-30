using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

public class PlayerManagerO :  NetworkBehaviour
{

    // The original idea was to have 2 different mechas but for now that won't be
    [SerializeField] private AudioClip dedSfx;
    private float _playerHealth = 100f;

    public GlobalHealthManagerO healthManager;

    private GameObject eventSystem;

    [SerializeField] public WeaponManagerO raycastGun;
    [SerializeField] public ProjectileWeaponManagerO projectileGun;

    /*
    private GameObject deathCam;
    private GameObject mainCam;

    //Player prefab to instantiate it on respawn
    [SerializeField] private GameObject playerPrefab;
    */
    [SerializeField] private GameObject spawnPoint;

    //spawn position
    private float x;
    private float z;
    private bool soundCheck=false;

    // Start is called before the first frame update
    public override void Spawned()
    {
        x= Random.Range(10, 25);
        z= Random.Range(10, 25);
        healthManager = gameObject.GetComponent<GlobalHealthManagerO>(); 
        healthManager.Health = _playerHealth;
        healthManager.MaxHealth = healthManager.Health;
        Debug.Log(healthManager == null);
        Debug.Break();

        eventSystem = GameObject.Find("EventSystem"); //The canvas manager script is in this object
        //deathCam = GameObject.Find("DeathCam");
        //mainCam = GameObject.Find("Main Camera");

        // raycastGO = gameObject.transform.Find("gun_or_smt").gameObject;
        // projectileGO = gameObject.transform.Find("ProjectileGun").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(healthManager.Health);
        if(healthManager.Health < 0 || transform.position.y <= -100)
        {
            if(!soundCheck){
                AudioSource.PlayClipAtPoint(dedSfx, Vector3.zero, .3f); //use transform.position for 3d sound
                soundCheck = true;
            }
            healthManager.Health = -1;
            Death();
        }
    }

    // When player is hit by other player or AI
    public void ReceiveDamage(float damage)
    {
        healthManager.Health -= damage;
        //Debug.Log(healthManager.Health);
    }

    // Functions that I think we are going to need in the future
    void Death()
    {
        //Debug.Log("You Dead");
        //CanvasManager canvasManager = eventSystem.GetComponent<CanvasManager>();
        //mainCam.SetActive(false);
        //deathCam.SetActive(true);
        gameObject.transform.position = new Vector3(x, 15.0f, z);
        //transform.GetComponent<PlayerInput>().DeactivateInput();
        //canvasManager.DeathScreen();
        StartCoroutine(Respawn());

    }

    IEnumerator Respawn()
    {
        //mainCam.SetActive(true);
        //deathCam.SetActive(false);
        yield return new WaitForSeconds(10.0f);
        healthManager.Health = _playerHealth; 
        //transform.GetComponent<PlayerInput>().ActivateInput();
        //WeaponManager raycastGun = raycastGO.GetComponent<WeaponManager>();
        raycastGun.AmmoLeft = raycastGun.MagazineSize; // dont know if it ould be better to make Reload() public and use it
        //ProjectileWeaponManager projectileGun = projectileGO.GetComponent<ProjectileWeaponManager>();
        projectileGun.AmmoLeft = projectileGun.MagazineSize; // same comment as with raycastGun
        soundCheck = false;
        //gameObject.transform.position = spawnPoint.transform.position;
        // Restart the weapons bullets (reload them)

    }
}
