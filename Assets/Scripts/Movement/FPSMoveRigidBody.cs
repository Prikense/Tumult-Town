using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public Rigidbody body;
    [SerializeField] public float velocity;
    //public float grav = 9.8f;
    [SerializeField] private bool grounded;
    //[SerializeField] private bool falseGrounded;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    //[SerializeField] private float groundDistance2 = 0.6f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 vectorMove;
    private Vector2 inputXY;
    [SerializeField] private Vector3 fallSpeed = new Vector3(0, -4, 0);
    private bool jumptime = false;

    [SerializeField] private float dashCooldown = 4f;
    [SerializeField] private int dashMulti = 50;
    public float time = 0.0f;
    [SerializeField] private bool noFricOn = false;
    [SerializeField] private bool DASH = false;
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

    public void onMove(InputAction.CallbackContext context){
        inputXY  =  context.ReadValue<Vector2>();
    }
    public void onJump(InputAction.CallbackContext context){
        jumptime  =  context.action.triggered;
    }
    public void onDash(InputAction.CallbackContext context){
        DASH  =  context.action.triggered;
    }
    public void onSlide(InputAction.CallbackContext context){
        noFricOn  =  context.action.triggered;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(body.velocity.magnitude);
        velocity = new Vector2(body.velocity.x, body.velocity.z).magnitude;



        //for moving
        // float x = Input.GetAxisRaw("Horizontal");
        // float z = Input.GetAxisRaw("Vertical");

        vectorMove = new Vector3(inputXY.x, 0, inputXY.y).normalized;



        //camera movement

        //Debug.Log("camera local pos: "+camera.localPosition);
        //Debug.Log("start pos?: "+startPos);
        

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
        

        if(grounded){
            groundTime += Time.fixedDeltaTime;
        }
        if(!grounded && (body.velocity.y < 0 && body.velocity.y > -6)){
            //body.velocity += new Vector3 (body.velocity.x, fallSpeed.y, body.velocity.z);
            body.AddRelativeForce(fallSpeed);
        }
        //Jump
        if( jumptime/*&& Input.GetButton("Jump")*/){
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
        if(false && /*Input.GetButton("dashNofric")*/ noFricOn){
            //noFricOn = true;
            FrictionSafe = 0.05f;
            accelSafe = airAccel-20f;
        }else{
            //we save the original values back
            //noFricOn = false;
            accelSafe = accel;
            FrictionSafe = frictionCoef;
        }

        //dash
        time = time + Time.fixedDeltaTime;
        //if the dash cooldown is over then dash
        if(false && Input.GetButton("DashChilo") && time > dashCooldown && grounded){
                //Debug.Log("cooldown ok");
                time = 0.0f;
                //DASH = true;
                //body.velocity = new Vector3(vectorMove.x*dashMulti, 0, vectorMove.z*dashMulti);
                if(vectorMove.magnitude == 0){
                    Debug.Log("no input");
                    //vectorMove.z = dashMulti;
                    //lets try moving the velocity
                    body.velocity = transform.forward*dashMulti;
                }else{
                    Debug.Log("input??");
                    //body.velocity = body.velocity*dashMulti;
                    //Debug.Log("input" + vectorMove);
                    body.velocity = transform.TransformDirection(vectorMove)*dashMulti;
                }
        }
        if (time > .5f){
            //DASH = false;
        }

        if(!BobEnable){return;}
        CheckMotion();
        camera.LookAt(FocusTarget());

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
            Vector3 accelDir = transform.TransformDirection(vectorMove);// * Speed;
            //producto punto de la velocidad actual * direccion de input
            float dotProductVel = Vector3.Dot(body.velocity, accelDir);
            //Debug.Log(dotProductVel);
            float accelVel = airAccel * Time.fixedDeltaTime;
            

            
            if(dotProductVel + accelVel > MaxSpeedAir){
                //accelVel = MaxSpeedAir - dotProductVel;//version2
                accelVel = Mathf.Max(MaxSpeedAir - dotProductVel, 0);//version1
            }

            //Debug.Log("dotProduct: "+dotProductVel);

            body.velocity += accelDir * accelVel;
        }else if (groundTime > Time.fixedDeltaTime*3 && !DASH){//if grounded and not dashing
            //friction
            if(velocity != 0){
                //Debug.Break();
                float friction = FrictionSafe * velocity * Time.fixedDeltaTime;
                
               body.velocity *=Mathf.Max(velocity-friction, 0) /velocity;
            }
            //direction of player inputs
            Vector3 accelDir = transform.TransformDirection(vectorMove);// * Speed;
            //producto punto de la velocidad actual * direccion de input
            float dotProductVel = Vector3.Dot(new Vector3(body.velocity.x,0f, body.velocity.z), accelDir);

            float accelVel = accelSafe * Time.fixedDeltaTime;
            
            if(accelVel > MaxSpeed - dotProductVel){
                accelVel = MaxSpeed - dotProductVel;
            }
            body.velocity += accelDir * accelVel;

        }
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
