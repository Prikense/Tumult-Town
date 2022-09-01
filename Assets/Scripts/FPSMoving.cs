using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMoving : MonoBehaviour
{
    public float speed = 15f;
    public float jumpHeight = 5f;
    public CharacterController cont;
    Vector3 velocity;
    public float grav = 9.8f;
    private bool grounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(grounded && velocity.y < 0){
            velocity.y = -3.5f;
        }


        //for moving
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        //using "move"
        Vector3 move = transform.right* x + transform.forward*z;
        cont.Move(move*speed*Time.deltaTime);

        //for jumping n falling
        velocity.y -= grav* Time.deltaTime;
        cont.Move(velocity*Time.deltaTime);

        if(grounded && Input.GetButtonDown("Jump")){
            velocity.y = jumpHeight;
        }

        //dash & slide? WIP
    }
}
