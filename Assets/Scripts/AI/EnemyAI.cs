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
    private Animator animator;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 checkDirection;

    // Shooting variables
    private bool _readyToShoot;
    public bool ReadyToShoot
    {
        get{return _readyToShoot;}
        set{_readyToShoot = value;}
    }

    private float timeBetweenShots = 5.0f;
    [SerializeField] private float damage = 20;

    // New variables to give health to AI
    private float _health = 100.0f;
    public float Health
    {
        get{return _health;}
        set{_health = value;}
    }

    // Number of Allies
    private int numAllies;
    private GameObject retreatCoordinates;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    // Used for the AIManager
    private AIManager aiManager;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        AI = LayerMask.GetMask("AI");
        ReadyToShoot = true;
        //animator.SetInteger("NumAllies", numAllies);
        retreatCoordinates = GameObject.Find("RetreatPoint");
        aiManager = retreatCoordinates.GetComponent<AIManager>();
        animator.SetInteger("NumAllies", 8);
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

        if(Health <= 0)
        {
            Destroyed();
        }
    }

    public void MoveToPlayer(){
        navMeshAgent.SetDestination(player.transform.position);
    }

    public void Shoot()
    {
        if(Physics.Raycast(ray, out hit, maxRange, ~AI)) 
        {
            // if(hit.collider.gameObject == player)
            //Debug.Log("Hello");
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
        Debug.Log("Fuck off");
    }

    public void ReceiveDamage(float damage)
    {
        Health -= damage;
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
