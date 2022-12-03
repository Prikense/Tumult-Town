using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ObjectSpawnController : MonoBehaviour
{

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerPrefab2;
    [SerializeField] private List<NetworkObject> _networkedObjects = new List<NetworkObject>(); 
    [SerializeField] private List<cagadero> netWorkInfo = new List<cagadero>(); 
    [SerializeField] private GameManagerO timer;
    private int playerCount=0;
    
    public void SpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
    {
        cagadero a = new cagadero{netR = runner, playRef = playerRef};
        netWorkInfo.Add(a);
        playerCount++;
        if(playerCount == 2 && runner.IsServer){spawn2Guys();}
    }
    void spawn2Guys(){
        Debug.Log("Spawn Player");
        //Vector3 spawnPosition = new Vector3(-125, 50, 80+Random.Range(0,15)*2);
        playerCount++;
        NetworkObject _object = netWorkInfo[0].netR.Spawn(_playerPrefab, new Vector3(-125, 50, 80+Random.Range(0,15)*2), new Quaternion(0,80,0,0), netWorkInfo[0].playRef);
        _networkedObjects.Add(_object);
        NetworkObject _object2 = netWorkInfo[1].netR.Spawn(_playerPrefab2, new Vector3(-125, 50, 80+Random.Range(0,15)*2), new Quaternion(0,80,0,0), netWorkInfo[1].playRef);
        _networkedObjects.Add(_object2);
        timer.startTimerTime();
        Debug.Log($"Objects in simulation {_networkedObjects.Count}");
    }

}

public class cagadero{
    public NetworkRunner netR {get; set;}
    public PlayerRef playRef {get;set;}

}