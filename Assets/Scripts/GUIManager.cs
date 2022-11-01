using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Image playerHealthFill;
    [SerializeField] private Image targetHealthFill;
  

    [SerializeField] private TextMeshProUGUI ammoCounter;
    [SerializeField] private TextMeshProUGUI objectiveHealth;
    [SerializeField] private TextMeshProUGUI Score;

    [SerializeField] private ScoreScript scoreboard;

    // New values that are being used
    [SerializeField] private bool isProjectileWeapon;
    [SerializeField] private WeaponSwitch weaponSwitch; // takes the script
    private GameObject currentWeapon;
    private ProjectileWeaponManager projectileWeapon;
    private WeaponManager raycastWeapon;
    [SerializeField] private PlayerManager PlayerHealth;

    [SerializeField] private Camera playerCamera;
    private float range = 100f;

    private int prevSelectedWeapon;

    //private BuildingManager prevBuildingManager;
    private GlobalHealthManager prevObjectHealthManager;
    private float prevHealth;

    // This line is only for testing, should be deleted later on
    [SerializeField] private ProjectileWeaponManager getProjectileWeapon;

    // Start is called before the first frame update
    void Start()
    {
        objectiveHealth.text = "No data";
        // New
        currentWeapon = weaponSwitch.CurrentWeapon;

        if(currentWeapon.GetComponent<ProjectileWeaponManager>() != null) 
        {
            isProjectileWeapon = true;
            projectileWeapon = currentWeapon.GetComponent<ProjectileWeaponManager>();
            ammoCounter.text = projectileWeapon.MagazineSize + " / " + projectileWeapon.MagazineSize;
        } else
        {
            isProjectileWeapon = false;
            raycastWeapon = currentWeapon.GetComponent<WeaponManager>();
            ammoCounter.text = raycastWeapon.MagazineSize + " / " + raycastWeapon.MagazineSize;
        }
        // This line is only for testing, should be deleted later on (used to show bullets of third weapon)
        projectileWeapon = getProjectileWeapon;

        getProjectileWeapon = null;

        prevHealth = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Score.text = scoreboard.Player1Score + " | " +scoreboard.Player2Score;

        // Check if melee weapon (show no ammo)
        if(weaponSwitch.CurrentWeapon.GetComponentInChildren<HitDetection>() != null)
        {
            ammoCounter.text = "Infinite";
            prevSelectedWeapon = weaponSwitch.SelectedWeapon;
        }
        // Check if projectile weapon (show respective ammo)
        else if(isProjectileWeapon && projectileWeapon.Shooting || (weaponSwitch.SelectedWeapon == 2 && weaponSwitch.SelectedWeapon != prevSelectedWeapon) || projectileWeapon.DoneReloading)
        {
            ammoCounter.text = projectileWeapon.AmmoLeft + " / " + projectileWeapon.MagazineSize; //change to call function
            prevSelectedWeapon = weaponSwitch.SelectedWeapon;
        }
        // Check if raycast weapon (show respective ammo)
        else if(!isProjectileWeapon && raycastWeapon.IsShooting || (weaponSwitch.SelectedWeapon == 0 && weaponSwitch.SelectedWeapon != prevSelectedWeapon) || raycastWeapon.DoneReloading)
        {
            ammoCounter.text = raycastWeapon.AmmoLeft + " / " + raycastWeapon.MagazineSize; //change to call function
            prevSelectedWeapon = weaponSwitch.SelectedWeapon;
        }

    }

    void FixedUpdate()
    {
        ObtainObjectHealth();
    }

    void ObtainObjectHealth()
    {

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {

            //BuildingManager buildingManager = hit.transform.GetComponent<BuildingManager>();
            GlobalHealthManager objectHealthManager = hit.transform.GetComponent<GlobalHealthManager>(); 
            float currentHealth = 0.0f; 
            // this if and else if is currently working but there may be a cleaner way of doing this
            if(objectHealthManager == null && prevObjectHealthManager == null) // i dont think this makes sense
            {
                currentHealth = 0.0f;
                objectiveHealth.text = "" + 0; 
            } 
            else if(objectHealthManager != null)
            {
                currentHealth = objectHealthManager.Health;
            } 
            // maybe the prevBuildingManager isn't necessary anymore since it now compares the health so if you stay looking the same its the same effect
            if(objectHealthManager != null && currentHealth != prevHealth) 
            { 
                //Debug.Log("got one");
                objectiveHealth.text = "" + objectHealthManager.Health;            
                FillBar(targetHealthFill, objectHealthManager.HealthRatio); 
                prevObjectHealthManager = objectHealthManager;
                prevHealth = currentHealth;
            }

        FillBar(playerHealthFill, PlayerHealth.healthManager.HealthRatio);
        }
    }

    void ProjectileWeapon()
    {
         
    }

    void RaycastWeapon()
    {

    }

    void FillBar(Image image, float fillAmount){
       image.fillAmount = fillAmount;
    }


}