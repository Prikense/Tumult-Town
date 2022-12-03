using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{    

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

    void Start()
    {
        Player1Score = 1;
        Player2Score = 1;
    }

}
