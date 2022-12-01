using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Fusion;

public class GameManagerO : NetworkBehaviour
{
    public static GameManagerO Instance;
    public event Action TimeOver;
    [Networked]public int timeCounter{get; set;}
    private int maxTime;
     private IEnumerator target;

    public override void Spawned(){

        if(Object.Runner.IsServer){
            maxTime = 120;
            timeCounter = maxTime;
        }    
    }
    
    void Awake()
    {
        Instance = this;
        
    }
    public void startTimerTime(){
        if(Object.Runner.IsServer){StartCoroutine(Timer());}
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

        //Invoke("GameOver", 1);
        

    }

    public override void FixedUpdateNetwork()
    {
        timeCounter=timeCounter;
    }

}
