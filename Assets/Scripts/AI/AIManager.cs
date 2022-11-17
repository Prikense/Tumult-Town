using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    private int maxAmountAI = 8;

    private int _currAmountAI;
    public int CurrAmountAI
    {
        get{return _currAmountAI;}
        set{_currAmountAI = value;}
    }

    [SerializeField] private GameObject enemyAIPrefab;
    [SerializeField] private GameObject spawnPoint; // going to use this for now
    [SerializeField] private GameObject spawnPoint2;

    // ask the teacher how to better manage this one, for now I'll make it public
    public List<GameObject> AIList = new List<GameObject>();

    private Animator animator;

    private bool canRespawn;

    // Start is called before the first frame update
    void Start()
    {
        //animator = gameObject.GetComponent<Animator>();
        CurrAmountAI = 0;
        canRespawn = false;
        StartCoroutine(FirstTimeSpawn());
    }

    // For now only being used to change amount, check if its better on Update
    void FixedUpdate()
    {
        // This loop is in charge of updating the var NumAllies of each enemy
        if(CurrAmountAI != AIList.Count)
        {
            CurrAmountAI = AIList.Count;
            foreach(GameObject ai in AIList)
            {
                animator = ai.GetComponent<Animator>();
                animator.SetInteger("NumAllies", CurrAmountAI);

            }
            
        }

        // Not the biggest fan of respawning enemies this way, but it is what it is for now
        if(CurrAmountAI < 3 && AIList.Count > 0 && canRespawn)
        {
            StartCoroutine(Reinforcements());
            canRespawn = false;
        }  
    }

    IEnumerator FirstTimeSpawn()
    {
        yield return new WaitForSeconds(20.0f);
        // CurrAmountAI = maxAmountAI;
        SpawnAI();
    }

    IEnumerator Reinforcements()
    {
        yield return new WaitForSeconds(12.0f);
        SpawnAI();
    }

    void SpawnAI()
    {
        Debug.Log("Spawning enemies");
        Debug.Log($"Max amount {maxAmountAI}");
        Debug.Log($"CurrAmountAI {CurrAmountAI}");
        for(int i = CurrAmountAI; i < maxAmountAI; i++)
        {
            Debug.Log("Spawn one");
            Vector3 newSpawnPosition;
            if(i%2 == 0){
                newSpawnPosition = spawnPoint.transform.position + new Vector3(i * 2.5f, 0, 0); //make this better, I mean the whole spawn process
            } else{
                newSpawnPosition = spawnPoint2.transform.position + new Vector3(i * 2.5f, 0, 0); //make this better, I mean the whole spawn process
            }
            GameObject currentAI = Instantiate(enemyAIPrefab, newSpawnPosition, Quaternion.identity);
            AIList.Add(currentAI);
        }
        CurrAmountAI = maxAmountAI;
        canRespawn = true;
    }
}

//notes
// this script is going to be in charge of spawning the AI
// as of now I may spawn them at the start of the game
// in the future they should spawn after certain time or when the score reaches a certain point
// most likely going to need a list of all the AI
// have a record of when they are destroyed and reduce the number
// looks like the tricky part will be Destroying the enemy once it is done
