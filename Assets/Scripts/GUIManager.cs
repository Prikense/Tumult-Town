using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Image playerHealthFill;
    [SerializeField] private Image targetHealthFill;
  

    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI objectiveHealth;
    public TextMeshProUGUI Score;

    public ScoreScript scoreboard;

    // New values that are being used
    public bool isProjectileWeapon;
    public WeaponSwitch weaponSwitch; // takes the script
    private GameObject currentWeapon;
    private ProjectileWeaponManager projectileWeapon;
    private WeaponManager raycastWeapon;

    public Camera playerCamera;
    float range = 100f;

    private int prevSelectedWeapon;

    BuildingManager prevBuildingManager;
    float prevHealth;

    // This line is only for testing, should be deleted later on
    public ProjectileWeaponManager getProjectileWeapon;

    // Start is called before the first frame update
    void Start()
    {
        objectiveHealth.text = "No data";
        // New
        currentWeapon = weaponSwitch.currentWeapon;

        if(currentWeapon.GetComponent<ProjectileWeaponManager>() != null) 
        {
            isProjectileWeapon = true;
            projectileWeapon = currentWeapon.GetComponent<ProjectileWeaponManager>();
            ammoCounter.text = projectileWeapon.magazineSize + " / " + projectileWeapon.magazineSize;
        } else
        {
            isProjectileWeapon = false;
            raycastWeapon = currentWeapon.GetComponent<WeaponManager>();
            ammoCounter.text = raycastWeapon.magazineSize + " / " + raycastWeapon.magazineSize;
        }
        // This line is only for testing, should be deleted later on (used to show bullets of third weapon)
        projectileWeapon = getProjectileWeapon;

        getProjectileWeapon = null;

        prevHealth = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Score.text = scoreboard.player1Score + "|" +scoreboard.player2Score;

        // Check if melee weapon (show no ammo)
        if(weaponSwitch.currentWeapon.GetComponentInChildren<HitDetection>() != null)
        {
            ammoCounter.text = "Infinite";
            prevSelectedWeapon = weaponSwitch.selectedWeapon;
        }
        // Check if projectile weapon (show respective ammo)
        else if(isProjectileWeapon && projectileWeapon.shooting || (weaponSwitch.selectedWeapon == 2 && weaponSwitch.selectedWeapon != prevSelectedWeapon) || projectileWeapon.doneReloading)
        {
            ammoCounter.text = projectileWeapon.ammoLeft + " / " + projectileWeapon.magazineSize; //change to call function
            prevSelectedWeapon = weaponSwitch.selectedWeapon;
        }
        // Check if raycast weapon (show respective ammo)
        else if(!isProjectileWeapon && raycastWeapon.isShooting || (weaponSwitch.selectedWeapon == 0 && weaponSwitch.selectedWeapon != prevSelectedWeapon) || raycastWeapon.doneReloading)
        {
            ammoCounter.text = raycastWeapon.ammoLeft + " / " + raycastWeapon.magazineSize; //change to call function
            prevSelectedWeapon = weaponSwitch.selectedWeapon;
        }

        ObtainObjectHealth();
    }

    void ObtainObjectHealth()
    {

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {

            BuildingManager buildingManager = hit.transform.GetComponent<BuildingManager>();
            float currentHealth;
            if(buildingManager == null)
            {
                currentHealth = 0.0f;
            } else
            {
                currentHealth = buildingManager.health;
            } 
            // maybe the prevBuildingManager isn't necessary anymore since it now compares the health so if you stay looking the same its the same effect
            if(buildingManager != null && currentHealth != prevHealth) 
            { 
                Debug.Log("got one");
                objectiveHealth.text = "" + buildingManager.health;            
                FillBar(targetHealthFill, buildingManager.healthRatio); 
                prevBuildingManager = buildingManager;
                prevHealth = currentHealth;
            }

        FillBar(playerHealthFill, 1);
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