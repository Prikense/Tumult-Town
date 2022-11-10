using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action TimeOver;
    public int timeCounter;
    private int maxTime;
     private IEnumerator target;

    void Start(){

        maxTime = 10;
        timeCounter = maxTime;    
        
        StartCoroutine(Timer());
    }
    
    void Awake()
    {
        Instance = this;
        
    }

    // Update is called once per frame
    void GameOver()
    {
        TimeOver?.Invoke();
    }
    
    IEnumerator Timer(){
        while(timeCounter > 0){
            yield return new WaitForSeconds(1);
            timeCounter --;
        }

        Invoke("GameOver", 1);
        

    }
    
   /*  public int timeCounter
    {
        get{return _timeCounter;}
        set{_timeCounter = value;}
    } */

}
