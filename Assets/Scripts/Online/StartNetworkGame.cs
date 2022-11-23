using System.Collections;
using System.Collections.Generic;
using System;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement ;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class StartNetworkGame : MonoBehaviour, INetworkRunnerCallbacks
{

    [SerializeField] private NetworkRunner _networkRunner;
    [SerializeField] private string _roomName;
    [SerializeField] private string _sceneName;
    [SerializeField] private UnityEvent<NetworkRunner, PlayerRef> OnPlayerJoinedEvent;
    
    //variables de input?
    private float _mouseX, _mouseY, _scrollValue;
    private NetworkBool _jumptTime, _DASH, _IsShooting, _reloading, _we1, _we2, _we3 = false;
    private Vector2 _inputXY;


    // para el input (esta en script aparte)
    // CharacterMovementHandler characterMovementHandler;


    //inputtime jsjsj

    //looking inputs
    public void onLookx(InputAction.CallbackContext context){
        _mouseX  =  context.ReadValue<float>() ;
    }
    public void onLooky(InputAction.CallbackContext context){
        _mouseY  =  -context.ReadValue<float>() ;
    }
    public void onLook(InputAction.CallbackContext context){
        Vector2 _lookm  =  context.ReadValue<Vector2>();
        _mouseX = _lookm.x *30f;
        _mouseY = -_lookm.y *12f;
    }
    //movement input and dash n jump
    public void onMove(InputAction.CallbackContext context){
        _inputXY  =  context.ReadValue<Vector2>();
    }
    public void onJump(InputAction.CallbackContext context){
        _jumptTime  =  context.action.triggered;
    }
    public void onDash(InputAction.CallbackContext context){
        _DASH  =  context.action.triggered;
    }
    //shoot n reload
    public void onFire(InputAction.CallbackContext context){
        _IsShooting  =  context.action.triggered;
    }
    public void onReload(InputAction.CallbackContext context){
        _reloading  =  context.action.triggered;
    }
    //_weapon change
    public void onScroll(InputAction.CallbackContext context){
        _scrollValue  =  context.ReadValue<float>();
    }

    //coud probably be done better but idk
    public void on1(InputAction.CallbackContext context){
        _we1  =  context.action.triggered;
    } 
    public void on2(InputAction.CallbackContext context){
        _we2  =  context.action.triggered;
    }
    public void on3(InputAction.CallbackContext context){
        _we3  =  context.action.triggered;
    }

    async void StartNewGame(GameMode mode)
    {
        /*
        var gameArgs = new StartGameArgs();
        gameArgs.GameMode = mode;
        */
        _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = _roomName,
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
        _networkRunner.SetActiveScene(_sceneName);
    }

    public void StartGameAsHost()
    {
        StartNewGame(GameMode.AutoHostOrClient);
    }

    public void StartGameAsClient()
    {
        StartNewGame(GameMode.Client);
    }


    // public void PlayerJoined(PlayerRef player)
    // {
    //     Debug.Log("Player Joined");
    // }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("Player Joined");
        if(runner.IsServer)
        {
            Debug.Log("I am a host");
            OnPlayerJoinedEvent?.Invoke(runner, player);
        } else
        {
            Debug.Log("I am a client");
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

        NetworkInputData structChilo = new NetworkInputData();
        // movement input time
        structChilo.Moving = _inputXY;
        structChilo.MouseX = _mouseX;
        structChilo.MouseY = _mouseY;
        structChilo.Jump = _jumptTime;
        structChilo.Dash = _DASH;
        structChilo.Fire = _IsShooting;
        structChilo.Reload = _reloading;
        structChilo.Weapon1 = _we1;
        structChilo.Weapon2 = _we2;
        structChilo.Weapon3 = _we3;
        structChilo.WeaponChangeSCroll = _scrollValue;

        input.Set(structChilo);

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
}
