using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

public class CharacterMovementHandler : NetworkBehaviour
{
    //sfx n stuff
    [SerializeField] private AudioSource stepSfx;
    [SerializeField] private AudioSource dashSfx;
    [SerializeField] private AudioSource dashOKSfx;
    
    [SerializeField] private AudioSource jumpSfx;
    [SerializeField] private AudioSource landSfx;


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
    private bool DASH = false;
    [SerializeField] private bool DASH2 = false;
    private float FrictionSafe = 0f;
    private float accelSafe = 0f;


    //for CameraVariable movement when moving
    private bool BobEnable = true;

    [SerializeField] private float amplitude = 0.00035f;
    [SerializeField] private float frequency = 7.0f;

    [SerializeField] private Transform Whoknows;
    [SerializeField] private GameObject CameraVariable;
    [SerializeField] private GameObject SpeedBox;
//    [SerializeField] private Transform gun;
    [SerializeField] private float groundTime = 0f;//for calculating time on the ground
    [SerializeField] private float movingTime = 0f;//for step sfx    


    private float ToggleSpeed = 3.0f;
    private Vector3 startPos;

    [SerializeField] private GameObject cockpitNcanvas;
    private GUIManagerO aux;


    // Start is called before the first frame update
    void Start()
    {
        //i want to kill myself, this is so gross somebody stop me
        CameraVariable = GameObject.Find("Main Camera");
        cockpitNcanvas = GameObject.Find("CanvasNCockpit");
        cockpitNcanvas.transform.Find("Cockpit Parent").gameObject.SetActive(true);
        cockpitNcanvas.transform.Find("Canvas").gameObject.SetActive(true);

        if(Object.HasInputAuthority){
            CameraVariable.transform.SetParent(Whoknows, false);
            cockpitNcanvas.transform.SetParent(transform);
            aux = cockpitNcanvas.GetComponentInChildren<GUIManagerO>();
            aux.weaponSwitch = transform.GetComponentInChildren<WeaponSwitchO>();
            aux.PlayerHealth = transform.GetComponent<PlayerManagerO>();
            aux.raycastWeapon = transform.GetComponent<PlayerManagerO>().raycastGun;
            aux.getProjectileWeapon = transform.GetComponent<PlayerManagerO>().projectileGun;
        }
        
        //body = GetComponent<Rigidbody>();
        time = dashMulti;
        startPos = CameraVariable.transform.localPosition;
        SpeedBox.SetActive(false);
    }

    // public void onMove(InputAction.CallbackContext context){
    //     inputXY  =  context.ReadValue<Vector2>();
    // }
    // public void onJump(InputAction.CallbackContext context){
    //     jumptime  =  context.action.triggered;
    // }
    // public void onDash(InputAction.CallbackContext context){
    //     DASH  =  context.action.triggered;
    // }
    // public void onSlide(InputAction.CallbackContext context){
    //     noFricOn  =  context.action.triggered;
    // }

    public override void FixedUpdateNetwork(){
        if (GetInput(out NetworkInputData data)){
            inputXY = data.Moving;
            jumptime = data.buttons.IsSet(TheButtons.Jump);
            DASH = data.buttons.IsSet(TheButtons.Dash);
        }
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

        //Debug.Log("CameraVariable local pos: "+CameraVariable.localPosition);
        //Debug.Log("start pos?: "+startPos);
        


    }
    
    //movement on fixed update
    void FixedUpdate(){
        
        if(velocity > 12){
            SpeedBox.SetActive(true);
            Physics.IgnoreLayerCollision(0,9, true);
        }else{
            SpeedBox.SetActive(false);
            Physics.IgnoreLayerCollision(0,9, false);
        }

        if(grounded){
            if(groundTime > .04 && groundTime < .3 && !landSfx.isPlaying && !jumpSfx.isPlaying){
                landSfx.Play();
            }
            groundTime += Time.fixedDeltaTime;
            movingTime += Time.fixedDeltaTime;
            if(velocity < 1){
                movingTime = 0;
            }
        }
        if(!grounded && (body.velocity.y < 0 && body.velocity.y > -6)){
            //body.velocity += new Vector3 (body.velocity.x, fallSpeed.y, body.velocity.z);
            body.AddRelativeForce(fallSpeed);
        }
        //Jump
        if( jumptime/*&& Input.GetButton("Jump")*/){
            if(grounded){
                if(!jumpSfx.isPlaying){
                    jumpSfx.Play();
                }
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
        if(/*false && Input.GetButton("dashNofric")*/false && noFricOn){
            //noFricOn = true;
            FrictionSafe = 0.05f;
            accelSafe = 0;
        }else{
            //we save the original values back
            //noFricOn = false;
            accelSafe = accel;
            FrictionSafe = frictionCoef;
        }

        //dash
        time = time + Time.fixedDeltaTime;
        //if the dash cooldown is over then dash
        if(DASH && time > dashCooldown && grounded){
                //Debug.Log("cooldown ok");
                time = 0.0f;
                DASH2 = true;
                if(!dashSfx.isPlaying){
                    dashSfx.Play();
                }
                //body.velocity = new Vector3(vectorMove.x*dashMulti, 0, vectorMove.z*dashMulti);
                if(vectorMove.magnitude == 0){
                    //Debug.Log("no input");
                    //vectorMove.z = dashMulti;
                    //lets try moving the velocity
                    body.velocity = transform.forward*dashMulti;
                }else{
                    //Debug.Log("input??");
                    //body.velocity = body.velocity*dashMulti;
                    //Debug.Log("input" + vectorMove);
                    body.velocity = transform.TransformDirection(vectorMove)*dashMulti;
                }
        }
        if(time > dashCooldown-.2 && time < dashCooldown && !dashOKSfx.isPlaying){
            dashOKSfx.Play();
        }
        if (time > .5f){
            DASH2 = false;
        }

        // if(Object.HasInputAuthority && BobEnable){
        //     CheckMotion();
        //     CameraVariable.transform.LookAt(FocusTarget());
        // }
    }

    //actual movement stuff
    private void AccelAndMove(){
        //Debug.Log("Ground time: " + groundTime);
        //initial acceleration
        if(!grounded){
            groundTime = -Time.fixedDeltaTime;
            movingTime = 0;
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
        }else if (groundTime > Time.fixedDeltaTime*3 && !DASH2){//if grounded and not dashing
            //step sound
            if(/*CameraVariable.localPosition.y < 0.0f*/ (movingTime < .5f || (movingTime % 1f) <= 0.02)  && !stepSfx.isPlaying && vectorMove.magnitude != 0 && !landSfx.isPlaying ){
                 stepSfx.Play();
             }

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



    // //head bobbing stuff
    // private void PlayMotion(Vector3 motion){
    //     CameraVariable.transform.localPosition +=motion;
    // }
    // private Vector3 FootStep(){
    //     Vector3 pos = Vector3.zero;
    //     pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
    //     pos.x += Mathf.Cos(Time.time * frequency/2)*amplitude*2;
    //     return pos;
    // }
    // private void CheckMotion(){
    //     //float speed = new Vector3(body.velocity.x, 0, body.velocity.z).magnitude;
    //     if (body.velocity.magnitude < ToggleSpeed || !grounded) {resetPos();return;}
    //     PlayMotion(FootStep());
    // }
    // private void resetPos(){
    //     if(CameraVariable.transform.localPosition == startPos) return;
    //     CameraVariable.transform.localPosition = Vector3.Lerp(CameraVariable.transform.localPosition, startPos, 1 *Time.deltaTime);
    // }
    // private Vector3 FocusTarget(){
    //     Vector3 pos = new Vector3(transform.position.x, transform.position.y+Whoknows.localPosition.y, transform.position.z);
    //     pos += Whoknows.forward *15f;
    //     return pos;
    // }
}