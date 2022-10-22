using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMoveRigidBody : MonoBehaviour
{
    //[SerializeField] private float Speed = 15;
    [SerializeField] private float accel = 100;
    [SerializeField] private float airAccel = 60;
    [SerializeField] private float MaxSpeed = 60;
    [SerializeField] private float MaxSpeedAir = 2;
    
    [SerializeField] private float frictionCoef = 5;
    //[SerializeField] private float frictionCoefAir = 0;
    [SerializeField] private float jumpHeight = 5;
    //public CharacterController cont;
    private Rigidbody body;
    [SerializeField] public float velocity;
    //public float grav = 9.8f;
    [SerializeField] private bool grounded;
    //[SerializeField] private bool falseGrounded;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    //[SerializeField] private float groundDistance2 = 0.6f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 inputXY;
    [SerializeField] private Vector3 fallSpeed = new Vector3(0, -4, 0);

    [SerializeField] private float dashCooldown = 2.5f;
    [SerializeField] private int dashMulti = 50;
    private float time = 0.0f;
    [SerializeField] private bool noFricOn = false;
    private float FrictionSafe = 0f;
    private float accelSafe = 0f;


    //for camera movement when moving
    private bool BobEnable = true;

    [SerializeField] private float amplitude = 0.00035f;
    [SerializeField] private float frequency = 7.0f;

    [SerializeField] private Transform Whoknows;
    [SerializeField] private Transform camera;
    [SerializeField] private GameObject SpeedBox;
//    [SerializeField] private Transform gun;
    [SerializeField] private float groundTime = 0f;//for calculating time on the ground

    private float ToggleSpeed = 3.0f;
    private Vector3 startPos;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        time = dashMulti;
        startPos = camera.localPosition;
        SpeedBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(body.velocity.magnitude);
        velocity = new Vector2(body.velocity.x, body.velocity.z).magnitude;
        if(!grounded && (body.velocity.y < 0 && body.velocity.y > -6)){
            //body.velocity += new Vector3 (body.velocity.x, fallSpeed.y, body.velocity.z);
            body.AddRelativeForce(fallSpeed);
        }


        //for moving
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        inputXY = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;



        //camera movement
        if(!BobEnable){return;}
        //Debug.Log("camera local pos: "+camera.localPosition);
        //Debug.Log("start pos?: "+startPos);
        
        CheckMotion();
        camera.LookAt(FocusTarget());
        if(velocity > 10){
            SpeedBox.SetActive(true);
            Physics.IgnoreLayerCollision(0,9, true);
        }else{
            SpeedBox.SetActive(false);
            Physics.IgnoreLayerCollision(0,9, false);
        }
    }
    
    //movement on fixed update
    void FixedUpdate(){

        
        //Jump
        if( /*&&*/ Input.GetButton("Jump")){
            if(grounded){
            body.velocity = new Vector3 (body.velocity.x, jumpHeight, body.velocity.z);
            //body.AddForce(transform.up* jumpHeight);
            }
        }

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //falseGrounded = Physics.CheckSphere(groundCheck.position, groundDistance2, groundMask);
        AccelAndMove();

        //noFrictionMode
        //dash that reduces friction and acceleration giving a feeling of drifting around
        //basically, strafe jumping withour the jumping
        if(Input.GetButton("dashNofric")){
            noFricOn = true;
            FrictionSafe = 0.05f;
            accelSafe = airAccel+10f;
        }else{
            //we save the original values back
            noFricOn = false;
            accelSafe = accel;
            FrictionSafe = frictionCoef;
        }

        //dash
        time = time + Time.deltaTime;
        //if the dash cooldown is over then dash
        if(false && Input.GetButtonDown("DashChilo") && time > dashCooldown){
                //Debug.Log("cooldown ok");
                time = 0.0f;
                //body.velocity = new Vector3(inputXY.x*dashMulti, 0, inputXY.z*dashMulti);
                if(inputXY.magnitude == 0){
                    inputXY.x = dashMulti;
                }else{
                    inputXY = inputXY*dashMulti;
                }
        }
    }

    //actual movement stuff
    private void AccelAndMove(){
        //Debug.Log("Ground time: " + groundTime);
        //initial acceleration
        if(!grounded){
            groundTime = -Time.fixedDeltaTime;
            //air friction
            /*if(velocity != 0){
                float friction = frictionCoefAir * velocity * Time.fixedDeltaTime;
                body.velocity *=  Mathf.Max(velocity-friction, 0) /velocity;
            }*/
            //direction of player inputs
            Vector3 accelDir = transform.TransformDirection(inputXY);// * Speed;
            //producto punto de la velocidad actual * direccion de input
            float dotProductVel = Vector3.Dot(body.velocity, accelDir);
            //Debug.Log(dotProductVel);
            float accelVel = airAccel * Time.fixedDeltaTime;
            

            //Debug.Log("aaa: "+accelVel);
            if(dotProductVel + accelVel > MaxSpeedAir){
                accelVel = 0;
            }


            //Debug.Log("inputs: " + accelDir);
            // Debug.Log("???: "+accelVel);
            // Debug.Log("velocity: "+body.velocity);
            // Debug.Log("dot: "+dotProductVel);
            //return
//            body.velocity = new Vector3 (accelDir.x, body.velocity.y, accelDir.z);
            body.velocity += accelDir * accelVel;
        }else{ //if (groundTime > 0){//if grounded
            //friction
            if(velocity != 0 && groundTime > Time.fixedDeltaTime*3){
                //Debug.Break();
                float friction = FrictionSafe * velocity * Time.fixedDeltaTime;
                
               body.velocity *=Mathf.Max(velocity-friction, 0) /velocity;
            }
            //direction of player inputs
            Vector3 accelDir = transform.TransformDirection(inputXY);// * Speed;
            //producto punto de la velocidad actual * direccion de input
            float dotProductVel = Vector3.Dot(new Vector3(body.velocity.x,0f, body.velocity.z), accelDir);
            //Debug.Log(dotProductVel);
            float accelVel = accelSafe * Time.fixedDeltaTime;
            
            if(accelVel > MaxSpeed - dotProductVel){
                accelVel = MaxSpeed - dotProductVel;
            }
            /*Debug.Log("dir: " + accelDir);
            Debug.Log("wishthingy: "+accelDir);
            Debug.Log("velocity: "+body.velocity);
            Debug.Log("dot: "+dotProductVel);*/
            //return
//            body.velocity = new Vector3 (accelDir.x, body.velocity.y, accelDir.z);
            body.velocity += accelDir * accelVel;

            // Vector3 accelDir = transform.TransformDirection(inputXY) *AirResistance; //Air resistance .2f
            // body.velocity += new Vector3 (accelDir.x, 0, accelDir.z);
            //body.AddRelativeForce(inputXY * Speed*10);
            groundTime += Time.fixedDeltaTime;
        }/*else{
            groundTime += Time.fixedDeltaTime;
        }*/
    }



    //head bobbing stuff
    private void PlayMotion(Vector3 motion){
        camera.localPosition +=motion;
    }
    private Vector3 FootStep(){
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency/2)*amplitude*2;
        return pos;
    }
    private void CheckMotion(){
        //float speed = new Vector3(body.velocity.x, 0, body.velocity.z).magnitude;
        if (body.velocity.magnitude < ToggleSpeed || !grounded) {resetPos();return;}
        PlayMotion(FootStep());
    }
    private void resetPos(){
        if(camera.localPosition == startPos) return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, 1 *Time.deltaTime);
    }
    private Vector3 FocusTarget(){
        Vector3 pos = new Vector3(transform.position.x, transform.position.y+Whoknows.localPosition.y, transform.position.z);
        pos += Whoknows.forward *15f;
        return pos;
    }
}
