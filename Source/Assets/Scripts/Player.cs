using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour {

    [SerializeField]
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
