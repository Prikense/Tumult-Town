using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMoveRigidBody : MonoBehaviour
{
    public float speed = 100;
    public float jumpHeight = 50;
    //public CharacterController cont;
    Rigidbody body;
//    Vector3 velocity;
    //public float grav = 9.8f;
    private bool grounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 inputXY;

    public float dashCooldown = 2.5f;
    public int dashMulti = 50;
    private float time = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        time = dashMulti;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Debug.Log(body.velocity.magnitude);
        //if(grounded && velocity.y < 0){
        //    velocity.y = -3.5f;
        //}


        //for moving
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        inputXY = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        
        //Jump
        if(grounded && Input.GetButtonDown("Jump")){
            body.AddForce(transform.up* jumpHeight);
        }

        time = time + Time.deltaTime;
        //if the dash cooldown is over then dash
        if(false && Input.GetButtonDown("DashChilo") && time > dashCooldown){
                Debug.Log("cooldown ok");
                time = 0.0f;
                //body.velocity = new Vector3(inputXY.x*dashMulti, 0, inputXY.z*dashMulti);
                if(inputXY.magnitude == 0){
                    inputXY.x = dashMulti;
                }else{
                    inputXY = inputXY*dashMulti;
                }
        }
    }

    void FixedUpdate(){
        body.AddRelativeForce(inputXY * speed);
    }
}
