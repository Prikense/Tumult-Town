using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector2 Moving;
    public float MouseX;
    public float MouseY;
    public NetworkBool Jump;
    public NetworkBool Fire;
    public NetworkBool Reload;
    public NetworkBool Dash;
    public float WeaponChangeSCroll;
    public NetworkBool Weapon1;
    public NetworkBool Weapon2;
    public NetworkBool Weapon3;
}
