using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private LayerMask AI;
    private float distanceFromPlayer;
    private bool isPlayerVisible;
    private float maxRange = 50.0f;
    private GameObject player;

    private GameObject[] players;
    private Animator animator;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 checkDirection;

    private ScoreScript score;

    // Shooting variables
    private bool _readyToShoot;
    public bool ReadyToShoot
    {
        get{return _readyToShoot;}
        set{_readyToShoot = value;}
    }

    private float timeBetweenShots = 5.0f;
    private float damage = 5.0f;

    // New variables to give health to AI
    /*
    private float _health = 100.0f;
    public float Health
    {
        get{return _health;}
        set{_health = value;}
    }
    */
    private float _aiHealth = 100.0f;

    // Number of Allies
    private int numAllies;
    private GameObject retreatCoordinates;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    // Used for the AIManager
    private AIManager aiManager;

    private GlobalHealthManager healthManager;

    // private var gORenderer; doesnt let use var globally


    // Start is called before the first frame update
    void Awake()
    {
        //player = GameObject.FindWithTag("Player");
        // players = GameObject.FindGameObjectsWithTag("Player");
        score = GameObject.Find("GameManager").GetComponent<ScoreScript>();

        if(score.Player1Score > score.Player2Score){
            player = GameObject.Find("Player1");
        } else{
            player = GameObject.Find("Player2");
        }

        animator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        AI = LayerMask.GetMask("AI");
        ReadyToShoot = true;
        //animator.SetInteger("NumAllies", numAllies);
        retreatCoordinates = GameObject.Find("RetreatPoint");
        aiManager = retreatCoordinates.GetComponent<AIManager>();
        animator.SetInteger("NumAllies", 8);

        // Implementing new Global Health Class
        healthManager = gameObject.GetComponent<GlobalHealthManager>(); 
        healthManager.Health = _aiHealth; 
        healthManager.MaxHealth = healthManager.Health;
        healthManager.HealthRatio = healthManager.Health / healthManager.MaxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        animator.SetFloat("distanceFromPlayer", distanceFromPlayer);

        checkDirection = player.transform.position - transform.position;
        ray = new Ray(transform.position, checkDirection);

        if (Physics.Raycast(ray, out hit, maxRange, ~AI)) {
            if(hit.collider.gameObject == player){
                animator.SetBool("isPlayerVisible", true);
            } else {
                animator.SetBool("isPlayerVisible", false);
            }
        } else {
            animator.SetBool("isPlayerVisible", false);
        }

        if(healthManager.Health <= 0)
        {
            Destroyed();
        }
    }

    public void MoveToPlayer(){
        navMeshAgent.SetDestination(player.transform.position);
    }

    public void Shoot()
    {
        float hitChance = Random.Range(0.0f, 10.0f);
        Debug.Log(hitChance);
        if(Physics.Raycast(ray, out hit, maxRange, ~AI) && hitChance < 3.0f) 
        {
            // Going to change the color of the ai material, when shot damages player
            var gORenderer = gameObject.GetComponent<Renderer>();
            gORenderer.material.SetColor("_Color", Color.red);
            StartCoroutine(BackToGreenColor());

            // if(hit.collider.gameObject == player)
            Debug.Log("Hello");
            PlayerManager playerManager = hit.transform.GetComponent<PlayerManager>();
            if(playerManager != null)
            {
                playerManager.ReceiveDamage(damage);
            }
        }
        ReadyToShoot = false;
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(timeBetweenShots);
        ReadyToShoot = true;
        //Debug.Log("RELOADING");
    }

    IEnumerator BackToGreenColor()
    {
        // Return color of AI back to original after 1 
        var gORenderer = gameObject.GetComponent<Renderer>();
        yield return new WaitForSeconds(1.0f);
        Color greenTankColor = new Color(40.0f/255.0f, 63.0f/255.0f, 2.0f/255.0f, 1.0f);
        gORenderer.material.SetColor("_Color", greenTankColor);

    }

    public void ReceiveDamage(float damage)
    {
        healthManager.Health -= damage;
        healthManager.HealthRatio = healthManager.Health / healthManager.MaxHealth;
    }

    void Destroyed()
    {
        //numAllies -= 1;
        //aiManager.CurrAmountAI -= 1;
        //animator.SetInteger("NumAllies", aiManager.CurrAmountAI);
        aiManager.AIList.Remove(gameObject);
        Destroy(gameObject);
    }

    public void Retreat()
    {
        navMeshAgent.SetDestination(retreatCoordinates.transform.position);
    }
}
