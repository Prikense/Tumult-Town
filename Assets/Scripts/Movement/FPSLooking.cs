using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLooking : MonoBehaviour
{

    [SerializeField] private float cameraSensitivity = 650f;
    [SerializeField] private Transform playerBody;
    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {  
        //for looking
        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
            float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
            rotateTime(mouseX, -mouseY);
        }else if(Input.GetAxis("Horizontal2") != 0 || Input.GetAxis("Vertical2") != 0) {
            float mouseX = Input.GetAxis("Horizontal2") * cameraSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Vertical2") * cameraSensitivity * Time.deltaTime;
            rotateTime(mouseX, mouseY);
        }

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
}