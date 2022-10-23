using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{

    private int _selectedWeapon = 0;
    public int SelectedWeapon
    {
        get{return _selectedWeapon;}
        set{_selectedWeapon = value;}
    } 

    private GameObject _currentWeapon;
    public GameObject CurrentWeapon
    {
        get{return _currentWeapon;}
        set{_currentWeapon = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();   
    }

    // Update is called once per frame
    void Update()
    {

        int previousSelectedWeapon = SelectedWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(SelectedWeapon >= transform.childCount - 2) //2 since at the time the camera is also inside object
                SelectedWeapon = 0;
            else
                SelectedWeapon++;
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if(SelectedWeapon <= 0) //2 since at the time the camera is also inside object
                SelectedWeapon = transform.childCount - 2;
            else
                SelectedWeapon--;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            SelectedWeapon = 0;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)) 
        {
            SelectedWeapon = 1;
        }

        if(Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount - 1 >= 3) // again because of camera inside object
        {
            SelectedWeapon = 2;
        }

        if(previousSelectedWeapon != SelectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon() 
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if(i == SelectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                CurrentWeapon = weapon.gameObject;
            }
            else if(i != transform.childCount - 1) 
            {
                weapon.gameObject.SetActive(false); 
            }
            i++;
        }
    }

}
