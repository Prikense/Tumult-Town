using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    private int maxAmountAI = 8;

    private int _currAmountAI;
    [SerializeField] public int CurrAmountAI
    {
        get{return _currAmountAI;}
        set{_currAmountAI = value;}
    }

    [SerializeField] private GameObject enemyAIPrefab;
    [SerializeField] private GameObject spawnPoint; // going to be using this for now

    // ask the teacher how to better manage this one, for now I'll make it public
    public List<GameObject> AIList = new List<GameObject>();

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //animator = gameObject.GetComponent<Animator>();
        CurrAmountAI = maxAmountAI;
        SpawnAI(); 
    }

    // For now only being used to change amount, check if its better on Update
    void FixedUpdate()
    {
        if(CurrAmountAI != AIList.Count)
        {
            CurrAmountAI = AIList.Count;
            foreach(GameObject ai in AIList)
            {
                animator = ai.GetComponent<Animator>();
                animator.SetInteger("NumAllies", CurrAmountAI);

            }
            
        }  
    }

    void SpawnAI()
    {
        for(int i = maxAmountAI - CurrAmountAI; i < maxAmountAI; i++)
        {
            Vector3 newSpawnPosition = spawnPoint.transform.position + new Vector3(i * 5, 0, 0); //make this better, I mean the whole spawn process
            GameObject currentAI = Instantiate(enemyAIPrefab, newSpawnPosition, Quaternion.identity);
            AIList.Add(currentAI);
        }
    }
}

//notes
// this script is going to be in charge of spawning the AI
// as of now I may spawn them at the start of the game
// in the future they should spawn after certain time or when the score reaches a certain point
// most likely going to need a list of all the AI
// have a record of when they are destroyed and reduce the number
// looks like the tricky part will be Destroying the enemy once it is done
