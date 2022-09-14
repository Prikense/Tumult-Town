using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIManager : MonoBehaviour
{

    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI objectiveHealth;
    public WeaponManager weaponManager;

    // Start is called before the first frame update
    void Start()
    {
        ammoCounter.text = "" + weaponManager.magazineSize + " / " + weaponManager.magazineSize; 
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponManager.reloading || weaponManager.isShooting) {
            ammoCounter.text = "" + weaponManager.bulletsLeft + " / " + weaponManager.magazineSize;
        }
    }
}
