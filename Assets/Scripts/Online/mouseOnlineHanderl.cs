using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class mouseOnlineHanderl : NetworkBehaviour, IBeforeUpdate
{


    private float cameraSensitivity = 1;
    [SerializeField] private Transform playerBody;
    private float xRotation = 0f;
    // private bool lookC;
    // private bool lookM;
    float mouseX;
    float mouseY;

    float mouseXO;
    float mouseYO;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // public void onLookx(InputAction.CallbackContext context){
    //     mouseX  =  context.ReadValue<float>() * cameraSensitivity ;
    // }
    // public void onLooky(InputAction.CallbackContext context){
    //     mouseY  =  -context.ReadValue<float>() * cameraSensitivity ;
    // }
    // public void onLook(InputAction.CallbackContext context){
    //     lookm  =  context.ReadValue<Vector2>();
    //     mouseX = lookm.x * cameraSensitivity*30f;
    //     mouseY = -lookm.y * cameraSensitivity*12f;
    // }


    public override void FixedUpdateNetwork(){
        if (GetInput(out NetworkInputData data)){
            mouseXO = data.MouseX*cameraSensitivity+mouseX;
            mouseYO = data.MouseY*cameraSensitivity+mouseY;
        }
    }
    // Update is called once per frame
    public override void Render()
    {  
        //for looking
        // if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
        //     mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        //     mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        //     rotateTime(mouseX, -mouseY);
        // }else if(Input.GetAxis("Horizontal2") != 0 || Input.GetAxis("Vertical2") != 0) {
        //     mouseX = Input.GetAxis("Horizontal2") * cameraSensitivity * Time.deltaTime;
        //     mouseY = Input.GetAxis("Vertical2") * cameraSensitivity * Time.deltaTime;
        //     rotateTime(mouseX, mouseY);
        // }
        //Debug.Log("???"+lookm);
        // if(lookm.magnitude !=0 ){
        //     mouseX = lookm.x;
        //     mouseY = lookm.y;
        // }
        //Debug.Log("X: "+mouseXO*Time.fixedDeltaTime*10+"Y: "+mouseYO*Time.fixedDeltaTime*10);
        rotateTime(mouseXO*Time.fixedDeltaTime*10, mouseYO*Time.fixedDeltaTime*10);
    }

    void rotateTime(float x, float y){
         xRotation += y;
        //clamp x rotation looking
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);
        //transform itself
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //transform the root
        playerBody.Rotate(Vector3.up * x);
        return;
    }

    void IBeforeUpdate.BeforeUpdate()
    {
        if(Object.HasInputAuthority){
            //for looking localy
            if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
                mouseX += Input.GetAxis("Mouse X");//*2.5f;
                mouseY +=- Input.GetAxis("Mouse Y");//*1.5f;
            }else if(Input.GetAxis("Horizontal2") != 0 || Input.GetAxis("Vertical2") != 0) {
                mouseX += Input.GetAxis("Horizontal2");
                mouseY += Input.GetAxis("Vertical2");
            }
        }
    }

    public (float, float) takeLocalMouseInput(){
        var mouseXY = (mouseX, mouseY);
        mouseX = 0;
        mouseY = 0;
        return mouseXY;
    }
}
