using Fusion;
using UnityEngine;

enum TheButtons{
    Jump = 0,
    Fire = 1,
    Reload = 2,
    Dash = 3,
    Weapon1 = 4,
    Weapon2 = 5,
    Weapon3 = 6,
}

public struct NetworkInputData : INetworkInput
{
    public Vector2 Moving;
    public Vector2 Looking;
    public float MouseX;
    public float MouseY;
    public float WeaponChangeSCroll;

    // public NetworkBool Jump;
    // public NetworkBool Fire;
    // public NetworkBool Reload;
    // public NetworkBool Dash;
    // public NetworkBool Weapon1;
    // public NetworkBool Weapon2;
    // public NetworkBool Weapon3;

    public NetworkButtons buttons;
}
