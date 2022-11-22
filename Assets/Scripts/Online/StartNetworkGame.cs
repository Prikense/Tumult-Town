using System.Collections;
using System.Collections.Generic;
using System;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement ;
using UnityEngine.Events;

public class StartNetworkGame : MonoBehaviour, INetworkRunnerCallbacks
{

    [SerializeField] private NetworkRunner _networkRunner;
    [SerializeField] private string _roomName;
    [SerializeField] private string _sceneName;
    [SerializeField] private UnityEvent<NetworkRunner, PlayerRef> OnPlayerJoinedEvent;

    // para el input (esta en script aparte)
    CharacterInputHandler characterInputHandler;
    CharacterMovementHandler characterMovementHandler;

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

        Vector2 moveInputVector = Vector2.zero;
        // movement input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");



        Vector2 viewInputVector = Vector2.zero;
        // rotation input (mouse)
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y");

        bool isJumpButtonPressed = Input.GetButtonDown("Jump");

        NetworkInputData networkInputData = new NetworkInputData();
        networkInputData.movementInput = moveInputVector;
        networkInputData.rotationInput = viewInputVector.x;
        networkInputData.isJumpPressed = isJumpButtonPressed;
        input.Set(networkInputData);


        // characterInputHandler = NetworkPlayer.Local.GetComponent<CharacterInputHandler>();
        // CharacterInputHandler characterInputHandler = new CharacterInputHandler();
        // input.Set(characterInputHandler.GetNetworkInput());
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
