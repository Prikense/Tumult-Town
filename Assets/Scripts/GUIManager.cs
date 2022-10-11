using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIManager : MonoBehaviour
{

    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI objectiveHealth;
    public TextMeshProUGUI Score;
    public WeaponManager weaponManager;
    public HitDetection hitDetection;

    float lastHealth;
    float prevLastHealth;

    float prevLastHealthByMelee;
    float lastHealthByMelee;

    public ScoreScript scoreboard;

    // New values that are being used
    public bool isProjectileWeapon;
    public WeaponSwitch weaponSwitch; // takes the script
    private GameObject currentWeapon;
    private ProjectileWeaponManager projectileWeapon;
    private WeaponManager raycastWeapon;

    private int prevSelectedWeapon;

    // This line is only for testing, should be deleted later on
    public ProjectileWeaponManager getProjectileWeapon;

    // Start is called before the first frame update
    void Start()
    {
        //ScoreScript scoreboard = gameObject.GetComponent<scoreManager>();
        // ammoCounter.text = "" + weaponManager.magazineSize + " / " + weaponManager.magazineSize; 
        objectiveHealth.text = "No data";
        lastHealth = 0f;
        lastHealthByMelee = 0f;

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
        // This line is only for testing, should be deleted later on
        projectileWeapon = getProjectileWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(weaponManager.doneReloading || weaponManager.isShooting) {
            ammoCounter.text = "" + weaponManager.bulletsLeft + " / " + weaponManager.magazineSize;
        }
        */
        //Score.text = scoreboard.player1Score + " | " +scoreboard.player2Score;

        // Check if melee weapon (show no ammo)
        if(weaponSwitch.currentWeapon.GetComponent<HitDetection>() != null)
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


        prevLastHealth = lastHealth;
        lastHealth = weaponManager.lastHealth;

        prevLastHealthByMelee = lastHealthByMelee;
        lastHealthByMelee = hitDetection.lastHealth;

        if(prevLastHealth != lastHealth) {
            objectiveHealth.text = "Health left: " + lastHealth;
        }
        else if(prevLastHealthByMelee != lastHealthByMelee) {
            objectiveHealth.text = "Health left: " + lastHealthByMelee;
        }

    }

    void ProjectileWeapon()
    {
         
    }

    void RaycastWeapon()
    {

    }
}