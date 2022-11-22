using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public const byte MOUSEBUTTON1 = 0x01;
    public const byte MOUSEBUTTON2 = 0x02;

    // These 2 are from the video
    public Vector2 movementInput;
    public float rotationInput;
    public NetworkBool isJumpPressed;
    
    public byte Buttons;
    public Vector3 Direction;
}
