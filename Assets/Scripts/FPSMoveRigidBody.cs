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
    public bool grounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 inputXY;

    public float dashCooldown = 2.5f;
    public int dashMulti = 50;
    private float time = 0.0f;


    //for camera movement when moving
    public bool BobEnable = true;

    [SerializeField] private float amplitude = 0.15f;
    [SerializeField] private float frequency = 10.0f;

    [SerializeField] private Transform Whoknows;
    [SerializeField] private Transform camera;
//    [SerializeField] private Transform gun;

    private float ToggleSpeed = 3.0f;
    private Vector3 startPos;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        time = dashMulti;
        startPos = camera.localPosition;
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

        //camera movement
        if(!BobEnable){return;}
        Debug.Log("camera local pos: "+camera.localPosition);
        Debug.Log("start pos?: "+startPos);
        
        CheckMotion();
        camera.LookAt(FocusTarget());
    }

    void FixedUpdate(){
        body.AddRelativeForce(inputXY * speed);
    }

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
        float speed = new Vector3(body.velocity.x, 0, body.velocity.z).magnitude;
        if (speed < ToggleSpeed || !grounded) {resetPos();return;}
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
