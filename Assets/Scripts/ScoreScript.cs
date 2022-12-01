using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class ScoreScript : NetworkBehaviour
{
 
    [Networked] private int netPlayerScore1 {get; set;}
    [Networked] private int netPlayerScore2 {get; set;}
    

    private int _player1Score;
    public int Player1Score
    {
        get{return _player1Score;}
        set{_player1Score = value;}
    }

    private int _player2Score;

    public int Player2Score
    {
        get{return _player2Score;}
        set{_player2Score = value;}
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     Player1Score = 1;
    //     Player2Score = 1;
    // }

        public override void Spawned()
    {
        if(Object.Runner.IsServer){
            Player1Score = 1;
            Player2Score = 1;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if(Object.Runner.IsServer){
            netPlayerScore1 = _player1Score;
           netPlayerScore2 = _player2Score;
        }
    }

}
