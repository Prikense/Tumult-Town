using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{

    public int selectedWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();   
    }

    // Update is called once per frame
    void Update()
    {

        int previousSelectedWeapon = selectedWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(selectedWeapon >= transform.childCount - 2) //2 since at the time the camera is also inside object
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(selectedWeapon <= 0) //2 since at the time the camera is also inside object
                selectedWeapon = transform.childCount - 2;
            else
                selectedWeapon--;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            selectedWeapon = 0;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount - 1 >= 2) // again because of camera inside object
        {
            selectedWeapon = 1;
        }

        if(previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon() 
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if(i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else if(i != transform.childCount - 1) 
                weapon.gameObject.SetActive(false); 
            i++;
        }
    }
}
