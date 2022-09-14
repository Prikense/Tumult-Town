using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIManager : MonoBehaviour
{

    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI objectiveHealth;
    public WeaponManager weaponManager;
    public HitDetection hitDetection;

    float lastHealth;
    float prevLastHealth;

    float prevLastHealthByMelee;
    float lastHealthByMelee;

    // Start is called before the first frame update
    void Start()
    {
        ammoCounter.text = "" + weaponManager.magazineSize + " / " + weaponManager.magazineSize; 
        objectiveHealth.text = "No data";
        lastHealth = 0f;
        lastHealthByMelee = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponManager.reloading || weaponManager.isShooting) {
            ammoCounter.text = "" + weaponManager.bulletsLeft + " / " + weaponManager.magazineSize;
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
}