using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUIManagerO : MonoBehaviour
{
    [SerializeField] private AudioSource alarmsfx;
    [SerializeField] private Image playerHealthFill;
    [SerializeField] private Image targetHealthFill;
    [SerializeField] private Slider scoreBar;
    [SerializeField] private TextMeshProUGUI ammoCounter;
    [SerializeField] private TextMeshProUGUI objectiveHealth;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI ScorePlayer1;
    [SerializeField] private TextMeshProUGUI ScorePlayer2;
    [SerializeField] private ScoreScriptO scoreboard;
    [SerializeField] private GameManagerO gameManager;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Animator lightAlarm;

    // New values that are being used
    [SerializeField] private bool isProjectileWeapon;
    [SerializeField] public WeaponSwitchO weaponSwitch; // takes the script
    private GameObject currentWeapon;
    /*[SerializeField]*/ private ProjectileWeaponManagerO projectileWeapon;
    public WeaponManagerO raycastWeapon;
    [SerializeField] public PlayerManagerO PlayerHealth;

    [SerializeField] private Camera playerCamera;
    private float range = 100f;

    private int prevSelectedWeapon;

    //private BuildingManager prevBuildingManager;
    private GlobalHealthManagerO prevObjectHealthManager;

    
    private float prevHealth;

    // This line is only for testing, should be deleted later on
    [SerializeField] public ProjectileWeaponManagerO getProjectileWeapon;

    


    //timer for flavor texts of the sword, can be done better but who cares
    private float cooldown = 10;
    private float timer = 0;
    private float timerSword =0;
    int auxRNG;
    private bool gameOverTime=false;
    

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        
        // gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // scoreboard = GameObject.Find("GameManager").GetComponent<ScoreScript>();

        winScreen.SetActive(false);
        loseScreen.SetActive(false);


        //GameManagerO.Instance.TimeOver += OnGameOver; 
        timerSword = 0;
        auxRNG= Random.Range(0, 10);
        objectiveHealth.text = "No data";
        // New
        currentWeapon = weaponSwitch.CurrentWeapon;

        if(currentWeapon.GetComponent<ProjectileWeaponManager>() != null) 
        {
            isProjectileWeapon = true;
            projectileWeapon = currentWeapon.GetComponent<ProjectileWeaponManagerO>();
            ammoCounter.text = projectileWeapon.MagazineSize + " / " + projectileWeapon.MagazineSize;
        } else
        {
            isProjectileWeapon = false;
            raycastWeapon = currentWeapon.GetComponent<WeaponManagerO>();
            ammoCounter.text = raycastWeapon.MagazineSize + " / " + raycastWeapon.MagazineSize;
        }
        // This line is only for testing, should be deleted later on (used to show bullets of third weapon)
        projectileWeapon = getProjectileWeapon;

        getProjectileWeapon = null;
        prevHealth = 0.0f;


        
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOverTime){
            string min;
            string sec;
            if(gameManager.timeCounter % 60 < 10){
                sec = "0"+ gameManager.timeCounter % 60;
            }else{
                sec = ""+ gameManager.timeCounter % 60;
            }
            if(gameManager.timeCounter / 60 < 10){
                min = "0"+ gameManager.timeCounter / 60;
            }else{
                min = ""+ gameManager.timeCounter / 60;
            }
            timeText.text = "" + min + ":" + sec;
            
            scoreBar.value = (float)scoreboard.Player1Score / (scoreboard.Player1Score + scoreboard.Player2Score);
            
            ScorePlayer1.text = "" + (scoreboard.Player1Score - 1);
            ScorePlayer2.text = "" + (scoreboard.Player2Score - 1);
            
            if(weaponSwitch.SelectedWeapon == 0){
                if(!raycastWeapon.DoneReloading){
                    ammoCounter.text ="-Reloading-";
                }else {
                    ammoCounter.text =raycastWeapon.AmmoLeft + " / " + raycastWeapon.MagazineSize;
                }
            }else if(weaponSwitch.SelectedWeapon == 1){
                timerSword += Time.deltaTime;
                // auxRNG = 0;
                if(timerSword > cooldown){
                    auxRNG= Random.Range(0, 10);
                    timerSword = 0;
                }

                switch (auxRNG){
                    case 0:
                        ammoCounter.text ="Sword Time";
                        break;
                    case 1:
                        ammoCounter.text ="Destroy 'em";
                        break;
                    case 2:
                        ammoCounter.text = "Let's Rock!";
                        break;
                    case 3:
                        ammoCounter.text = "Clang Time";
                        break;
                    case 4:
                        ammoCounter.text = "Hew the Stone!";
                        break;
                    case 5:
                        ammoCounter.text = "A Red Day!";
                        break;
                    case 6:
                        ammoCounter.text = "Break It";
                        break;
                    case 7:
                        ammoCounter.text = "watchoutwatchout";
                        break;
                    case 8:
                        ammoCounter.text = "Show them";
                        break;
                    case 9:
                        ammoCounter.text = "Ride the Lighting!";
                        break;
                    case 10:
                        ammoCounter.text = "KIll 'em All!";
                        break;
                }
            }else if(weaponSwitch.SelectedWeapon == 2){
                if(!projectileWeapon.DoneReloading){
                    ammoCounter.text ="-Reloading-";
                }else {
                    ammoCounter.text =projectileWeapon.AmmoLeft + " / " + projectileWeapon.MagazineSize;
                    }
            }
        }
    }

    void FixedUpdate()
    {
        if(!gameOverTime){
            ObtainObjectHealth();
            if(gameManager.timeCounter == 0){OnGameOver();}
        }
    }

    void ObtainObjectHealth()
    {

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {

            //BuildingManager buildingManager = hit.transform.GetComponent<BuildingManager>();
            GlobalHealthManagerO objectHealthManager = hit.transform.GetComponent<GlobalHealthManagerO>(); 
            float currentHealth = 0.0f; 
            // this if and else if is currently working but there may be a cleaner way of doing this
            if(objectHealthManager == null && prevObjectHealthManager == null) // i dont think this makes sense
            {
                currentHealth = 0.0f;
                objectiveHealth.text = "" + 0; 
            } 
            else if(objectHealthManager != null)
            {
                currentHealth = objectHealthManager.Health;
            } 
            // maybe the prevBuildingManager isn't necessary anymore since it now compares the health so if you stay looking the same its the same effect
            if(objectHealthManager != null && currentHealth != prevHealth) 
            { 
                //Debug.Log("got one");
                objectiveHealth.text = "" + (int)objectHealthManager.Health;            
                FillBar(targetHealthFill, objectHealthManager.HealthRatio); 
                prevObjectHealthManager = objectHealthManager;
                prevHealth = currentHealth;
            }
        if((int)PlayerHealth.healthManager.Health < 0){
            health.text = "0";
        }else{
            health.text = "" + (int)PlayerHealth.healthManager.Health;
        }
        FillBar(playerHealthFill, PlayerHealth.healthManager.HealthRatio);

        timer += Time.fixedDeltaTime;
        if(timer >= (7f/6f)){
            timer = 0;
        }
        if(PlayerHealth.healthManager.HealthRatio <= .25f){
            lightAlarm.SetBool("lowHealth", true);
            if(!alarmsfx.isPlaying && timer > (.36f) && timer < .5){
                alarmsfx.Play();
            }
        }
        else
            lightAlarm.SetBool("lowHealth", false);
        }
    }

    void ProjectileWeapon()
    {
         
    }

    void RaycastWeapon()
    {

    }

    void FillBar(Image image, float fillAmount){
       image.fillAmount = fillAmount;
    }

    /* IEnumerator Timer(){

        yield return new WaitForSeconds(1);
        timeCounter ++;
    } */
    void OnGameOver()
    {
    gameOverTime=true;
      string thisPlayer = gameObject.transform.root.name;
      string winner;
      gameObject.transform.root.GetComponent<CharacterMovementHandler>().stopGaem();
      gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        //Comparar score
      if(scoreboard.netPlayerScore1 > scoreboard.netPlayerScore2){
            winner = "Player1(Online)(Clone)";
      }
      else
        winner = "Player2(Online)(Clone)";

        if(thisPlayer == winner){
            winScreen.SetActive(true);
            
            //canvasManager.SetActiveScreen(winScreen);
        }
        else
            loseScreen.SetActive(true);
            //canvasManager.SetActiveScreen(loseScreen);
    }
}