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

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        animator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        AI = LayerMask.GetMask("AI");
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
        
    }
    public void MoveToPlayer(){
        navMeshAgent.SetDestination(player.transform.position);
    }

    public void Shoot(){
        Debug.Log("baleando time");
        navMeshAgent.SetDestination(transform.position);
        Debug.Log("bip bup coso shoot pos: "+transform.position);
    }

}
