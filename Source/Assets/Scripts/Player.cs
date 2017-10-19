using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    int playerCode = 0;
    public int Code
    {
        get { return playerCode; }
        set { playerCode = playerCode == 0 ? value : playerCode; }
    } 

    GameController gc;
    SpriteRenderer _sprite;


    void Start()
    {
        
    }

    void OnTouch() {



    }


}
