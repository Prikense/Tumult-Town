using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class CharacterMovementHandler : NetworkBehaviour
{

    [SerializeField] private NetworkCharacterControllerPrototypeCustom _characterController;
    [SerializeField] private int _playerSpeed = 5;
    private Vector3 _forward;
    [Networked] private TickTimer _delay { get; set; }

    Vector2 viewInput;
    // Rotation
    float cameraRotationX = 0;
    Camera localCamera;

    void Awake()
    {
        localCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        cameraRotationX += viewInput.y * _characterController.viewUpDownRotationSpeed; //* Time.deltaTime
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);

        localCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0);
    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            // Rotate the view
            _characterController.Rotate(data.rotationInput);

            Vector3 moveDirection = transform.forward * data.movementInput.y + transform.right * data.movementInput.x;
            // Vector moveDirection = data.Direction * Runner.DeltaTime * _playerSpeed; // codigo del profe
            moveDirection.Normalize();

            // Jump 
            if(data.isJumpPressed)
            {
                _characterController.Jump();
            }

            _characterController.Move(moveDirection);
        }
    }

    public void SetViewInputVector(Vector2 viewInput)
    {
        this.viewInput = viewInput;
    }
}
