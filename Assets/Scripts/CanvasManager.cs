using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    private GameObject _hudCanvas;
    public GameObject HUDCanvas
    {
        get{return _hudCanvas;}
        set{_hudCanvas = value;}
    }

    private GameObject _deathCanvas;
    public GameObject DeathCanvas
    {
        get{return _deathCanvas;}
        set{_deathCanvas = value;}
    }

    private GameObject deathCam;
    private GameObject mainCam;

    //Player prefab to instantiate it on respawn
    [SerializeField] private GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        HUDCanvas = GameObject.Find("Canvas");
        DeathCanvas = GameObject.Find("DeathMenu");
        DeathCanvas.SetActive(false);
        deathCam = GameObject.Find("DeathCam");
        mainCam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeathScreen()
    {
        HUDCanvas.SetActive(false);
        DeathCanvas.SetActive(true);
        mainCam.SetActive(false);
        deathCam.SetActive(true);
        StartCoroutine(PlayerRespawned());
    }

    IEnumerator PlayerRespawned()
    {
        yield return new WaitForSeconds(10.0f);
        deathCam.SetActive(false);
        mainCam.SetActive(true);
        HUDCanvas.SetActive(true);
        DeathCanvas.SetActive(false);
        //Vector3 pos = new Vector3(20.0f, 2.0f, 6.0f);
        //Instantiate(playerPrefab, pos, Quaternion.identity);
    }
}
