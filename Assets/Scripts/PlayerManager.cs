using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    // The original idea was to have 2 different mechas but for now that won't be
    private float _playerHealth = 100f;

    private GlobalHealthManager healthManager;

    private GameObject eventSystem;

    /*
    private GameObject deathCam;
    private GameObject mainCam;

    //Player prefab to instantiate it on respawn
    [SerializeField] private GameObject playerPrefab;
    */
    [SerializeField] private GameObject spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        healthManager = gameObject.GetComponent<GlobalHealthManager>(); 
        healthManager.Health = _playerHealth; 

        eventSystem = GameObject.Find("EventSystem"); //The canvas manager script is in this object
        //deathCam = GameObject.Find("DeathCam");
        //mainCam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if(healthManager.Health <= 0)
        {
            Death();
        }
    }

    // When player is hit by other player or AI
    public void ReceiveDamage(float damage)
    {
        healthManager.Health -= damage;
        Debug.Log(healthManager.Health);
    }

    // Functions that I think we are going to need in the future
    void Death()
    {
        Debug.Log("You Dead");
        CanvasManager canvasManager = eventSystem.GetComponent<CanvasManager>();
        //mainCam.SetActive(false);
        //deathCam.SetActive(true);
        gameObject.transform.position = new Vector3(20.0f, 20.0f, 6.0f);
        canvasManager.DeathScreen();
        StartCoroutine(Respawn());

    }

    IEnumerator Respawn()
    {
        //mainCam.SetActive(true);
        //deathCam.SetActive(false);
        yield return new WaitForSeconds(10.0f);
        healthManager.Health = _playerHealth; 
        //gameObject.transform.position = spawnPoint.transform.position;
    }
}