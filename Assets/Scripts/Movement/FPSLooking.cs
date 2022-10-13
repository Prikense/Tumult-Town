using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLooking : MonoBehaviour
{

    public float cameraSensitivity = 650f;
    public Transform playerBody;
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {  
        //for looking
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;        
        xRotation -= mouseY;
        //clamp x rotation looking
        xRotation = Mathf.Clamp(xRotation, -89f, 89f);        
        //transform itself
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //transform the root
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
