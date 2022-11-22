using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ObjectSpawnController : MonoBehaviour
{

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private List<NetworkObject> _networkedObjects = new List<NetworkObject>(); 
    
    public void SpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
    {
        Debug.Log("Spawn Player");
        Vector3 spawnPosition = new Vector3(_networkedObjects.Count, 0, 0);
        NetworkObject _object = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, playerRef);
        _networkedObjects.Add(_object);
        Debug.Log($"Objects in simulation {_networkedObjects.Count}");
    }

}
