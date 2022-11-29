using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ObjectSpawnController : MonoBehaviour
{

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerPrefab2;
    [SerializeField] private List<NetworkObject> _networkedObjects = new List<NetworkObject>(); 
    
    public void SpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
    {
        Debug.Log("Spawn Player");
        Vector3 spawnPosition = new Vector3(-125, 50, 80+Random.Range(0,15)*2);
        if(_networkedObjects.Count == 0){
            NetworkObject _object = runner.Spawn(_playerPrefab, spawnPosition, new Quaternion(0,80,0,0), playerRef);
            _networkedObjects.Add(_object);
        }else{
            NetworkObject _object = runner.Spawn(_playerPrefab2, spawnPosition, new Quaternion(0,80,0,0), playerRef);
            _networkedObjects.Add(_object);
        }
        Debug.Log($"Objects in simulation {_networkedObjects.Count}");
    }

}
