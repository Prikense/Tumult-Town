using System.Collections;
using System.Collections.Generic;
using System;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement ;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class StartNetworkGame : SimulationBehaviour, INetworkRunnerCallbacks
{

    [SerializeField] private NetworkRunner _networkRunner;
    [SerializeField] private string _roomName;
    [SerializeField] private string _sceneName;
    [SerializeField] private UnityEvent<NetworkRunner, PlayerRef> OnPlayerJoinedEvent;

    private mouseOnlineHanderl _localCam;

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

        //lets do it the normal way
        //move
        structChilo.Moving = new Vector2 (Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        //jump n dash
        structChilo.buttons.Set(TheButtons.Jump, Input.GetButton("Jump"));
        structChilo.buttons.Set(TheButtons.Dash, Input.GetButton("DashChilo"));

        //looking
        //tried to do it as close as locally as posible, but new input has different values for some reason so q:
        // if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0){
        //     structChilo.MouseX = Input.GetAxis("Mouse X");
        //     structChilo.MouseY = -Input.GetAxis("Mouse Y");
        // }else if(Input.GetAxis("Horizontal2") != 0 || Input.GetAxis("Vertical2") != 0) {
        //     structChilo.MouseX = Input.GetAxis("Horizontal2")*2.4f;
        //     structChilo.MouseY = Input.GetAxis("Vertical2")*.95f;
        // }

        if(_localCam == null){
            if(runner.IsServer && runner.SimulationUnityScene.FindObjectsOfTypeInOrder<mouseOnlineHanderl>().Length != 0){
                _localCam = runner.SimulationUnityScene.FindObjectsOfTypeInOrder<mouseOnlineHanderl>()[0];
            }else if(runner.IsClient && runner.SimulationUnityScene.FindObjectsOfTypeInOrder<mouseOnlineHanderl>().Length > 1){
                _localCam = runner.SimulationUnityScene.FindObjectsOfTypeInOrder<mouseOnlineHanderl>()[1];
            }
        }else{
            var aux = _localCam.takeLocalMouseInput();
            structChilo.MouseX = aux.Item1;
            structChilo.MouseY = aux.Item2;
        }
        //shooting n stuff
        structChilo.buttons.Set(TheButtons.Fire, Input.GetAxisRaw("Fire1") == 1);
        structChilo.buttons.Set(TheButtons.Reload, Input.GetButton("Reload"));

        //weapon scroll n change
        structChilo.buttons.Set(TheButtons.Weapon1, Input.GetKey("1") == true || Input.GetAxis("dpadX") == 1);
        structChilo.buttons.Set(TheButtons.Weapon2, Input.GetKey("2") == true || Input.GetAxis("dpadY") == -1);
        structChilo.buttons.Set(TheButtons.Weapon3, Input.GetKey("3") == true || Input.GetAxis("dpadX") == -1);

        // structChilo.WeaponChangeSCroll = Input.GetAxis("Mouse ScrollWheel")*10;//doesnt work too well for some reason, too tired now to fix it
        // Debug.Log("scroll: "+structChilo.WeaponChangeSCroll);

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
