using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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

    [SerializeField] private MeleeManager meleeTime;
    private int previousSelectedWeapon;
    private float scrollValue;
    private bool we1;
    private bool we2;
    private bool we3;

    // Start is called before the first frame update
    void Awake()
    {
        SelectWeapon();
    }
    public void onScroll(InputAction.CallbackContext context){
        scrollValue  =  context.ReadValue<float>();
    }

    //coud probably be done better but idk
    public void on1(InputAction.CallbackContext context){
        we1  =  context.action.triggered;
    } 
    public void on2(InputAction.CallbackContext context){
        we2  =  context.action.triggered;
    }
    public void on3(InputAction.CallbackContext context){
        we3  =  context.action.triggered;
    }
    // Update is called once per frame
    void Update()
    {

        if(scrollValue > 0f)
        {
            if(SelectedWeapon >= transform.childCount - 3) //2 since at the time the camera is also inside object
                SelectedWeapon = 0;
            else
                SelectedWeapon++;
        }

        if(scrollValue < 0f)
        {
            if(SelectedWeapon <= 0) //2 since at the time the camera is also inside object
                SelectedWeapon = transform.childCount - 3;
            else
                SelectedWeapon--;
        }

        if(we1)
        {
            SelectedWeapon = 0;
        }

        if(we2)
        {
            SelectedWeapon = 1;
        }

        if(we3)// && transform.childCount - 1 >= 3) // again because of camera inside object
        {
            SelectedWeapon = 2;
        }

        if(previousSelectedWeapon != SelectedWeapon && !meleeTime.IsAttacking)
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
            else if(i <= transform.childCount - 3)
            {
                weapon.gameObject.SetActive(false); 
            }
            i++;
        }
        previousSelectedWeapon = SelectedWeapon;
    }

}
